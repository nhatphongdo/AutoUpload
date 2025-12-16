using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoUpload.Shared.Models;
using Newtonsoft.Json.Linq;

namespace AutoUpload.Shared
{
    public class Sunfrog
    {
        public static readonly string BaseUrl = "https://manager.sunfrogshirts.com";
        public static readonly string DesignUrl = $"{BaseUrl}/Designer";
        public static readonly string LoginUrl = $"{BaseUrl}/Login.cfm";
        public static readonly string ProductsRateUrl = BaseUrl + "/Designer/data/sunfrog-products{0}.json";
        public static readonly string LeggingProductsRateUrl = BaseUrl + "/Designer/data/sunfrog-products-leggings.json";
        public static readonly string UploadUrl = $"{BaseUrl}/Designer/php/upload-handler.cfm";
        public static readonly string EditCampaignUrl = $"{BaseUrl}/my-art-edit.cfm?GroupID=";
        public static readonly string EditNewCampaignUrl = $"{BaseUrl}/my-art-edit.cfm?editNewDesign";

        public static CookieContainer Login(string username, string password, out string affiliateRate)
        {
            var cookiesContainer = new CookieContainer();
            var result = Common.PostTextAsForm(
                                               LoginUrl,
                                               new List<KeyValuePair<string, string>>()
                                               {
                                                   new KeyValuePair<string, string>("username", username),
                                                   new KeyValuePair<string, string>("password", password),
                                                   new KeyValuePair<string, string>("login", "Login"),
                                               },
                                               cookiesContainer, null, true);

            var designer = Common.GetText(DesignUrl, cookiesContainer) ?? "";
            affiliateRate = "";
            var affiliateIndex = designer.IndexOf("affiliateratevar", StringComparison.OrdinalIgnoreCase);
            if (affiliateIndex >= 0)
            {
                var index = designer.IndexOf("=", affiliateIndex, StringComparison.OrdinalIgnoreCase);
                var endIndex = designer.IndexOf(";", index, StringComparison.OrdinalIgnoreCase);
                if (index >= 0 && endIndex >= index)
                {
                    affiliateRate = designer.Substring(index + 1, endIndex - index - 1).Trim(' ', '\'', '"');
                }
            }

            return (result ?? "").ToLower().Contains("your seller id") ? cookiesContainer : null;
        }

        public static JArray GetProductsRate(string affiliateRate)
        {
            return Common.GetJson<JArray>(string.Format(ProductsRateUrl, affiliateRate), new CookieContainer());
        }

        public static JArray GetLeggingProductsRate()
        {
            return Common.GetJson<JArray>(LeggingProductsRateUrl, new CookieContainer());
        }

        public static string GetImageLink(string imageName)
        {
            return BaseUrl + "/Designer/" + imageName.TrimStart('/');
        }

        public static JToken Upload(SunfrogItem item, CookieContainer cookiesContainer)
        {
            return Common.PostJson<JToken>(UploadUrl, item, cookiesContainer);
        }

        public static void UpdateCampaign(SunfrogItem item, string pageName, string facebookPixel, string googleAnalytics, bool isPrivate, bool autoRestart, CookieContainer cookiesContainer)
        {
            var campaignId = pageName.Split('-', '.')[0];
            var mockupId = pageName.Split('-', '.')[1];
            var parameters = new List<KeyValuePair<string, string>>()
                                               {
                                                   new KeyValuePair<string, string>("GroupID", campaignId),
                                                   new KeyValuePair<string, string>("existpagename", pageName),
                                                   new KeyValuePair<string, string>("title", item.Title),
                                                   new KeyValuePair<string, string>("Description", item.Description),
                                                   new KeyValuePair<string, string>("Keywords", string.Join(" ", item.Keywords)),
                                                   new KeyValuePair<string, string>("mockupID", mockupId),
                                                   new KeyValuePair<string, string>("isBlackoutOriginal", "0"),
                                                   new KeyValuePair<string, string>("waslimitedtime", "0"),
                                                   new KeyValuePair<string, string>("facebookPixel", facebookPixel),
                                                   new KeyValuePair<string, string>("googleAnalytics", googleAnalytics),
                                                   new KeyValuePair<string, string>("twitterTracking", ""),
                                                   new KeyValuePair<string, string>("pinterestTracking", ""),
                                                   new KeyValuePair<string, string>("submit", "")
                                               };
            foreach (var type in item.Types)
            {
                parameters.Add(new KeyValuePair<string, string>($"amount_{type.Id}", type.Price.ToString("######.##")));
            }
            //if (autoRestart)
            //{
            //    parameters.Add(new KeyValuePair<string, string>("LimitedTime", "1"));
            //    parameters.Add(new KeyValuePair<string, string>("ltoRelaunch", "1"));
            //    parameters.Add(new KeyValuePair<string, string>("startDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm")));
            //    parameters.Add(new KeyValuePair<string, string>("endDate", DateTime.Now.AddDays(30).ToString("yyyy/MM/dd HH:mm")));
            //}
            //else
            {
                parameters.Add(new KeyValuePair<string, string>("startDate", ""));
                parameters.Add(new KeyValuePair<string, string>("endDate", ""));
            }
            if (isPrivate)
            {
                parameters.Add(new KeyValuePair<string, string>("ExcludeFromSearch", "1"));
                parameters.Add(new KeyValuePair<string, string>("DoNotAllowGoogle", "1"));
                parameters.Add(new KeyValuePair<string, string>("isBlackout", "2"));
            }

            var count = 0;
            while (count < 5)
            {
                Task.Delay(TimeSpan.FromMilliseconds(1000)).Wait();
                var result = Common.PostTextAsForm(EditCampaignUrl + campaignId,
                                                   parameters,
                                                   cookiesContainer);

                if (result.ToLower().Contains("update applied"))
                {
                    // Successful
                    break;
                }

                ++count;
            }
        }

        public static string GetNewCampaign(CookieContainer cookiesContainer)
        {
            var content = Common.GetText(EditNewCampaignUrl, cookiesContainer) ?? "";
            var index = content.IndexOf("existpagename", StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                index = content.IndexOf("value", index + 13, StringComparison.OrdinalIgnoreCase);
                var endIndex = content.IndexOf("/", index, StringComparison.OrdinalIgnoreCase);
                if (index >= 0 && endIndex >= index)
                {
                    var campaignLink = content.Substring(index + 5, endIndex - index - 5).Trim(' ', '\'', '"', '=');
                    return campaignLink;
                }
            }

            return "";
        }
    }
}
