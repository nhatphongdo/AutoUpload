using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using AutoUpload.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutoUpload.Shared
{
    public class Teezily
    {
        private static readonly Dictionary<string, Dictionary<string, decimal>> RecommendedPrices
            = new Dictionary<string, Dictionary<string, decimal>>()
              {
                  {
                      "TSRN_U", new Dictionary<string, decimal>()
                                {
                                    {"eur", 21.95M},
                                    {"usd", 21.95M},
                                    {"gbp", 18.95M},
                                    {"aud", 29.95M}
                                }
                  },
                  {
                      "TSRN_W", new Dictionary<string, decimal>()
                                {
                                    {"eur", 21.95M},
                                    {"usd", 23.95M},
                                    {"gbp", 18.95M},
                                    {"aud", 31.95M}
                                }
                  },
                  {
                      "TSVN_U", new Dictionary<string, decimal>()
                                {
                                    {"eur", 22.95M},
                                    {"usd", 23.95M},
                                    {"gbp", 19.95M},
                                    {"aud", 31.95M}
                                }
                  },
                  {
                      "TSVN_W", new Dictionary<string, decimal>()
                                {
                                    {"eur", 22.95M},
                                    {"usd", 23.95M},
                                    {"gbp", 19.95M},
                                    {"aud", 31.95M}
                                }
                  },
                  {
                      "TSLS_U", new Dictionary<string, decimal>()
                                {
                                    {"eur", 27.95M},
                                    {"usd", 29.95M},
                                    {"gbp", 24.95M},
                                    {"aud", 40.95M}
                                }
                  },
                  {
                      "TKTP_U", new Dictionary<string, decimal>()
                                {
                                    {"eur", 20.95M},
                                    {"usd", 24.95M},
                                    {"gbp", 19.95M},
                                    {"aud", 31.95M}
                                }
                  },
                  {
                      "TKTP_W", new Dictionary<string, decimal>()
                                {
                                    {"eur", 22.95M},
                                    {"usd", 23.95M},
                                    {"gbp", 19.95M},
                                    {"aud", 31.95M}
                                }
                  },
                  {
                      "HOOD_U", new Dictionary<string, decimal>()
                                {
                                    {"eur", 34.95M},
                                    {"usd", 38.95M},
                                    {"gbp", 28.95M},
                                    {"aud", 49.95M}
                                }
                  },
                  {
                      "SWTR_U", new Dictionary<string, decimal>()
                                {
                                    {"eur", 29.95M},
                                    {"usd", 31.95M},
                                    {"gbp", 26.95M},
                                    {"aud", 44.95M}
                                }
                  },
                  {
                      "TSRN_K", new Dictionary<string, decimal>()
                                {
                                    {"eur", 21.95M},
                                    {"usd", 22.95M},
                                    {"gbp", 18.95M},
                                    {"aud", 30.95M}
                                }
                  },
                  {
                      "HOOD_K", new Dictionary<string, decimal>()
                                {
                                    {"eur", 30.95M},
                                    {"usd", 37.95M},
                                    {"gbp", 24.95M},
                                    {"aud", 36.95M}
                                }
                  },
                  {
                      "BODY_B", new Dictionary<string, decimal>()
                                {
                                    {"eur", 18.95M},
                                    {"usd", 22.95M},
                                    {"gbp", 14.95M},
                                    {"aud", 28.95M}
                                }
                  },
                  {
                      "MUG_U", new Dictionary<string, decimal>()
                               {
                                   {"eur", 15.95M},
                                   {"usd", 18.95M},
                                   {"gbp", 13.95M},
                                   {"aud", 24.95M}
                               }
                  },
                  {
                      "PTSRN_U", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 23.95M},
                                     {"usd", 23.95M},
                                     {"gbp", 20.95M},
                                     {"aud", 33.95M}
                                 }
                  },
                  {
                      "PHOOD_U", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 39.95M},
                                     {"usd", 39.95M},
                                     {"gbp", 33.95M},
                                     {"aud", 55.95M}
                                 }
                  },
                  {
                      "PSWTR_U", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 33.95M},
                                     {"usd", 33.95M},
                                     {"gbp", 28.95M},
                                     {"aud", 47.95M}
                                 }
                  },
                  {
                      "TOTE_U", new Dictionary<string, decimal>()
                                {
                                    {"eur", 16.95M},
                                    {"usd", 19.95M},
                                    {"gbp", 14.95M},
                                    {"aud", 23.95M}
                                }
                  },
                  {
                      "OHOOD_U", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 44.95M},
                                     {"usd", 44.95M},
                                     {"gbp", 38.95M},
                                     {"aud", 60.95M}
                                 }
                  },
                  {
                      "OSWTR_U", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 37.95M},
                                     {"usd", 37.95M},
                                     {"gbp", 32.95M},
                                     {"aud", 52.95M}
                                 }
                  },
                  {
                      "OTSLS_U", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 32.95M},
                                     {"usd", 32.95M},
                                     {"gbp", 28.95M},
                                     {"aud", 45.95M}
                                 }
                  },
                  {
                      "OTSRN_U", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 24.95M},
                                     {"usd", 24.95M},
                                     {"gbp", 21.95M},
                                     {"aud", 34.95M}
                                 }
                  },
                  {
                      "OTSRN_W", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 24.95M},
                                     {"usd", 24.95M},
                                     {"gbp", 21.95M},
                                     {"aud", 34.95M}
                                 }
                  },
                  {
                      "OTSVN_U", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 25.95M},
                                     {"usd", 25.95M},
                                     {"gbp", 22.95M},
                                     {"aud", 35.95M}
                                 }
                  },
                  {
                      "OTSVN_W", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 25.95M},
                                     {"usd", 25.95M},
                                     {"gbp", 22.95M},
                                     {"aud", 35.95M}
                                 }
                  },
                  {
                      "IP6C_U", new Dictionary<string, decimal>()
                                {
                                    {"eur", 17.95M},
                                    {"usd", 18.95M},
                                    {"gbp", 16.95M},
                                    {"aud", 25.95M}
                                }
                  },
                  {
                      "IP5C_U", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 17.95M},
                                     {"usd", 18.95M},
                                     {"gbp", 16.95M},
                                     {"aud", 25.95M}
                                 }
                  },
                  {
                      "IP5SC_U", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 17.95M},
                                     {"usd", 18.95M},
                                     {"gbp", 16.95M},
                                     {"aud", 25.95M}
                                 }
                  },
                  {
                      "IP6PC_U", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 17.95M},
                                     {"usd", 18.95M},
                                     {"gbp", 16.95M},
                                     {"aud", 25.95M}
                                 }
                  },
                  {
                      "SGS6C_U", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 17.95M},
                                     {"usd", 18.95M},
                                     {"gbp", 16.95M},
                                     {"aud", 25.95M}
                                 }
                  },
                  {
                      "SGS5C_U", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 17.95M},
                                     {"usd", 18.95M},
                                     {"gbp", 16.95M},
                                     {"aud", 25.95M}
                                 }
                  },
                  {
                      "NECKL_R", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 27.95M},
                                     {"usd", 29.95M},
                                     {"gbp", 24.95M},
                                     {"aud", 40.95M}
                                 }
                  },
                  {
                      "NECKL_S", new Dictionary<string, decimal>()
                                 {
                                     {"eur", 27.95M},
                                     {"usd", 29.95M},
                                     {"gbp", 24.95M},
                                     {"aud", 40.95M}
                                 }
                  }
              };

        private static readonly string BaseUrl = @"https://www.teezily.com";
        private static readonly string GetAccessTokenApiUrl = $"{BaseUrl}/oauth/token";
        private static readonly string GetProductsApiUrl = $"{BaseUrl}/api/v1/products";
        private static readonly string CreateCampaignApiUrl = $"{BaseUrl}/api/v1/campaigns";
        private static readonly string UploadImageApiUrl = $"{BaseUrl}/api/v1/images";
        private static readonly string LoginUrl = $"{BaseUrl}/users/sign_in";
        private static readonly string CampaignListUrl = $"{BaseUrl}/account/campaigns";
        private static readonly string CreateCampaignUrl = $"{BaseUrl}/campaigns/create_from_get";
        private static readonly string ProductCategoriesUrl = $"{BaseUrl}/api/1/categories";
        private static readonly string CheckSlugUrl = $"{BaseUrl}/campaigns/is_slug_free/";
        private static readonly string GetPriceUrl = $"{BaseUrl}/campaigns/pricing";
        private static readonly string UploadImageUrl = $"{BaseUrl}/images";
        private static readonly string AnalyzeImageUrl = $"{BaseUrl}/images/analyze.json";
        private static readonly string UploadCampaignUrl = BaseUrl + "/campaigns/{0}?format=json";
        private static readonly string PublishCampaignUrl = BaseUrl + "/campaigns/{0}/publish";
        private static readonly string EditCampaignUrl = BaseUrl + "/campaigns/{0}/edit";
        private static readonly string UpdateFacebookPixelUrl = BaseUrl + "/account/campaigns/{0}?setting_context=fb-custom-audience-pixel";
        private static readonly string UpdateCampaignUrl = BaseUrl + "/account/campaigns/{0}";

        public static string GetAccessTokenApi(string applicationId, string secret, string username, string password)
        {
            var content = Common.PostJson<JToken>(GetAccessTokenApiUrl, new Dictionary<string, string>
                                                                        {
                                                                            {"client_id", applicationId},
                                                                            {"client_secret", secret},
                                                                            {"grant_type", "password"},
                                                                            {"email", username},
                                                                            {"password", password}
                                                                        }, new CookieContainer());

            return content == null ? "" : content["access_token"].ToObject<string>() ?? "";
        }

        public static string GetProductsApi(string token)
        {
            var content = Common.GetText(GetProductsApiUrl, new CookieContainer(),
                                         new Dictionary<string, string>()
                                         {
                                             {"Authorization", $"Bearer {token}"}
                                         });
            return content;
        }

        public static string GetToken(CookieContainer cookiesContainer)
        {
            var content = Common.GetText(LoginUrl, cookiesContainer);
            if (string.IsNullOrEmpty(content))
            {
                return "";
            }
            var regex = new Regex("<meta content=(\"|')(?<token>.*)(\"|') name=(\"|')csrf-token(\"|') />");
            var groups = regex.Match(content).Groups;
            return groups["token"].Captures.Count > 0 ? groups["token"].Value : "";
        }

        public static bool Login(string username, string password, CookieContainer cookiesContainer)
        {
            var token = GetToken(cookiesContainer);
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var result = Common.PostTextAsForm(LoginUrl,
                                               new List<KeyValuePair<string, string>>
                                               {
                                                   new KeyValuePair<string, string>("utf8", "✓"),
                                                   new KeyValuePair<string, string>("authenticity_token", token),
                                                   new KeyValuePair<string, string>("user[email]", username),
                                                   new KeyValuePair<string, string>("user[password]", password),
                                                   new KeyValuePair<string, string>("user[remember_me]", "1"),
                                                   new KeyValuePair<string, string>("commit", "Login")
                                               }, cookiesContainer, null, true);

            return result != null && (result.Contains("You have logged in successfully!") || result.Contains("Logout"));
        }

        public static JToken GetProducts(CookieContainer cookiesContainer)
        {
            var result = Common.GetJson<JToken>(ProductCategoriesUrl, cookiesContainer,
                                                new Dictionary<string, string>()
                                                {
                                                    {"X-Requested-With", "XMLHttpRequest"},
                                                    {"Accept-Language", "en-US,en;q=0.8,es;q=0.6,vi;q=0.4,zh-CN;q=0.2,zh;q=0.2"},
                                                    {"Accept", "application/json, text/plain, */*"},
                                                    {"Accept-Encoding", "gzip, deflate, sdch, br"}
                                                });
            return result;
        }

        public static string GetImageLink(string imageName)
        {
            return BaseUrl + "/" + imageName.TrimStart('/');
        }

        public static (string CampaignId, string Token, string AppToken, JArray Categories, JToken CampaignInfo) CreateCampaign(CookieContainer cookiesContainer)
        {
            var content = Common.GetText(CreateCampaignUrl, cookiesContainer);
            if (string.IsNullOrEmpty(content))
            {
                return ("", "", "", null, null);
            }

            var regex = new Regex("<meta content=(\"|')(?<token>.*)(\"|') name=(\"|')csrf-token(\"|') />");
            var groups = regex.Match(content).Groups;
            var token = groups["token"].Captures.Count > 0 ? groups["token"].Value : "";

            regex = new Regex("<div ng-init=\"campaignReference='(?<campaign_id>.*)';init\\(\\)\"></div>");
            groups = regex.Match(content).Groups;
            var campaignId = groups["campaign_id"].Captures.Count > 0 ? groups["campaign_id"].Value : "";

            regex = new Regex("<div tee-app-token=(\"|')(?<tee_app_token>.*)(\"|')></div>");
            groups = regex.Match(content).Groups;
            var appToken = groups["tee_app_token"].Captures.Count > 0 ? groups["tee_app_token"].Value : "";

            regex = new Regex("window\\.marketplace_categories = (?<categories>.*)");
            groups = regex.Match(content).Groups;
            var categories = JsonConvert.DeserializeObject<JArray>(groups["categories"].Captures.Count > 0 ? groups["categories"].Value.Trim(' ', '\n', '\r', '\t') : "[]");

            var info = Common.GetJson<JToken>(string.Format(UploadCampaignUrl, campaignId), cookiesContainer);

            return (campaignId, token, appToken, categories, info);
        }

        public static bool CheckUrl(string url, CookieContainer cookiesContainer)
        {
            var result = Common.GetJson<JToken>(CheckSlugUrl + url, cookiesContainer,
                                                new Dictionary<string, string>()
                                                {
                                                    {"X-Requested-With", "XMLHttpRequest"},
                                                    {"Accept-Language", "en-US,en;q=0.8,es;q=0.6,vi;q=0.4,zh-CN;q=0.2,zh;q=0.2"},
                                                    {"Accept", "application/json, text/plain, */*"},
                                                    {"Accept-Encoding", "gzip, deflate, sdch, br"}
                                                });
            return result?["available"]?.ToObject<bool>() == true;
        }

        public static decimal GetPrice(int productId, string color, int goal, TeezilyDrawableArea front, TeezilyDrawableArea back, CookieContainer cookiesContainer)
        {
            var drawableAreas = new Dictionary<string, object>();
            if (front == null)
            {
                drawableAreas.Add("front", new Dictionary<string, object>()
                                           {
                                               {"save_object", new object[] { }},
                                               {"side_id", 1}
                                           });
            }
            else
            {
                drawableAreas.Add("front", new Dictionary<string, object>()
                                           {
                                               {"save_object", front.SaveObject},
                                               {"side_id", front.SideId}
                                           });
            }
            if (back == null)
            {
                drawableAreas.Add("back", new Dictionary<string, object>()
                                          {
                                              {"save_object", new object[] { }},
                                              {"side_id", 2}
                                          });
            }
            else
            {
                drawableAreas.Add("back", new Dictionary<string, object>()
                                          {
                                              {"save_object", back.SaveObject},
                                              {"side_id", back.SideId}
                                          });
            }
            var result = Common.PutTextAsJson(GetPriceUrl,
                                              new Dictionary<string, object>()
                                              {
                                                  {
                                                      "pricing", new Dictionary<string, object>()
                                                                 {
                                                                     {
                                                                         "items", new object[]
                                                                                  {
                                                                                      new Dictionary<string, object>()
                                                                                      {
                                                                                          {"product_id", productId},
                                                                                          {"color", color}
                                                                                      }
                                                                                  }
                                                                     },
                                                                     {"sales_goal", goal},
                                                                     {"drawable_areas", drawableAreas}
                                                                 }
                                                  }
                                              },
                                              cookiesContainer);
            if (result == null)
            {
                return 0;
            }

            try
            {
                var json = JsonConvert.DeserializeObject<JArray>(result);
                return json.Count > 0 ? json[0]["base_prices"]["us"].ToObject<decimal>() : 0;
            }
            catch (Exception exc)
            {
                return 0;
            }
        }

        public static decimal GetRecommendedPrice(string productCode, string currency)
        {
            if (RecommendedPrices.ContainsKey(productCode) && RecommendedPrices[productCode].ContainsKey(currency))
            {
                return RecommendedPrices[productCode][currency];
            }

            return 0;
        }

        public static JToken UploadImage(string file, Stream input, CookieContainer cookiesContainer)
        {
            var result = Common.PostTextAsMultipartForm(UploadImageUrl,
                                                        new List<KeyValuePair<string, object>>()
                                                        {
                                                            new KeyValuePair<string, object>("files[]=" + Path.GetFileName(file), input)
                                                        },
                                                        cookiesContainer,
                                                        new Dictionary<string, string>()
                                                        {
                                                            {"Connection", "Keep-Alive"},
                                                            {"Keep-Alive", "timeout=600"},
                                                            {"X-Requested-With", "XMLHttpRequest"},
                                                            {"Accept-Language", "en-US,en;q=0.8,es;q=0.6,vi;q=0.4,zh-CN;q=0.2,zh;q=0.2"},
                                                            {"Accept", "application/json, text/plain, */*"},
                                                            {"Accept-Encoding", "gzip, deflate, sdch, br"}
                                                        });

            if (result != null)
            {
                try
                {
                    var imageResult = JsonConvert.DeserializeObject<JToken>(result);
                    var analyzeResult = Common.PutTextAsJson(AnalyzeImageUrl,
                                                             new Dictionary<string, object>()
                                                             {
                                                                 {
                                                                     "image", new Dictionary<string, string>()
                                                                              {
                                                                                  {"sha1", imageResult[0]["sha1"].ToObject<string>()},
                                                                                  {"format", imageResult[0]["format"].ToObject<string>()}
                                                                              }
                                                                 }
                                                             },
                                                             cookiesContainer,
                                                             new Dictionary<string, string>()
                                                             {
                                                                 {"Connection", "Keep-Alive"},
                                                                 {"Keep-Alive", "timeout=600"},
                                                                 {"X-Requested-With", "XMLHttpRequest"},
                                                                 {"Accept-Language", "en-US,en;q=0.8,es;q=0.6,vi;q=0.4,zh-CN;q=0.2,zh;q=0.2"},
                                                                 {"Accept", "application/json, text/plain, */*"},
                                                                 {"Accept-Encoding", "gzip, deflate, sdch, br"}
                                                             });

                    return imageResult;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return null;
        }

        public static JToken UploadCampaign(TeezilyItem item, string token, CookieContainer cookiesContainer)
        {
            var result = Common.PutTextAsJson(string.Format(UploadCampaignUrl, item.Reference),
                                              new Dictionary<string, object>()
                                              {
                                                  {"campaign", item}
                                              },
                                              cookiesContainer,
                                              new Dictionary<string, string>()
                                              {
                                                  {"Connection", "Keep-Alive"},
                                                  {"Keep-Alive", "timeout=600"},
                                                  {"X-CSRF-Token", token},
                                                  {"X-Requested-With", "XMLHttpRequest"},
                                                  {"Accept-Language", "en-US,en;q=0.8,es;q=0.6,vi;q=0.4,zh-CN;q=0.2,zh;q=0.2"},
                                                  {"Accept", "application/json, text/plain, */*"},
                                                  {"Accept-Encoding", "gzip, deflate, sdch, br"},
                                                  {"Referer", string.Format(EditCampaignUrl, item.Reference)}
                                              });
            return result;
        }

        public static string LaunchCampaign(string publishUrl, string campaignId, CookieContainer cookiesContainer)
        {
            var result = Common.GetText(publishUrl, //string.Format(PublishCampaignUrl, campaignId),
                                        cookiesContainer,
                                        new Dictionary<string, string>()
                                        {
                                            {"Accept-Language", "en-US,en;q=0.8,es;q=0.6,vi;q=0.4,zh-CN;q=0.2,zh;q=0.2"},
                                            {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"},
                                            {"Accept-Encoding", "gzip, deflate, sdch, br"},
                                            {"Referer", string.Format(EditCampaignUrl, campaignId)}
                                        });
            return result;
        }

        public static string UpdateFacebookPixel(string campaignId, string facebook, string token, CookieContainer cookiesContainer)
        {
            var result = Common.PostTextAsForm(string.Format(UpdateFacebookPixelUrl, campaignId),
                                               new List<KeyValuePair<string, string>>()
                                               {
                                                   new KeyValuePair<string, string>("utf8", "✓"),
                                                   new KeyValuePair<string, string>("authenticity_token", token),
                                                   new KeyValuePair<string, string>("_method", "put"),
                                                   new KeyValuePair<string, string>("campaign[fb_custom_audience_pixel_id]", facebook),
                                                   new KeyValuePair<string, string>("commit", "Save")
                                               },
                                               cookiesContainer,
                                               new Dictionary<string, string>()
                                               {
                                                   {"Accept-Language", "en-US,en;q=0.8,es;q=0.6,vi;q=0.4,zh-CN;q=0.2,zh;q=0.2"},
                                                   {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"},
                                                   {"Accept-Encoding", "gzip, deflate, br"}
                                               }
                                              );

            return result;
        }

        public static string UpdateRelaunch(string campaignId, string token, CookieContainer cookiesContainer)
        {
            var result = Common.PostTextAsForm(string.Format(UpdateCampaignUrl, campaignId),
                                               new List<KeyValuePair<string, string>>()
                                               {
                                                   new KeyValuePair<string, string>("utf8", "✓"),
                                                   new KeyValuePair<string, string>("authenticity_token", token),
                                                   new KeyValuePair<string, string>("_method", "put"),
                                                   new KeyValuePair<string, string>("campaign[auto_relaunch]", "1"),
                                                   new KeyValuePair<string, string>("commit", "Confirm")
                                               },
                                               cookiesContainer,
                                               new Dictionary<string, string>()
                                               {
                                                   {"Accept-Language", "en-US,en;q=0.8,es;q=0.6,vi;q=0.4,zh-CN;q=0.2,zh;q=0.2"},
                                                   {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"},
                                                   {"Accept-Encoding", "gzip, deflate, br"}
                                               }
                                              );

            return result;
        }
    }
}
