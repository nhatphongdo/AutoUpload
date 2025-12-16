using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AutoUpload.Shared.Models;

namespace AutoUpload.Shared
{
    public class Teespring
    {
        private static readonly string AvailableCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";

        public static readonly string BaseUrl = "https://teespring.com";
        public static readonly string TokenUrl = $"{BaseUrl}/tokens.json";
        public static readonly string LoginUrl = $"{BaseUrl}/login";
        public static readonly string LoginSubmitUrl = $"{BaseUrl}/sessions";
        public static readonly string DesignLauncherUrl = $"{BaseUrl}/design-launcher";
        public static readonly string DesignUrl = $"{BaseUrl}/designs";
        public static readonly string CategoriesUrl = $"{BaseUrl}/categories.json";
        public static readonly string CampaignTemplateUrl = $"{BaseUrl}/campaign_templates.json";
        public static readonly string ProductsUrl = BaseUrl + "/products.json?partnership=&region={0}";
        public static readonly string SignInToS3Url = DesignUrl + "/sign_s3?s3_object_type={0}&s3_object_name={1}";
        public static readonly string BootstrapUrl = DesignUrl + "/{0}/bootstrap.json";
        public static readonly string ProductPriceUrl = $"{BaseUrl}/product_pricing";
        public static readonly string CheckLinkUrl = BaseUrl + "/url/availability?url={0}";
        public static readonly string UploadUrl = DesignUrl + "/{0}";
        public static readonly string CheckAssetReadyUrl = DesignUrl + "/{0}/assets_ready";
        public static readonly string LaunchUrl = BaseUrl + "/campaigns/{0}/launch";
        public static readonly string InksUrl = $"{BaseUrl}/inks";
        public static readonly string GetCheckUploadUrl = DesignUrl + "/{0}/uploads";
        public static readonly string MakePrivateUrl = BaseUrl + "/dashboard/campaigns/{0}/hide_from_search";
        public static readonly string RelaunchUrl = BaseUrl + "/dashboard/campaigns/{0}/relaunch_type";
        public static readonly string UpdateTrackingUrl = BaseUrl + "/dashboard/campaigns/{0}/campaign_tracking_snippets";
        public static readonly string NewProductsUrl = $"{BaseUrl}/composer/v1/products.json";
        public static readonly string NewBootstrapUrl = $"{BaseUrl}/composer/v1/bootstrap.json";
        public static readonly string UploadNewProductMockupsUrl = $"{BaseUrl}/composer/v1/mockups.json";
        public static readonly string LaunchNewProductUrl = $"{BaseUrl}/composer/v1/campaigns.json";
        public static readonly string ValidateAddNewProductUrl = BaseUrl + "/composer/v1/validate_if_product_can_be_added_to_campaign?url={0}&product_id={1}";
        public static readonly string AddNewProductToCampaignUrl = $"{BaseUrl}/composer/v1/add_product_to_campaign.json";

        public static string GetDesignId(CookieContainer cookiesContainer)
        {
            var response = Common.GetResponse(DesignUrl, cookiesContainer);
            var urlPath = response.RequestMessage.RequestUri.PathAndQuery;
            var pathParts = urlPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var index = Array.FindIndex(pathParts, x => x.ToLower() == "designs");
            if (index >= 0 && index < pathParts.Length - 1)
            {
                return pathParts[index + 1];
            }

            return "";
        }

        public static JToken GetProducts(CookieContainer cookiesContainer, string region)
        {
            Common.GetJson<JToken>(CampaignTemplateUrl, cookiesContainer);
            var products = Common.GetJson<JToken>(string.Format(ProductsUrl, region), cookiesContainer);

            //var content = Common.GetText(DesignLauncherUrl, cookiesContainer);
            //if (string.IsNullOrEmpty(content))
            //{
            //    return products;
            //}
            
            //var regex = new Regex("<script id=\"products_meh\" type=\"text/javascript\">productInjection = (?<products>.*)}</script>");
            //var groups = regex.Match(content).Groups;
            //var newProducts = groups["products"].Captures.Count > 0 ? JToken.Parse(groups["products"].Value + "}") : JToken.Parse("{\"products\":[]}");

            //var newProducts = Common.GetJson<JToken>(NewProductsUrl, cookiesContainer);
            //if (newProducts != null)
            //{
            //    foreach (var newProduct in newProducts["products"].ToObject<JArray>())
            //    {
            //        if (newProduct["region_names"].ToObject<JArray>() != null)
            //        {
            //            var found = false;
            //            foreach (var reg in newProduct["region_names"].ToObject<JArray>())
            //            {
            //                if (reg.ToObject<string>().ToLower() == region.ToLower())
            //                {
            //                    found = true;
            //                }
            //            }
            //            if (!found)
            //            {
            //                continue;
            //            }
            //        }
            //        newProduct["IsNewProduct"] = true;
            //        products["products"].Last.AddAfterSelf(newProduct);
            //    }
            //}
            return products;
        }

        public static JToken GetInks(CookieContainer cookiesContainer)
        {
            return Common.GetJson<JToken>(InksUrl, cookiesContainer);
        }

        public static JToken GetCategories(CookieContainer cookiesContainer)
        {
            return Common.GetJson<JToken>(CategoriesUrl, cookiesContainer);
        }

        public static JToken GetBootstrap(CookieContainer cookiesContainer, string designId = "nothing")
        {
            return Common.GetJson<JToken>(string.Format(BootstrapUrl, designId), cookiesContainer);
        }

        public static JToken GetToken(CookieContainer cookiesContainer)
        {
            return Common.GetJson<JToken>(TokenUrl, cookiesContainer);
        }

        public static CookieContainer Login(string username, string password, CookieContainer cookiesContainer)
        {
            var token = GetToken(cookiesContainer);
            if (token == null)
            {
                return null;
            }

            var result = Common.PostText(
                                         LoginSubmitUrl,
                                         new Dictionary<string, string>
                                         {
                                             { "email", username },
                                             { "password", password },
                                             { "remember_user", "on" }
                                         },
                                         cookiesContainer,
                                         new Dictionary<string, string>()
                                         {
                                             { "x-csrf-token", token["token"].ToObject<string>() }
                                         });

            if (result == null)
            {
                return null;
            }

            var cookies = cookiesContainer.GetCookies(new Uri(BaseUrl));
            if (cookies["__cfduid"] != null && cookies["teespring_user_email"] != null 
                && (cookies["_teespring_session_2"] != null || cookies["_teespring_session_5"] != null)
                && cookies["teespring_dashboard_owner_profile"] != null)
            {
                var json = JsonConvert.DeserializeObject<JToken>(WebUtility.UrlDecode(cookies["teespring_dashboard_owner_profile"].Value));
                return cookiesContainer;
            }

            return null;
        }

        public static JToken SignInToS3(string contentType, string name, CookieContainer cookiesContainer)
        {
            return Common.GetJson<JToken>(string.Format(SignInToS3Url, contentType, name), cookiesContainer);
        }

        public static string GenerateRandomName(int length)
        {
            var random = new Random((int) DateTime.Now.Ticks);
            var result = "";
            for (var i = 0; i < length; i++)
            {
                var index = random.Next(AvailableCharacters.Length);
                result += AvailableCharacters[index];
            }

            return result;
        }

        public static string UploadPhoto(string designId, string imageName, string link, string file, Stream input, string amazonS3Host, CookieContainer cookiesContainer)
        {
            var result = Common.PutTextAsFile(
                                              link,
                                              file,
                                              input,
                                              cookiesContainer,
                                              new Dictionary<string, string>()
                                              {
                                                  { "Content-Type", "image/" + Path.GetExtension(file).TrimStart('.') },
                                                  { "Host", amazonS3Host }
                                              });

            if (result != null && !string.IsNullOrEmpty(designId))
            {
                var uploadJson = Common.PostText(
                                                 string.Format(GetCheckUploadUrl, designId),
                                                 new Dictionary<string, object>()
                                                 {
                                                     { "filepath", imageName },
                                                     { "filetype", Path.GetExtension(imageName).TrimStart('.') }
                                                 },
                                                 cookiesContainer);
            }
            return result;
        }

        public static string GetImageLink(string cdn, string file)
        {
            return cdn.ToLower().StartsWith("http:") || cdn.ToLower().StartsWith("https:")
                       ? (cdn + "/images/products/apparel/" + file)
                       : ("https:" + cdn + "/images/products/apparel/" + file);
        }

        public static JToken GetCost(CookieContainer cookiesContainer, int goal, string products, string frontColors = null, string backColors = null, string region = "USA", string currency = "USD")
        {
            var link = ProductPriceUrl + "?tipping_point=" + goal + "&noun_project_uses=0&currency=" + currency + "&region=" + region;
            link += "&product_options=" + products;
            if (!string.IsNullOrEmpty(frontColors))
            {
                link += "&colors_front[]=" + frontColors;
            }
            if (!string.IsNullOrEmpty(backColors))
            {
                link += "&colors_back[]=" + backColors;
            }

            return Common.GetJson<JToken>(link, cookiesContainer);
        }

        public static bool CheckUrl(string url, CookieContainer cookies)
        {
            var content = Common.GetText(string.Format(CheckLinkUrl, url), cookies);
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

        public static string Upload(string designId, TeespringItem product, string token, CookieContainer cookies)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            foreach (var property in product.GetType().GetRuntimeProperties())
            {
                var name = property.Name;
                var jsonProperty = property.GetCustomAttribute<JsonPropertyAttribute>();
                if (jsonProperty != null)
                {
                    name = jsonProperty.PropertyName;
                }
                var value = property.GetValue(product)?.ToString();
                parameters.Add(new KeyValuePair<string, string>(name, value));
            }

            var result = Common.PutTextAsForm(
                                              string.Format(UploadUrl, designId),
                                              parameters,
                                              cookies,
                                              new Dictionary<string, string>
                                              {
                                                  { "X-CSRF-Token", token }
                                              });
            return result;
        }

        public static bool CheckAsset(string designId, CookieContainer cookies, int failedTimes = 30)
        {
            const int DelayTime = 10000;
            var result = false;
            var count = 0;
            do
            {
                var content = Common.GetText(string.Format(CheckAssetReadyUrl, designId), cookies);
                if (content == null)
                {
                    result = false;
                    Thread.Sleep(TimeSpan.FromMilliseconds(DelayTime));
                    ++count;
                }
                else
                {
                    try
                    {
                        var json = JsonConvert.DeserializeObject<JToken>(content);
                        result = json["ready"].ToObject<bool>();
                        if (result == false)
                        {
                            Thread.Sleep(TimeSpan.FromMilliseconds(DelayTime));
                            ++count;
                        }
                    }
                    catch (Exception exc)
                    {
                        result = false;
                        Thread.Sleep(TimeSpan.FromMilliseconds(DelayTime));
                        ++count;
                    }
                }
            } while (result == false && count < failedTimes);
            
            return result;
        }

        public static string Launch(string campaignId, string designId, string token, CookieContainer cookiesContainer)
        {
            return Common.PostText(
                                   string.Format(LaunchUrl, campaignId),
                                   new Dictionary<string, string>()
                                   {
                                       { "partnership", "" }
                                   },
                                   cookiesContainer,
                                   new Dictionary<string, string>()
                                   {
                                       { "X-CSRF-Token", token }
                                   },
                                   false);
        }

        public static string MakePrivate(string campaignId, string token, CookieContainer cookiesContainer)
        {
            return Common.PostText(
                                   string.Format(MakePrivateUrl, campaignId),
                                   new Dictionary<string, string>()
                                   {
                                       { "utf8", "✓" },
                                       { "hide_from_search", "on" }
                                   },
                                   cookiesContainer,
                                   new Dictionary<string, string>()
                                   {
                                       { "X-CSRF-Token", token }
                                   },
                                   false);
        }

        public static string AutoRelaunch(string campaignId, bool continuous, int days, string token, CookieContainer cookiesContainer)
        {
            return Common.PostText(
                                   string.Format(RelaunchUrl, campaignId),
                                   new Dictionary<string, string>()
                                   {
                                       { "relaunch_type", continuous ? "continuous" : "one_time" },
                                       { "continuous_auto_relaunch_duration", days.ToString() }
                                   },
                                   cookiesContainer,
                                   new Dictionary<string, string>()
                                   {
                                       { "X-CSRF-Token", token }
                                   },
                                   false);
        }

        public static void UpdateTracking(string campaignId, string token, string facebookPixel, string googleAnalytics, CookieContainer cookiesContainer)
        {
            if (!string.IsNullOrEmpty(facebookPixel))
            {
                var result = Common.PostText(
                                       string.Format(UpdateTrackingUrl, campaignId),
                                       new Dictionary<string, object>()
                                       {
                                           { "campaign_tracking_snippet", new Dictionary<string, string>()
                                               {
                                                   { "snippet", facebookPixel },
                                                   { "snippet_type", "10" }
                                               }
                                           }
                                       },
                                       cookiesContainer,
                                       new Dictionary<string, string>()
                                       {
                                           { "X-CSRF-Token", token }
                                       },
                                       true);
            }
            if (!string.IsNullOrEmpty(googleAnalytics))
            {
                var result = Common.PostText(
                                       string.Format(UpdateTrackingUrl, campaignId),
                                       new Dictionary<string, object>()
                                       {
                                           { "campaign_tracking_snippet", new Dictionary<string, string>()
                                               {
                                                   { "snippet", googleAnalytics },
                                                   { "snippet_type", "5" }
                                               }
                                           }
                                       },
                                       cookiesContainer,
                                       new Dictionary<string, string>()
                                       {
                                           { "X-CSRF-Token", token }
                                       },
                                       true);
            }
        }

        public static JToken GetNewBootstrap(CookieContainer cookiesContainer)
        {
            return Common.GetJson<JToken>(NewBootstrapUrl, cookiesContainer);
        }

        public static string UploadNewProductMockup(string file, Stream input, string token, CookieContainer cookiesContainer)
        {
            return Common.PostTextAsFile(UploadNewProductMockupsUrl,
                                         file,
                                         input,
                                         cookiesContainer,
                                         new Dictionary<string, string>
                                         {
                                             {"X-CSRF-Token", token},
                                             {"Content-Type", "image/" + Path.GetExtension(file).TrimStart('.')}
                                         });
        }

        public static string UploadNewProduct(TeespringNewItem item, string token, CookieContainer cookiesContainer)
        {
            return Common.PostText(LaunchNewProductUrl,
                                   item,
                                   cookiesContainer,
                                   new Dictionary<string, string>
                                   {
                                       {"X-CSRF-Token", token},
                                       {"Content-Type", "application/json"}
                                   });
        }

        public static JToken ValidateAddNewProduct(string url, string productId, CookieContainer cookiesContainer)
        {
            return Common.GetJson<JToken>(string.Format(ValidateAddNewProductUrl, url, productId), cookiesContainer);
        }

        public static string AddNewProductToCampaign(TeespringNewItem item, string token, CookieContainer cookiesContainer)
        {
            item.Title = null;
            item.Description = null;
            return Common.PostText(AddNewProductToCampaignUrl,
                                   item,
                                   cookiesContainer,
                                   new Dictionary<string, string>
                                   {
                                       {"X-CSRF-Token", token},
                                       {"Content-Type", "application/json"}
                                   });
        }
    }
}
