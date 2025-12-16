using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AutoUpload.Shared.Models;

namespace AutoUpload.Shared
{
    public class Teechip
    {
        public static readonly string BaseUrl = "https://teechip.com";
        public static readonly string LoginUrl = $"{BaseUrl}/sign_in";
        public static readonly string UploadImageUrl = $"{BaseUrl}/api/campaigns/design";
        public static readonly string ProductsUrl = $"{BaseUrl}/pick_products";
        public static readonly string GoalsUrl = $"{BaseUrl}/set_goal";
        public static readonly string CheckLinkUrl = $"{BaseUrl}/url";
        public static readonly string UploadUrl = $"{BaseUrl}/api/campaigns";
        public static readonly string GetCampaignsByUrlLink = BaseUrl + "/api/campaigns?page=1&perPage=30&search={0}&sort=-startDate&status=active";
        public static readonly string UpdateCampaignUrl = BaseUrl + "/api/campaigns/{0}";

        private static readonly Dictionary<string, double[]> PrintableAreas = new Dictionary<string, double[]>()
                                                                              {
                                                                                  // Top, Left, Width, Height, Limit width (negative for left align, positive for right align, 0 for no limit)
                                                                                  { "base", new[] { 0.25, 0, 0.4, 0.51, 0 } },
                                                                                  { "base-back", new[] { 0.22, 0, 0.42, 0.51, 0 } },
                                                                                  { "gildan-50-50-hoodie", new[] { 0.25, 0, 0.4, 0.4, 0 } },
                                                                                  { "gildan-50-50-hoodie-back", new[] { 0.25, 0, 0.4, 0.52, 0 } },
                                                                                  { "has-hoodie", new[] { 0.25, 0, 0.4, 0.4, 0 } },
                                                                                  { "gildan-zip-front-hoodie", new[] { 0.25, 0, 0.4, 0.33, -0.5 } },
                                                                                  { "gildan-zip-front-hoodie-back", new[] { 0.25, 0, 0.4, 0.52, 0 } },
                                                                                  { "canvas-mens-rib-tank", new[] { 0.25, 0, 0.38, 0.51, 0 } },
                                                                                  { "fruit-of-the-loom-hd-tank", new[] { 0.25, 0, 0.38, 0.51, 0 } },
                                                                                  { "canvas-v-neck-t-shirt", new[] { 0.25, 0, 0.38, 0.51, 0 } },
                                                                                  { "canvas-v-neck-t-shirt-back", new[] { 0.25, 0, 0.38, 0.51, 0 } },
                                                                                  { "fruit-of-the-loom-hd-v-neck-t-shirt", new[] { 0.25, 0, 0.38, 0.51, 0 } },
                                                                                  { "fruit-of-the-loom-hd-v-neck-t-shirt-back", new[] { 0.25, 0, 0.38, 0.51, 0 } },
                                                                                  { "bella-flowy-racerback-tank", new[] { 0.25, 0, 0.36, 0.51, 0 } },
                                                                                  { "bella-flowy-racerback-tank-back", new[] { 0.27, 0, 0.36, 0.51, 0 } },
                                                                                  { "bella-racerback-tank", new[] { 0.25, 0, 0.36, 0.51, 0 } },
                                                                                  { "bella-racerback-tank-back", new[] { 0.27, 0, 0.36, 0.51, 0 } },
                                                                                  { "gloss-poster-11x17", new[] { 0, 0, 1.0, 1, 0 } },
                                                                                  { "gloss-poster-16x24", new[] { 0, 0, 1.0, 1, 0 } },
                                                                                  { "gloss-poster-24x36", new[] { 0, 0, 1.0, 1, 0 } },
                                                                                  { "phone-case", new[] { 0.25, 0, 0.5, 0.5, 0 } },
                                                                                  { "beverage-mug", new[] { 0.2, 0.0, 0.42, 0.638, 0 } },
                                                                                  { "mug", new[] { 0.2, 0.0, 0.42, 0.638, 0 } }
                                                                              };

        public static CookieContainer Login(string username, string password)
        {
            var cookiesContainer = new CookieContainer();

            var result = Common.GetText(LoginUrl, cookiesContainer);
            var cookies = cookiesContainer.GetCookies(new Uri(BaseUrl));
            var loginResult = Common.PostJson<JToken>(
                                                      LoginUrl,
                                                      new Dictionary<string, object>
                                                      {
                                                          { "email", username },
                                                          { "password", password }
                                                      },
                                                      cookiesContainer,
                                                      new Dictionary<string, string>()
                                                      {
                                                          { "X-XSRF-TOKEN", cookies["XSRF-TOKEN"].Value }
                                                      });

            if (loginResult != null && loginResult["role"] != null)
            {
                return cookiesContainer;
            }

            return null;
        }

        public static JToken UploadImage(string file, Stream input, CookieContainer cookiesContainer)
        {
            var cookies = cookiesContainer.GetCookies(new Uri(BaseUrl));
            var result = Common.PostTextAsMultipartForm(
                                                        UploadImageUrl,
                                                        new List<KeyValuePair<string, object>>()
                                                        {
                                                            new KeyValuePair<string, object>("file=" + Path.GetFileName(file), input)
                                                        },
                                                        cookiesContainer,
                                                        new Dictionary<string, string>()
                                                        {
                                                            { "X-XSRF-TOKEN", cookies["XSRF-TOKEN"].Value },
                                                            { "Connection", "Keep-Alive" },
                                                            { "Keep-Alive", "timeout=600" }
                                                        });

            if (result != null)
            {
                try
                {
                    return JsonConvert.DeserializeObject<JToken>(result);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return null;
        }

        public static ProductsInfo GetProductsInfo()
        {
            var content = Common.GetText(ProductsUrl, new CookieContainer());
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            var colors = "";
            var products = "";
            var productNames = "";
            var basePrices = "";
            var productTypes = "";
            var design = "";
            var searchTerm = "<div id=\"colors\" style=\"display: none\">";
            var index = content.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase);
            var lastIndex = 0;
            if (index >= 0)
            {
                lastIndex = content.IndexOf("</div>", index, StringComparison.OrdinalIgnoreCase);
                if (lastIndex >= 0)
                {
                    colors = content.Substring(index + searchTerm.Length, lastIndex - index - searchTerm.Length);
                }
            }

            searchTerm = "<div id=\"menuNames\" style=\"display: none\">";
            index = content.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                lastIndex = content.IndexOf("</div>", index, StringComparison.OrdinalIgnoreCase);
                if (lastIndex >= 0)
                {
                    productNames = content.Substring(index + searchTerm.Length, lastIndex - index - searchTerm.Length);
                }
            }

            searchTerm = "<div id=\"basePrices\" style=\"display: none\">";
            index = content.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                lastIndex = content.IndexOf("</div>", index, StringComparison.OrdinalIgnoreCase);
                if (lastIndex >= 0)
                {
                    basePrices = content.Substring(index + searchTerm.Length, lastIndex - index - searchTerm.Length);
                }
            }

            searchTerm = "<div id=\"productTypes\" style=\"display: none\">";
            index = content.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                lastIndex = content.IndexOf("</div>", index, StringComparison.OrdinalIgnoreCase);
                if (lastIndex >= 0)
                {
                    productTypes = content.Substring(index + searchTerm.Length, lastIndex - index - searchTerm.Length);
                }
            }

            searchTerm = "<div id=\"Design\" style=\"display: none\">";
            index = content.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                lastIndex = content.IndexOf("</div>", index, StringComparison.OrdinalIgnoreCase);
                if (lastIndex >= 0)
                {
                    design = content.Substring(index + searchTerm.Length, lastIndex - index - searchTerm.Length);
                }
            }

            searchTerm = "<div id=\"products\" style=\"display: none\">";
            index = content.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                lastIndex = content.IndexOf("</div>", index, StringComparison.OrdinalIgnoreCase);
                if (lastIndex >= 0)
                {
                    products = content.Substring(index + searchTerm.Length, lastIndex - index - searchTerm.Length);
                }
            }

            try
            {
                return new ProductsInfo
                       {
                           Colors = JsonConvert.DeserializeObject<JToken>(colors),
                           Products = JsonConvert.DeserializeObject<JToken>(products),
                           ProductNames = JsonConvert.DeserializeObject<JToken>(productNames),
                           BasePrices = JsonConvert.DeserializeObject<JToken>(basePrices),
                           ProductTypes = JsonConvert.DeserializeObject<JToken>(productTypes),
                           Design = JsonConvert.DeserializeObject<JToken>(design),
                           Price = GetPriceInfo()
                       };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static JToken GetPriceInfo()
        {
            var content = Common.GetText(GoalsUrl, new CookieContainer());
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            var priceMinimum = "";
            var searchTerm = "<div id=\"priceMinimums\" style=\"display: none\">";
            var index = content.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase);
            var lastIndex = 0;
            if (index >= 0)
            {
                lastIndex = content.IndexOf("</div>", index, StringComparison.OrdinalIgnoreCase);
                if (lastIndex >= 0)
                {
                    priceMinimum = content.Substring(index + searchTerm.Length, lastIndex - index - searchTerm.Length);
                }
            }

            try
            {
                return JsonConvert.DeserializeObject<JToken>(priceMinimum);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetImageLink(string image)
        {
            return $"{BaseUrl}/images/productsNew/{image}";
        }

        public static double[] GetPrintableArea(string id, bool isFront, string productType)
        {
            var name = id;
            if (productType.Equals("garment", StringComparison.OrdinalIgnoreCase) && !isFront)
            {
                name += "-back";
            }
            else if (productType.Equals("case", StringComparison.OrdinalIgnoreCase))
            {
                name = "phone-case";
            }

            if (PrintableAreas.ContainsKey(name))
            {
                return PrintableAreas[name];
            }

            return isFront ? PrintableAreas["base"] : PrintableAreas["base-back"];
        }

        public static bool CheckUrl(string url, CookieContainer cookiesContainer)
        {
            var cookies = cookiesContainer.GetCookies(new Uri(BaseUrl));
            var content = Common.PostText(
                                          CheckLinkUrl,
                                          new Dictionary<string, object> { { "check", url } },
                                          cookiesContainer,
                                          new Dictionary<string, string>()
                                          {
                                              { "X-XSRF-TOKEN", cookies["XSRF-TOKEN"].Value }
                                          });
            if (content == null)
            {
                return false;
            }

            if (content.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public static string Upload(TeechipItem item, CookieContainer cookiesContainer)
        {
            var cookies = cookiesContainer.GetCookies(new Uri(BaseUrl));
            return Common.PostText(
                                   UploadUrl,
                                   item,
                                   cookiesContainer,
                                   new Dictionary<string, string>()
                                   {
                                       { "X-XSRF-TOKEN", cookies["XSRF-TOKEN"].Value }
                                   });
        }

        public static JToken GetCampaignByUrl(string url, CookieContainer cookiesContainer)
        {
            var cookies = cookiesContainer.GetCookies(new Uri(BaseUrl));
            var result = Common.GetJson<JToken>(
                string.Format(GetCampaignsByUrlLink, url), 
                cookiesContainer,
                new Dictionary<string, string>()
                {
                    { "X-XSRF-TOKEN", cookies["XSRF-TOKEN"].Value }
                });
            if (result != null && result["campaigns"] != null && result["campaigns"].ToObject<JArray>() != null && result["campaigns"].ToObject<JArray>().Count > 0)
            {
                return result["campaigns"].ToObject<JArray>()[0];
            }
            return null;
        }

        public static bool UpdateCampaign(string url, string facebookPixel, string googleAnalytics, CookieContainer cookiesContainer)
        {
            var campaign = GetCampaignByUrl(url, cookiesContainer);
            if (campaign == null)
            {
                return false;
            }

            var campaignId = campaign["parent"]["_id"].ToObject<string>();
            var result = Common.PatchJson<JToken>(
                string.Format(UpdateCampaignUrl, campaignId),
                new Dictionary<string, object>()
                {
                    { "analytics", new Dictionary<string, string>()
                        {
                            { "facebookPixel", facebookPixel },
                            { "google", googleAnalytics }
                        }
                    },
                    { "autoRestart", campaign["parent"]["autoRestart"].ToObject<bool>() },
                    { "description", campaign["parent"]["description"].ToObject<string>() },
                    { "goal", campaign["parent"]["goal"].ToObject<int>() },
                    { "title", campaign["parent"]["title"].ToObject<string>() }
                },
                cookiesContainer);

            return result != null;
        }
    }
}
