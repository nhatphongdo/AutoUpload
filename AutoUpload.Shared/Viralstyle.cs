using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AutoUpload.Shared.Models;

namespace AutoUpload.Shared
{
    public class Viralstyle
    {
        public static readonly string BaseUrl = "https://viralstyle.com";
        public static readonly string GetTokenUrl = $"{BaseUrl}/api/v2/token";
        public static readonly string LoginUrl = $"{BaseUrl}/api/v2/auth/login";
        public static readonly string DesignUrl = $"{BaseUrl}/design.beta";
        public static readonly string ProductsInfoUrl = $"{DesignUrl}/product-categories?api_campaign=false";
        public static readonly string GetPriceUrl = $"{DesignUrl}/pricing?goal=10";
        public static readonly string GetCategoriesUrl = $"{DesignUrl}/categories";
        public static readonly string UploadAssetUrl = $"{DesignUrl}/upload-asset";
        public static readonly string CheckLinkUrl = $"{DesignUrl}/check-url";
        public static readonly string UploadUrl = $"{BaseUrl}/api/v2/designer/store";
        public static readonly string UpdateCampaignUrl = BaseUrl + "/client/campaigns/{0}";
        public static readonly string UpdateTrackingUrl = BaseUrl + "/client/campaigns/{0}/tracking";

        public static JToken GetToken(CookieContainer cookiesContainer)
        {
            var result = Common.PostTextAsForm(
                                               GetTokenUrl,
                                               new List<KeyValuePair<string, string>>
                                               {
                                                   new KeyValuePair<string, string>("grant_type", "client_credentials"),
                                                   new KeyValuePair<string, string>("client_id", "frontend"),
                                                   new KeyValuePair<string, string>("client_secret", "frontend"),
                                                   new KeyValuePair<string, string>("scope", "public")
                                               },
                                               cookiesContainer);

            if (result == null)
            {
                return null;
            }

            var json = JsonConvert.DeserializeObject<JToken>(result);
            if (json != null && json["access_token"] != null)
            {
                return json;
            }

            return null;
        }

        public static string GetPageToken(CookieContainer cookiesContainer)
        {
            var content = Common.GetText(DesignUrl, cookiesContainer);
            if (content == null)
            {
                return "";
            }

            var index = content.IndexOf("id=\"_token\"", StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                index = content.IndexOf("value=", index + 1, StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                {
                    return content.Substring(index + "value=".Length, content.IndexOf(">", index + 1, StringComparison.OrdinalIgnoreCase) - index - "value=".Length).Trim('"');
                }
            }

            return "";
        }

        public static JToken Login(string username, string password, CookieContainer cookiesContainer)
        {
            var token = GetToken(cookiesContainer);
            if (token == null)
            {
                return null;
            }

            var result = Common.PostJson<JToken>(
                                                 LoginUrl,
                                                 new Dictionary<string, object>
                                                 {
                                                     { "email_address", username },
                                                     { "password", password },
                                                     { "remember", true }
                                                 },
                                                 cookiesContainer,
                                                 new Dictionary<string, string>
                                                 {
                                                     { "authorization", token["token_type"].ToObject<string>() + " " + token["access_token"].ToObject<string>() }
                                                 });

            if (result == null || result["token"] == null || result["user"] == null)
            {
                return null;
            }

            return result;
        }

        public static JToken GetProductsInfo(CookieContainer cookiesContainer)
        {
            return Common.GetJson<JToken>(ProductsInfoUrl, cookiesContainer);
        }

        public static JToken GetPricing(CookieContainer cookiesContainer)
        {
            return Common.GetJson<JToken>(GetPriceUrl, cookiesContainer);
        }

        public static JToken GetCategories(CookieContainer cookiesContainer)
        {
            return Common.GetJson<JToken>(GetCategoriesUrl, cookiesContainer);
        }

        public static bool CheckUrl(string url, CookieContainer cookiesContainer)
        {
            var result = Common.PostJson<JToken>(
                                                 CheckLinkUrl,
                                                 new Dictionary<string, object>
                                                 {
                                                     { "url", url }
                                                 },
                                                 cookiesContainer);

            if (result == null || result["status"].ToObject<string>().ToLower() != "success")
            {
                return false;
            }

            return true;
        }

        public static JToken UploadAsset(string fileName, Stream input, int width, int productId, string identifier, string campaignId, CookieContainer cookiesContainer)
        {
            var result = Common.PostTextAsMultipartForm(
                                                        UploadAssetUrl,
                                                        new List<KeyValuePair<string, object>>()
                                                        {
                                                            new KeyValuePair<string, object>("image_file=" + Path.GetFileName(fileName), input),
                                                            new KeyValuePair<string, object>("extension", Path.GetExtension(fileName).Trim('.')),
                                                            new KeyValuePair<string, object>("width", width),
                                                            new KeyValuePair<string, object>("identifier", identifier),
                                                            new KeyValuePair<string, object>("campaign_identifier", campaignId),
                                                            new KeyValuePair<string, object>("sublimation", 0),
                                                            new KeyValuePair<string, object>("is_phone_case", 0),
                                                            new KeyValuePair<string, object>("is_embroidery", 0),
                                                            new KeyValuePair<string, object>("product_id", productId)
                                                        },
                                                        cookiesContainer);

            if (result == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<JToken>(result);
        }

        public static JToken Upload(ViralstyleItem item, string token, string authorizationToken, CookieContainer cookiesContainer)
        {
            var result = Common.PostJson<JToken>(
                                                 UploadUrl,
                                                 new Dictionary<string, object>
                                                 {
                                                     { "_token", token },
                                                     { "campaign_data", JsonConvert.SerializeObject(item) },
                                                 },
                                                 cookiesContainer,
                                                 new Dictionary<string, string>()
                                                 {
                                                     { "authorization", authorizationToken },
                                                     { "X-CSRF-TOKEN", token },
                                                     { "Referer", DesignUrl }
                                                 });

            return result;
        }

        public static string GetCampaignPageToken(string campaignId, CookieContainer cookiesContainer)
        {
            var content = Common.GetText(string.Format(UpdateCampaignUrl, campaignId), cookiesContainer);
            if (content == null)
            {
                return "";
            }

            var index = content.IndexOf("id=\"_token\"", StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                index = content.IndexOf("value=", index + 1, StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                {
                    return content.Substring(index + "value=".Length, content.IndexOf(">", index + 1, StringComparison.OrdinalIgnoreCase) - index - "value=".Length).Trim('"');
                }
            }

            return "";
        }

        public static bool UpdateTracking(string campaignId, string facebookPixel, string token, CookieContainer cookiesContainer)
        {
            var ret = Common.GetText(string.Format("https://viralstyle.com/client/campaigns/{0}/tracking-with-defaults", campaignId), cookiesContainer);
            var result = Common.PostJson<JToken>(
                                                 string.Format(UpdateTrackingUrl, campaignId),
                                                 new Dictionary<string, object>
                                                 {
                                                     { "_token", token },
                                                     { "custom_variables", new string[] { } },
                                                     { "pixel_data", new Dictionary<string, object>[] 
                                                         {
                                                             new Dictionary<string, object>()
                                                             {
                                                                 { "type", "new-fb" },
                                                                 { "code", facebookPixel },
                                                                 { "new_facebook_retarget_enabled", 1 },
                                                                 { "new_facebook_salespage_enabled", 1 },
                                                                 { "new_facebook_tracking_enabled", 1 }
                                                             },
                                                             new Dictionary<string, object>()
                                                             {
                                                                 { "type", "tracking" },
                                                                 { "code", null },
                                                                 { "provider", "facebook" },
                                                                 { "enabled", false }
                                                             },
                                                             new Dictionary<string, object>()
                                                             {
                                                                 { "type", "retarget" },
                                                                 { "code", null },
                                                                 { "provider", "facebook" },
                                                                 { "enabled", false }
                                                             },
                                                             new Dictionary<string, object>()
                                                             {
                                                                 { "type", "tracking" },
                                                                 { "code", null },
                                                                 { "provider", "google" },
                                                                 { "enabled", false }
                                                             },
                                                             new Dictionary<string, object>()
                                                             {
                                                                 { "type", "retarget" },
                                                                 { "code", null },
                                                                 { "provider", "google" },
                                                                 { "enabled", false }
                                                             },
                                                             new Dictionary<string, object>()
                                                             {
                                                                 { "type", "retarget" },
                                                                 { "code", null },
                                                                 { "provider", "twitter" },
                                                                 { "enabled", false }
                                                             },
                                                             new Dictionary<string, object>()
                                                             {
                                                                 { "type", "retarget" },
                                                                 { "code", "{\"pixel_id\":\"\",\"adv_id\":\"\"}" },
                                                                 { "provider", "adroll" },
                                                                 { "enabled", false }
                                                             },
                                                             new Dictionary<string, object>()
                                                             {
                                                                 { "type", "retarget" },
                                                                 { "code", null },
                                                                 { "provider", "perfect" },
                                                                 { "enabled", false }
                                                             },
                                                             new Dictionary<string, object>()
                                                             {
                                                                 { "type", "tracking" },
                                                                 { "code", null },
                                                                 { "provider", "custom" },
                                                                 { "enabled", false }
                                                             },
                                                             new Dictionary<string, object>()
                                                             {
                                                                 { "type", "default" },
                                                                 { "code", null },
                                                                 { "provider", "pinterest" },
                                                                 { "enabled", false }
                                                             },
                                                         }
                                                     },
                                                 },
                                                 cookiesContainer,
                                                 new Dictionary<string, string>()
                                                 {
                                                     { "_token", token },
                                                     { "Referer", string.Format(UpdateCampaignUrl, campaignId) },
                                                 });

            return result != null && result["status"]?.ToObject<string>()?.ToLower() == "success";
        }
    }
}
