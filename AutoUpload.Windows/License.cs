using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using AutoUpload.Shared;
using Newtonsoft.Json.Linq;
using AutoUpload.Photoshop;

namespace AutoUpload.Windows
{
    public static class License
    {
        public static readonly string CheckLicenseUrl = "http://autoupload.teeinsight.com/api/users/checklicense";

        public static string Email { get; set; }

        public static string LicenseKey { get; set; }

        public static string Code { get; set; }

        public static string Chunk { get; set; }

        public static string IV { get; set; }

        public static string Key { get; set; }

        public static DateTime ExpiredTime { get; set; }

        public static string Message { get; set; }

        public static string CheckLicense(string email, string license)
        {
            var md5String = GetFileChecksum();
            if (string.IsNullOrEmpty(md5String))
            {
                return "Cannot access to file.";
            }

            var machineId = GetMachineId();
#if DEBUG
            md5String = "B0A1-6552-36D3-CCB7-04E5-0812-63F7-DB64";
            
            // For testing
            //email = "doanminh90@gmail.com";
            //license = "7A9EF849-06BB-40F3-8B99-3E4BBC81DD4B";
            //machineId = "4DF0-675F-C846-9517-9D57-A114-C836-4926";
#endif

            var result = Common.PostJson<JToken>(
                                                 CheckLicenseUrl,
                                                 new Dictionary<string, string>
                                                 {
                                                     { "Email", email },
                                                     { "LicenseKey", license },
                                                     { "MachineId", machineId },
                                                     { "Checksum", md5String }
                                                 },
                                                 new CookieContainer());

            if (result == null)
            {
                return "Cannot access to Internet. Please check your network connection.";
            }

            if (result["error"].ToObject<int>() != 0)
            {
                return result["message"].ToObject<string>();
            }

            // Assign
            Email = email;
            LicenseKey = license;
            Code = result["code"].ToObject<string>();
            Chunk = result["chunk"].ToObject<string>();
            Key = result["key"].ToObject<string>();
            IV = result["initial"].ToObject<string>();
            ExpiredTime = result["expiredTime"] != null ? new DateTime(result["expiredTime"].ToObject<long>()) : DateTime.Now;
            Message = result["message"] != null ? result["message"].ToObject<string>() : "";

            return "";
        }

        public static string DecodeChunk(string chunk, string key, string iv)
        {
            try
            {
                var buffer = Convert.FromBase64String(chunk);
                var decrypted = Decode(buffer, key, iv);
                return Encoding.UTF8.GetString(decrypted);
            }
            catch (Exception exc)
            {
                return "";
            }
        }

        public static byte[] Decode(byte[] array, string key, string iv)
        {
            byte[] decrypted;
            using (var cipher = Aes.Create())
            {
                cipher.Key = Convert.FromBase64String(key);
                cipher.IV = Convert.FromBase64String(iv);
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

        public static string GetKey(string code, string machineId, string checksum, string licenseKey)
        {
            return $@"{code}.{machineId}.{checksum}.{licenseKey}";
        }

        public static string GetMachineId()
        {
            var machineId = $@"CPU >> {CpuId()}
BIOS >> {BiosId()}
BASE >> {BaseId()}
VIDEO >> {VideoId()}
MAC >> {MacId()}";
            using (var md5 = MD5.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(machineId);
                var md5Hash = md5.ComputeHash(buffer, 0, buffer.Length);
                return GetHexString(md5Hash);
            }
        }

        public static string GetFileChecksum()
        {
            try
            {
                var file = File.Open(Application.ExecutablePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var buffer = new byte[file.Length];
                file.Read(buffer, 0, buffer.Length);
                using (var md5 = MD5.Create())
                {
                    var md5Hash = md5.ComputeHash(buffer, 0, buffer.Length);
                    return GetHexString(md5Hash);
                }
            }
            catch (Exception exc)
            {
                return "";
            }
        }

        private static string GetHexString(byte[] bt)
        {
            var s = string.Empty;
            for (var i = 0; i < bt.Length; i++)
            {
                byte b = bt[i];
                int n, n1, n2;
                n = (int) b;
                n1 = n & 15;
                n2 = (n >> 4) & 15;
                if (n2 > 9)
                    s += ((char) (n2 - 10 + (int) 'A')).ToString();
                else
                    s += n2.ToString();
                if (n1 > 9)
                    s += ((char) (n1 - 10 + (int) 'A')).ToString();
                else
                    s += n1.ToString();
                if ((i + 1) != bt.Length && (i + 1) % 2 == 0) s += "-";
            }

            return s;
        }

        public static string DecodeTipsFile(string inputFile, string key, string iv)
        {
            try
            {
                var outputFile = Path.GetTempFileName();
                var buffer = new byte[4];
                using (var originalFileStream = new FileStream(inputFile, FileMode.Open))
                using (var outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var readBytes = originalFileStream.Read(buffer, 0, buffer.Length);
                    var length = buffer.ToInt();
                    buffer = new byte[length];
                    readBytes = originalFileStream.Read(buffer, 0, buffer.Length);
                    var decryptedBuffer = Decode(buffer, key, iv);
                    outputFileStream.Write(decryptedBuffer, 0, decryptedBuffer.Length);

                    // Continue to read until end
                    while (readBytes > 0 && originalFileStream.CanRead)
                    {
                        readBytes = originalFileStream.Read(buffer, 0, buffer.Length);
                        if (readBytes > 0)
                        {
                            outputFileStream.Write(buffer, 0, readBytes);
                        }
                    }

                    originalFileStream.Close();
                    outputFileStream.Flush();
                    outputFileStream.Close();
                }

                return outputFile;
            }
            catch (Exception exc)
            {
                return "";
            }
        }

        #region Original Device ID Getting Code

        // Return a hardware Identifier
        private static string Identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue, bool firstOnly = true)
        {
            var result = new List<string>();
            var mc = new System.Management.ManagementClass(wmiClass);
            var moc = mc.GetInstances();
            foreach (var mo in moc)
            {
                if (mo[wmiMustBeTrue].ToString() == "True")
                {
                    try
                    {
                        result.Add(mo[wmiProperty].ToString());
                    }
                    catch
                    {
                    }
                }
            }

            result.Sort();

            return firstOnly ? result.FirstOrDefault() : string.Join(",", result);
        }

        // Return a hardware Identifier
        private static string Identifier(string wmiClass, string wmiProperty, bool firstOnly = true)
        {
            var result = new List<string>();
            var mc = new System.Management.ManagementClass(wmiClass);
            var moc = mc.GetInstances();
            foreach (var mo in moc)
            {
                try
                {
                    result.Add(mo[wmiProperty].ToString());
                }
                catch
                {
                }
            }

            result.Sort();

            return firstOnly ? result.FirstOrDefault() : string.Join(",", result);
        }

        private static string CpuId()
        {
            //Uses first CPU Identifier available in order of preference
            //Don't get all identifiers, as it is very time consuming
            var retVal = Identifier("Win32_Processor", "UniqueId");
            if (retVal == "") //If no UniqueID, use ProcessorID
            {
                retVal = Identifier("Win32_Processor", "ProcessorId");
                if (retVal == "") //If no ProcessorId, use Name
                {
                    retVal = Identifier("Win32_Processor", "Name");
                    if (retVal == "") //If no Name, use Manufacturer
                    {
                        retVal = Identifier("Win32_Processor", "Manufacturer");
                    }
                    //Add clock speed for extra security
                    retVal += Identifier("Win32_Processor", "MaxClockSpeed");
                }
            }
            return retVal;
        }

        // BIOS Identifier
        private static string BiosId()
        {
            return Identifier("Win32_BIOS", "Manufacturer")
                   + Identifier("Win32_BIOS", "SMBIOSBIOSVersion")
                   + Identifier("Win32_BIOS", "IdentificationCode")
                   + Identifier("Win32_BIOS", "SerialNumber")
                   + Identifier("Win32_BIOS", "ReleaseDate")
                   + Identifier("Win32_BIOS", "Version");
        }

        // Main physical hard drive ID
        private static string DiskId()
        {
            return Identifier("Win32_DiskDrive", "Model")
                   + Identifier("Win32_DiskDrive", "Manufacturer")
                   + Identifier("Win32_DiskDrive", "Signature")
                   + Identifier("Win32_DiskDrive", "TotalHeads");
        }

        // Motherboard ID
        private static string BaseId()
        {
            return Identifier("Win32_BaseBoard", "Model")
                   + Identifier("Win32_BaseBoard", "Manufacturer")
                   + Identifier("Win32_BaseBoard", "Name")
                   + Identifier("Win32_BaseBoard", "SerialNumber");
        }

        // Primary video controller ID
        private static string VideoId()
        {
            return Identifier("Win32_VideoController", "Name");
        }

        // First enabled network card ID
        private static string MacId()
        {
            return Identifier(
                              "Win32_NetworkAdapterConfiguration",
                              "MACAddress",
                              "IPEnabled");
        }

        #endregion
    }
}
