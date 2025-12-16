using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpload.Shared
{
    public class MockupTemplate
    {
        public string Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string Category
        {
            get; set;
        }

        public string FilePath
        {
            get; set;
        }

        public string ThumbnailPath
        {
            get; set;
        }

        public DateTime UpdatedOn
        {
            get; set;
        }

        public string TempFilePath
        {
            get; set;
        }
    }

    public class Mockup
    {
        public static readonly string BaseUrl = "http://autoupload.teeinsight.com/api/users";
        public static readonly string GetMockupTemplatesUrl = $"{BaseUrl}/mockuptemplates";
        public static readonly string DownloadMockupUrl = BaseUrl + "/downloadmockup/{0}";
        public static readonly string DownloadThumbnailUrl = BaseUrl + "/downloadmockupthumbnail/{0}";

        public static List<MockupTemplate> UpdateMockupTemplates(string email, string license, string machineId, string checksum)
        {
            var result = Common.PostJson<JToken>(
                                                 GetMockupTemplatesUrl,
                                                 new Dictionary<string, string>
                                                 {
                                                     { "Email", email },
                                                     { "LicenseKey", license },
                                                     { "MachineId", machineId },
                                                     { "Checksum", checksum }
                                                 },
                                                 new CookieContainer());

            if (result == null)
            {
                throw new Exception("Cannot access to Internet. Please check your network connection.");
            }

            if (result["error"].ToObject<int>() != 0)
            {
                throw new Exception(result["message"].ToObject<string>());
            }

            var list = new List<MockupTemplate>();
            foreach (var item in result["list"].ToArray())
            {
                list.Add(new MockupTemplate()
                {
                    Id = item["id"].ToObject<string>(),
                    Name = item["name"] == null ? "" : item["name"].ToObject<string>(),
                    Category = item["category"] == null ? "" : item["category"].ToObject<string>(),
                    UpdatedOn = new DateTime(item["updatedOn"].ToObject<long>())
                });
            }
            return list;
        }

        public static List<MockupTemplate> LoadMockupTemplates(Stream stream)
        {
            try
            {
                var buffer = new byte[stream.Length];
                var length = stream.Read(buffer, 0, (int)stream.Length);

                var list = JsonConvert.DeserializeObject<JArray>(Encoding.UTF8.GetString(buffer, 0, length));
                var result = new List<MockupTemplate>();
                foreach (var item in list)
                {
                    result.Add(new MockupTemplate()
                    {
                        Id = item["Id"].ToObject<string>(),
                        Name = item["Name"] == null ? "" : item["Name"].ToObject<string>(),
                        Category = item["Category"] == null ? "" : item["Category"].ToObject<string>(),
                        UpdatedOn = new DateTime(item["UpdatedOn"].ToObject<long>()),
                        FilePath = item["FilePath"] == null ? "" : item["FilePath"].ToObject<string>(),
                        ThumbnailPath = item["ThumbnailPath"] == null ? "" : item["ThumbnailPath"].ToObject<string>()
                    });
                }
                
                return result;
            }
            catch (Exception exc)
            {
                Logger.WriteLog(exc);
                return new List<MockupTemplate>();
            }
        }

        public static bool SaveMockupTemplates(Stream stream, List<MockupTemplate> list)
        {
            try
            {
                var array = new List<Dictionary<string, object>>();
                foreach (var item in list)
                {
                    array.Add(new Dictionary<string, object>()
                    {
                        { "Id", item.Id },
                        { "Name", item.Name },
                        { "Category", item.Category },
                        { "UpdatedOn", item.UpdatedOn.Ticks },
                        { "FilePath", item.FilePath },
                        { "ThumbnailPath", item.ThumbnailPath }
                    });
                }

                var content = JsonConvert.SerializeObject(array);
                var buffer = Encoding.UTF8.GetBytes(content);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();

                return true;
            }
            catch (Exception exc)
            {
                Logger.WriteLog(exc);
                return false;
            }
        }
    }
}
