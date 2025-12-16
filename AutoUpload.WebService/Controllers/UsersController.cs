using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoUpload.WebService.HashIds;
using AutoUpload.WebService.Models;
using AutoUpload.WebService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace AutoUpload.WebService.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private const string CodeGenerationSalt = "L!ceNse Co6e 9en3raTi0n $4lt";

        private readonly AutoUploadContext _autoUploadContext;
        private readonly ILogger<UsersController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfigurationRoot _configuration;

        public UsersController(AutoUploadContext autoUploadContext, ILogger<UsersController> logger, IHttpContextAccessor httpContextAccessor, IConfigurationRoot configuration)
        {
            _autoUploadContext = autoUploadContext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<dynamic> Register([FromBody] RegisterModel register)
        {
            if (register?.Email == null)
            {
                return NotFound();
            }

            var user = _autoUploadContext.Users.FirstOrDefault(x => x.Email == register.Email.ToLower());
            if (user == null)
            {
                user = new User()
                       {
                           CreatedOn = DateTime.Now,
                           Email = register.Email.ToLower(),
                           MachineId = null,
                           UpdatedOn = DateTime.Now,
                           IsDeleted = false,
                       };
                _autoUploadContext.Users.Add(user);
                try
                {
                    await _autoUploadContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return new
                           {
                               Error = 500,
                               License = "System got problems right now. Please come back after a while"
                           };
                }
            }

            var registeredLicenses = _autoUploadContext.Licenses
                                                       .Where(l => l.UserId == user.Id && !l.IsDeleted)
                                                       .ToList();
            var registeredPlatforms = registeredLicenses
                .SelectMany(l => (l.ValidPlatforms ?? "").ToLower().Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                .ToList();

            var platforms = (register.Platforms ?? "").ToLower().Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();
            var reRegisterPlatforms = new List<string>();
            try
            {
                // Trim all existed licenses
                foreach (var platform in registeredPlatforms)
                {
                    if (platforms.Contains(platform))
                    {
                        platforms.Remove(platform);
                        reRegisterPlatforms.Add(platform);
                    }
                }

                var licenseKey = registeredLicenses.Count > 0 ? registeredLicenses[0].LicenseKey : Guid.NewGuid().ToString().ToUpper();

                if (reRegisterPlatforms.Count > 0)
                {
                    var license = new License
                                  {
                                      CreatedOn = DateTime.Now,
                                      IsDeleted = true,
                                      UpdatedOn = DateTime.Now,
                                      ValidPlatforms = string.Join(",", reRegisterPlatforms),
                                      LicenseKey = licenseKey,
                                      UserId = user.Id,
                                      ValidFrom = DateTime.Now,
                                      ValidTo = DateTime.Now.Date.AddMonths(1).AddDays(1).AddTicks(-1),
                                      IsTrial = true
                                  };
                    _autoUploadContext.Licenses.Add(license);
                    await _autoUploadContext.SaveChangesAsync();
                }

                if (platforms.Count > 0)
                {
                    var license = new License
                                  {
                                      CreatedOn = DateTime.Now,
                                      IsDeleted = false,
                                      UpdatedOn = DateTime.Now,
                                      ValidPlatforms = string.Join(",", platforms),
                                      LicenseKey = licenseKey,
                                      UserId = user.Id,
                                      ValidFrom = new DateTime(2000, 1, 1),
                                      ValidTo = new DateTime(2000, 1, 1),
                                      IsTrial = true
                                  };
                    _autoUploadContext.Licenses.Add(license);
                    await _autoUploadContext.SaveChangesAsync();
                }

                return new
                       {
                           Error = 0,
                           License = licenseKey
                };
            }
            catch (Exception)
            {
                return new
                       {
                           Error = 500,
                           License = "System got problems right now. Please come back after a while"
                       };
            }
        }

        [HttpPost("checklicense")]
        public async Task<dynamic> CheckLicense([FromBody] LicenseModel licenseModel)
        {
            // Check integrity of information and return a group of {code, encrypted chunk}
            // App must use this data to patch before can run correctly

            // Validate
            var validation = await ValidateLicense(licenseModel);
            if (validation.Error != 0)
            {
                return new
                {
                    Error = validation.Error,
                    Message = validation.Message
                };
            }

            User user = validation.User;
            List<License> activeLicenses = validation.Licenses;

            // Get most platform license
            var mostLicense = activeLicenses
                .OrderByDescending(x => (x.ValidPlatforms ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length)
                .ThenByDescending(x => x.ValidTo)
                .First();

            // Generate code
            var hashids = new Hashids(CodeGenerationSalt);
            var log = new Log
                      {
                          UserId = user.Id,
                          LicenseId = mostLicense.Id,
                          CreatedOn = DateTime.Now,
                          IpAddress = GetIpAddress(),
                          Code = hashids.EncodeLong(DateTime.Now.Ticks)
                      };
            _autoUploadContext.Logs.Add(log);
            try
            {
                await _autoUploadContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                _logger.LogError(new EventId(1, "AutoUpload"), exc, exc.Message);
                return new
                       {
                           Error = 500,
                           Message = "System failed (cannot write to database)"
                       };
            }

            // Generate chunk data
            var platforms = activeLicenses.SelectMany(x => (x.ValidPlatforms ?? "").ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                .ToArray();
            var chunk = new Dictionary<string, object>
                        {
                            { "code", log.Code },
                            { "is_trial", activeLicenses.All(x => x.IsTrial) },
                            { "delay", _configuration["Settings:DelayBetween"] ?? "0" },
                            { "delay_sunfrog", _configuration["Settings:DelayBetweenSunfrog"] ?? "0" },
                            { "delay_teechip", _configuration["Settings:DelayBetweenTeechip"] ?? "0" },
                            { "delay_teespring", _configuration["Settings:DelayBetweenTeespring"] ?? "0" },
                            { "delay_viralstyle", _configuration["Settings:DelayBetweenViralstyle"] ?? "0" },
                            { "delay_teezily", _configuration["Settings:DelayBetweenTeezily"] ?? "0" }
                        };
            if (platforms.Contains("sunfrog"))
            {
                chunk.Add(
                          "sunfrog",
                          new
                          {
                              imageFront =
                              "<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" viewBox=\"{0} {1} {2} {3}\" height=\"{4}\" width=\"{5}\" version=\"1.1\" id=\"SvgjsSvg1000\"><g transform=\"scale({6} {7}) translate({8} {9})\" id=\"SvgjsG1052\"><image height=\"{10}\" width=\"{11}\" xlink:href=\"__dataURI:0__\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" id=\"SvgjsImage1053\"></image></g><defs id=\"SvgjsDefs1001\"></defs></svg>",
                              imageBack =
                              "<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" viewBox=\"{0} {1} {2} {3}\" height=\"{4}\" width=\"{5}\" version=\"1.1\" id=\"SvgjsSvg1006\"><g transform=\"scale({6} {7}) translate({8} {9})\" id=\"SvgjsG1054\"><image height=\"{10}\" width=\"{11}\" xlink:href=\"__dataURI:1__\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" id=\"SvgjsImage1055\"></image></g><defs id=\"SvgjsDefs1007\"></defs></svg>",
                              refreshAccount = (_configuration["Settings:SunfrogRefreshAccount"] ?? "False").ToLower() == "true",
                              isTrial = activeLicenses.Any(x => (x.ValidPlatforms ?? "").ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Contains("sunfrog") && x.IsTrial)
                          });
            }
            if (platforms.Contains("teespring"))
            {
                chunk.Add(
                          "teespring",
                          new
                          {
                              frontSvg =
                              @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" width =""666px"" height =""1000px"" viewBox =""{0} {1} {2} {3}""><defs data-element=""defs""><clipPath data-element=""globalClipPath"" id =""global-clip-path-front-4""><rect x=""{4}"" y=""{5}"" width=""{6}"" height=""{7}"" data-element=""globalClipRect""></rect></clipPath></defs><g data-element=""rootTag"" clip-path=""url(#global-clip-path-front-4)""><g data-id=""front-layer-0"" data-width=""{8}"" data-height=""{9}"" transform=""matrix({10}, {11}, {12}, {13}, {14}, {15})""><image data-imageuri=""{16}"" xlink:href=""{17}"" width=""{18}"" height=""{19}""></image></g></g></svg>",
                              frontEmptySvg =
                              @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" width =""666px"" height =""1000px"" viewBox =""{0} {1} {2} {3}""><defs data-element=""defs""><clipPath data-element=""globalClipPath"" id =""global-clip-path-front-4""><rect x=""{4}"" y=""{5}"" width=""{6}"" height=""{7}"" data-element=""globalClipRect""></rect></clipPath></defs><g data-element=""rootTag"" clip-path=""url(#global-clip-path-front-4)""></g></svg>",
                              backSvg =
                              @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" width =""651px"" height =""1000px"" viewBox =""{0} {1} {2} {3}""><defs data-element=""defs""><clipPath data-element=""globalClipPath"" id =""global-clip-path-front-5""><rect x=""{4}"" y=""{5}"" width=""{6}"" height=""{7}"" data-element=""globalClipRect""></rect></clipPath></defs><g data-element=""rootTag"" clip-path=""url(#global-clip-path-front-5)""><g data-id=""front-layer-0"" data-width=""{8}"" data-height=""{9}"" transform=""matrix({10}, {11}, {12}, {13}, {14}, {15})""><image data-imageuri=""{16}"" xlink:href=""{17}"" width=""{18}"" height=""{19}""></image></g></g></svg>",
                              backEmptySvg =
                              @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" width =""651px"" height =""1000px"" viewBox =""{0} {1} {2} {3}""><defs data-element=""defs""><clipPath data-element=""globalClipPath"" id =""global-clip-path-front-5""><rect x=""{4}"" y=""{5}"" width=""{6}"" height=""{7}"" data-element=""globalClipRect""></rect></clipPath></defs><g data-element=""rootTag"" clip-path=""url(#global-clip-path-front-5)""></g></svg>",
                              refreshAccount = (_configuration["Settings:TeespringRefreshAccount"] ?? "False").ToLower() == "true",
                              isTrial = activeLicenses.Any(x => (x.ValidPlatforms ?? "").ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Contains("teespring") && x.IsTrial)
                          });
            }
            if (platforms.Contains("teechip"))
            {
                chunk.Add(
                          "teechip",
                          new
                          {
                              frontColorCount = 11,
                              frontSide = "front",
                              frontTypeGarment = "garment",
                              frontTypeMug = "mug",
                              frontTypeCase = "case",
                              frontTypePoster = "poster",
                              backColorCount = 11,
                              backSide = "back",
                              backTypeGarment = "garment",
                              refreshAccount = (_configuration["Settings:TeechipRefreshAccount"] ?? "False").ToLower() == "true",
                              isTrial = activeLicenses.Any(x => (x.ValidPlatforms ?? "").ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Contains("teechip") && x.IsTrial)
                          });
            }
            if (platforms.Contains("viralstyle"))
            {
                chunk.Add(
                          "viralstyle",
                          new
                          {
                              frontWidth = 1090,
                              frontHeight = 1875,
                              frontMultiplier = 5,
                              frontId = "item-front-0",
                              backWidth = 1090,
                              backHeight = 1875,
                              backMultiplier = 5,
                              backId = "item-back-0",
                              refreshAccount = (_configuration["Settings:ViralstyleRefreshAccount"] ?? "False").ToLower() == "true",
                              isTrial = activeLicenses.Any(x => (x.ValidPlatforms ?? "").ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Contains("viralstyle") && x.IsTrial)
                          });
            }
            if (platforms.Contains("teezily"))
            {
                chunk.Add("teezily",
                    new
                    {
                        applicationId = _configuration["Settings:TeezilyApplicationId"] ?? "",
                        applicationSecret = _configuration["Settings:TeezilyApplicationSecret"] ?? "",
                        refreshAccount = (_configuration["Settings:TeezilyRefreshAccount"] ?? "False").ToLower() == "true",
                        isTrial = activeLicenses.Any(x => (x.ValidPlatforms ?? "").ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Contains("teezily") && x.IsTrial)
                    });
            }
            if (platforms.Contains("mockup"))
            {
                chunk.Add("mockup",
                    new
                    {
                        key = _configuration["Settings:MockupEncryptionKey"] ?? "",
                        iv = _configuration["Settings:MockupEncryptionInitialVector"] ?? "",
                        isTrial = activeLicenses.Any(x => (x.ValidPlatforms ?? "").ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Contains("mockup") && x.IsTrial)
                    });
            }

            var chunkAsString = JsonConvert.SerializeObject(chunk);
            byte[] key;
            byte[] iv;
            var encodedChunk = Encode(chunkAsString, out key, out iv);

            return new
                   {
                       Error = 0,
                       Code = log.Code,
                       Chunk = encodedChunk,
                       Key = Convert.ToBase64String(key),
                       Initial = Convert.ToBase64String(iv),
                       ExpiredTime = mostLicense.ValidTo.Ticks,
                       Message = _configuration["Settings:Message"] ?? ""
                   };
        }

        [HttpGet("checkupdate")]
        public string CheckUpdate()
        {
            return $@"{_configuration["Settings:Version"] ?? "1.0"}={Url.Action("Download", "Users", null, "http")}";
        }

        [HttpGet("download")]
        public IActionResult Download()
        {
            var filePath = _configuration["Settings:ZipFile"] ?? @"C:\autoupload\autoupload.zip";
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(fileStream, "application/octet-stream", Path.GetFileName(filePath));
        }

        [HttpPost("mockuptemplates")]
        public async Task<dynamic> MockupTemplates([FromBody] LicenseModel licenseModel)
        {
            // Validate
            var validation = await ValidateLicense(licenseModel);
            if (validation.Error != 0)
            {
                return new
                {
                    Error = validation.Error,
                    Message = validation.Message
                };
            }

            User user = validation.User;
            List<License> activeLicenses = validation.Licenses;

            var platforms = activeLicenses.SelectMany(x => (x.ValidPlatforms ?? "").ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                .ToArray();
            if (!platforms.Contains("mockup"))
            {
                return new
                {
                    Error = 400,
                    Message = "This license is not valid for this feature"
                };
            }

            var hashids = new Hashids(CodeGenerationSalt);
            var mockups = await _autoUploadContext.MockupTemplates
                .Where(x => x.User == null || x.User.Id == user.Id)
                .ToListAsync();

            return new
            {
                Error = 0,
                List = mockups.Select(x => new
                {
                    Id = hashids.EncodeLong(x.Id),
                    Name = x.Name,
                    Category = x.Category,
                    UpdatedOn = x.UpdatedOn.Ticks
                })
            };
        }

        [HttpGet("downloadmockup/{id}")]
        public IActionResult DownloadMockup(string id)
        {
            var hashids = new Hashids(CodeGenerationSalt);
            var mockupId = hashids.DecodeLong(id).FirstOrDefault();

            var folder = _configuration["Settings:MockupFolder"] ?? @"C:\autoupload\mockups\";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var mockupTemplate = _autoUploadContext.MockupTemplates.FirstOrDefault(x => x.Id == mockupId);
            if (mockupTemplate == null || !System.IO.File.Exists(Path.Combine(folder, mockupTemplate.FilePath)))
            {
                return NotFound();
            }

            var filePath = Path.Combine(folder, @"compressed\" + mockupTemplate.FilePath);
            if (!System.IO.File.Exists(filePath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }

                try
                {
                    // Compress file with password if need
                    var key = Convert.FromBase64String(_configuration["Settings:MockupEncryptionKey"] ?? "");
                    var iv = Convert.FromBase64String(_configuration["Settings:MockupEncryptionInitialVector"] ?? "");
                    var encryptedBytes = (new Random((int)DateTime.Now.Ticks)).Next(4096, 8193);
                    var buffer = new byte[encryptedBytes];
                    using (var originalFileStream = new FileStream(Path.Combine(folder, mockupTemplate.FilePath), FileMode.Open))
                    using (var outputFileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        var readBytes = originalFileStream.Read(buffer, 0, encryptedBytes);
                        var encryptedBuffer = Encode(buffer, key, iv);

                        byte[] intBytes = BitConverter.GetBytes(encryptedBuffer.Length);
                        if (BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(intBytes);
                        }
                        outputFileStream.Write(intBytes, 0, intBytes.Length);
                        outputFileStream.Write(encryptedBuffer, 0, encryptedBuffer.Length);

                        // Continue to read until end
                        while (readBytes > 0 && originalFileStream.CanRead)
                        {
                            readBytes = originalFileStream.Read(buffer, 0, buffer.Length);
                            if (readBytes > 0)
                            {
                                outputFileStream.Write(buffer, 0, readBytes);
                            }
                        }

                        outputFileStream.Flush();
                    }
                }
                catch(Exception exc)
                {
                    _logger.LogError(new EventId(1, "AutoUpload"), exc, exc.Message);
                }
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(fileStream, "application/octet-stream", Path.GetFileName(filePath));
        }

        [HttpGet("downloadmockupthumbnail/{id}")]
        public IActionResult DownloadMockupThumbnail(string id)
        {
            var hashids = new Hashids(CodeGenerationSalt);
            var mockupId = hashids.DecodeLong(id).FirstOrDefault();

            var folder = _configuration["Settings:MockupFolder"] ?? @"C:\autoupload\mockups\";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var mockupTemplate = _autoUploadContext.MockupTemplates.FirstOrDefault(x => x.Id == mockupId);
            var filePath = Path.Combine(folder, mockupTemplate.ThumbnailPath);
            if (mockupTemplate == null || !System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(fileStream, "application/octet-stream", Path.GetFileName(filePath));
        }

        private async Task<dynamic> ValidateLicense(LicenseModel licenseModel)
        {
            if (!licenseModel.Checksum.Equals(_configuration["Settings:Checksum"] ?? CodeGenerationSalt, StringComparison.CurrentCultureIgnoreCase))
            {
                return new
                {
                    Error = 500,
                    Message = "Application Checksum is incorrect. Program has new changes. Please restart to get update."
                };
            }

            var user = await _autoUploadContext.Users
                            .Include(x => x.Licenses)
                            .FirstOrDefaultAsync(x => !x.IsDeleted && x.Email == licenseModel.Email.ToLower());
            if (user == null)
            {
                return new
                {
                    Error = 404,
                    Message = "Email Not Found"
                };
            }

            if (user.MachineId == null && licenseModel.MachineId != null)
            {
                // First time then assign machine id to user
                //var oldMachineId = await _autoUploadContext.Users.FirstOrDefaultAsync(x => x.MachineId.ToLower() == licenseModel.MachineId.ToLower());
                //if (oldMachineId != null)
                //{
                //    // This Machine ID is already registered with another email
                //    return new
                //    {
                //        Error = 400,
                //        Message = "This machine is already registered with another email."
                //    };
                //}

                user.MachineId = licenseModel.MachineId;
                user.UpdatedOn = DateTime.Now;
            }
            else if (user.MachineId == null || !user.MachineId.Equals(licenseModel.MachineId, StringComparison.CurrentCultureIgnoreCase))
            {
                return new
                {
                    Error = 400,
                    Message = "This email is already used on another machine"
                };
            }

            var licenses = user.Licenses?.Where(x => !x.IsDeleted && x.LicenseKey.Equals(licenseModel.LicenseKey, StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (licenses == null || licenses.Count == 0)
            {
                return new
                {
                    Error = 404,
                    Message = "License Not Found"
                };
            }

            foreach (var license in licenses)
            {
                if (license.ValidFrom.Year < 2010)
                {
                    license.ValidFrom = DateTime.Now;
                    license.ValidTo = DateTime.Now.AddDays(3);
                    try
                    {
                        await _autoUploadContext.SaveChangesAsync();
                    }
                    catch (Exception exc)
                    {
                    }
                }
            }

            var activeLicenses = licenses.Where(license => license.ValidFrom <= DateTime.Now && license.ValidTo >= DateTime.Now).ToList();
            if (activeLicenses.Count == 0)
            {
                return new
                {
                    Error = 400,
                    Message = "This license is not activated at current time"
                };
            }

            return new
            {
                Error = 0,
                User = user,
                Licenses = activeLicenses
            };
        }

        private string GetIpAddress(bool tryUseXForwardHeader = true)
        {
            var ip = "";

            if (tryUseXForwardHeader)
            {
                ip = GetHeaderValue("X-Forwarded-For")
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .FirstOrDefault();
            }

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrWhiteSpace(ip) && _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
            {
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }

            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
            }

            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = GetHeaderValue("REMOTE_ADDR");
            }

            return ip;
        }

        private string GetHeaderValue(string headerName)
        {
            StringValues values = "";
            if (_httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                var rawValues = values.ToString();

                if (!string.IsNullOrEmpty(rawValues))
                {
                    return rawValues;
                }
            }

            return "";
        }

        private static string Encode(string data, out byte[] key, out byte[] iv)
        {
            var valueBytes = Encoding.UTF8.GetBytes(data);
            using (var cipher = Aes.Create())
            {
                cipher.GenerateIV();
                cipher.GenerateKey();

                key = cipher.Key;
                iv = cipher.IV;
            }

            return Convert.ToBase64String(Encode(valueBytes, key, iv));
        }

        public static byte[] Encode(byte[] array, byte[] key, byte[] iv)
        {
            byte[] encrypted;
            using (var cipher = Aes.Create())
            {
                cipher.Key = key;
                cipher.IV = iv;

                cipher.Mode = CipherMode.CBC;
                cipher.Padding = PaddingMode.PKCS7;

                using (var encryptor = cipher.CreateEncryptor(cipher.Key, cipher.IV))
                {
                    using (var to = new MemoryStream())
                    {
                        using (var writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(array, 0, array.Length);
                            writer.FlushFinalBlock();
                            encrypted = to.ToArray();
                        }
                    }
                }
            }

            return encrypted;
        }

        public static byte[] Decode(byte[] array, byte[] key, byte[] iv)
        {
            byte[] decrypted;
            using (var cipher = Aes.Create())
            {
                cipher.Key = key;
                cipher.IV = iv;
                cipher.Mode = CipherMode.CBC;
                cipher.Padding = PaddingMode.PKCS7;

                using (var decryptor = cipher.CreateDecryptor(cipher.Key, cipher.IV))
                {
                    using (var to = new MemoryStream())
                    {
                        using (var writer = new CryptoStream(to, decryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(array, 0, array.Length);
                            writer.FlushFinalBlock();
                            decrypted = to.ToArray();
                        }
                    }
                }
            }

            return decrypted;
        }
    }
}
