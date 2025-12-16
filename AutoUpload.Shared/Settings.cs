using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpload.Shared
{
    public class Settings
    {
        [JsonProperty(PropertyName = "SunfrogDelayTime")]
        public int SunfrogDelayTime { get; set; }

        [JsonProperty(PropertyName = "TeespringDelayTime")]
        public int TeespringDelayTime { get; set; }

        [JsonProperty(PropertyName = "TeechipDelayTime")]
        public int TeechipDelayTime { get; set; }

        [JsonProperty(PropertyName = "ViralstyleDelayTime")]
        public int ViralstyleDelayTime { get; set; }

        [JsonProperty(PropertyName = "TeezilyDelayTime")]
        public int TeezilyDelayTime { get; set; }

        [JsonProperty(PropertyName = "SunfrogDesignsPerAccount")]
        public int SunfrogDesignsPerAccount { get; set; }

        [JsonProperty(PropertyName = "TeespringDesignsPerAccount")]
        public int TeespringDesignsPerAccount { get; set; }

        [JsonProperty(PropertyName = "TeechipDesignsPerAccount")]
        public int TeechipDesignsPerAccount { get; set; }

        [JsonProperty(PropertyName = "ViralstyleDesignsPerAccount")]
        public int ViralstyleDesignsPerAccount { get; set; }

        [JsonProperty(PropertyName = "TeezilyDesignsPerAccount")]
        public int TeezilyDesignsPerAccount { get; set; }

        [JsonProperty(PropertyName = "MockupDefaultLocation")]
        public string MockupDefaultLocation { get; set; }

        [JsonProperty(PropertyName = "FacebookAccessToken")]
        public string FacebookAccessToken { get; set; }

        [JsonProperty(PropertyName = "FacebookPostContent")]
        public string FacebookPostContent { get; set; }

        public Settings()
        {
            // Set recommended values
            SunfrogDelayTime = 15;
            TeespringDelayTime = 15;
            TeechipDelayTime = 15;
            ViralstyleDelayTime = 15;
            TeezilyDelayTime = 15;
            SunfrogDesignsPerAccount = 0;
            TeespringDesignsPerAccount = 0;
            TeechipDesignsPerAccount = 0;
            ViralstyleDesignsPerAccount = 0;
            TeezilyDesignsPerAccount = 0;
            MockupDefaultLocation = "";
            FacebookAccessToken = "";
            FacebookPostContent = "";
        }

        public bool Load(Stream stream)
        {
            try
            {
                var buffer = new byte[stream.Length];
                var length = stream.Read(buffer, 0, (int)stream.Length);

                var settings = JsonConvert.DeserializeObject<JToken>(Encoding.UTF8.GetString(buffer, 0, length));

                SunfrogDelayTime = settings["SunfrogDelayTime"]?.ToObject<int>() ?? 15;
                TeespringDelayTime = settings["TeespringDelayTime"]?.ToObject<int>() ?? 15;
                TeechipDelayTime = settings["TeechipDelayTime"]?.ToObject<int>() ?? 15;
                ViralstyleDelayTime = settings["ViralstyleDelayTime"]?.ToObject<int>() ?? 15;
                TeezilyDelayTime = settings["TeezilyDelayTime"]?.ToObject<int>() ?? 15;

                SunfrogDesignsPerAccount = settings["SunfrogDesignsPerAccount"]?.ToObject<int>() ?? 0;
                TeespringDesignsPerAccount = settings["TeespringDesignsPerAccount"]?.ToObject<int>() ?? 0;
                TeechipDesignsPerAccount = settings["TeechipDesignsPerAccount"]?.ToObject<int>() ?? 0;
                ViralstyleDesignsPerAccount = settings["ViralstyleDesignsPerAccount"]?.ToObject<int>() ?? 0;
                TeezilyDesignsPerAccount = settings["TeezilyDesignsPerAccount"]?.ToObject<int>() ?? 0;

                MockupDefaultLocation = settings["MockupDefaultLocation"]?.ToObject<string>() ?? "";

                FacebookAccessToken = settings["FacebookAccessToken"]?.ToObject<string>() ?? "";
                FacebookPostContent = settings["FacebookPostContent"]?.ToObject<string>() ?? "";

                return true;
            }
            catch (Exception exc)
            {
                Logger.WriteLog(exc);
                return false;
            }
        }

        public bool Save(Stream stream)
        {
            try
            {
                var settings = new Dictionary<string, object>
                               {
                                   {"SunfrogDelayTime", SunfrogDelayTime},
                                   {"TeespringDelayTime", TeespringDelayTime},
                                   {"TeechipDelayTime", TeechipDelayTime},
                                   {"ViralstyleDelayTime", ViralstyleDelayTime},
                                   {"TeezilyDelayTime", TeezilyDelayTime},
                                   {"SunfrogDesignsPerAccount", SunfrogDesignsPerAccount},
                                   {"TeespringDesignsPerAccount", TeespringDesignsPerAccount},
                                   {"TeechipDesignsPerAccount", TeechipDesignsPerAccount},
                                   {"ViralstyleDesignsPerAccount", ViralstyleDesignsPerAccount},
                                   {"TeezilyDesignsPerAccount", TeezilyDesignsPerAccount},
                                   {"MockupDefaultLocation", MockupDefaultLocation},
                                   {"FacebookAccessToken", FacebookAccessToken},
                                   {"FacebookPostContent", FacebookPostContent}
                               };
                
                var content = JsonConvert.SerializeObject(settings);
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
