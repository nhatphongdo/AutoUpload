using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AutoFacebookWebsite
{
    public class FacebookClient
    {
        private const string GraphUrl = "https://graph.facebook.com";

        public string AccessToken { get; set; }

        public string ApplicationId { get; set; }

        public string ApplicationSecret { get; set; }

        public FacebookClient(string accessToken, string appId, string appSecret)
        {
            AccessToken = accessToken;
            ApplicationId = appId;
            ApplicationSecret = appSecret;
        }

        public static FacebookClient CreateInstance(string accessToken, IConfigurationRoot configuration)
        {
            return new FacebookClient(accessToken, configuration["Facebook:ApplicationId"], configuration["Facebook:ApplicationSecret"]);
        }

        public async Task<string> GetLongLivedToken()
        {
            var url = $@"{GraphUrl}/oauth/access_token?grant_type=fb_exchange_token&client_id={ApplicationId}&client_secret={ApplicationSecret}&fb_exchange_token={AccessToken}";
            var result = await GetResponseAsText(url);
            if (result.LastIndexOf('&') > 0 && result.ToLower().StartsWith("access_token"))
            {
                var length = "access_token=".Length;
                return result.Substring(length, result.LastIndexOf('&') - length);
            }
            return "";
        }

        public async Task<dynamic> GetTokenInfo()
        {
            var url = $@"{GraphUrl}/debug_token?input_token={AccessToken}&access_token={ApplicationId}|{ApplicationSecret}";
            var result = await GetResponseAsJson<dynamic>(url);
            return result;
        }

        public async Task<string> UploadPhoto(string objectId, dynamic photo)
        {
            var url = $@"{GraphUrl}/{objectId}/photos";
            var result = await GetResponseAsJson<dynamic>(url, true, new Dictionary<string, string>()
            {
                {"caption", photo.Caption},
                {"published", (photo.Published ?? true).ToString()},
                {"access_token", AccessToken}
            }, new string[] { photo.FilePath });

            if (result == null || result.error != null)
            {
                throw new Exception(result != null ? (string)result.error.message : "Result is null");
            }

            return result.id;
        }

        public async Task<string> Post(string objectId, string message, string[] files = null)
        {
            // Upload photo
            var photoIds = new List<string>();
            if (files != null)
            {
                foreach (var file in files)
                {
                    var photoId = await UploadPhoto("me", new
                    {
                        Caption = Path.GetFileNameWithoutExtension(file),
                        FilePath = file,
                        Published = false
                    });
                    photoIds.Add(photoId);
                }
            }

            var url = $@"{GraphUrl}/{objectId}/feed";
            var parameters = new Dictionary<string, string>()
            {
                //{"privacy", "{\"value\": \"EVERYONE\"}"}, // Privacy: EVERYONE, ALL_FRIENDS, FRIENDS_OF_FRIENDS, SELF
                {"message", message},
                {"access_token", AccessToken}
            };

            for (var i = 0; i < photoIds.Count; i++)
            {
                parameters[$"attached_media[{i}]"] = $@"{{""media_fbid"":""{photoIds[i]}""}}";
            }

            var result = await GetResponseAsJson<dynamic>(url, true, parameters);

            if (result == null || result.error != null)
            {
                throw new Exception(result != null ? (string)result.error.message : "Result is null");
            }

            return result.id;
        }

        private async Task<string> GetResponseAsText(string url, bool isPost = false, IDictionary<string, string> parameters = null, string[] files = null)
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response;

                if (isPost)
                {
                    var content = new MultipartFormDataContent($@"Upload----{DateTime.Now}");
                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            if (File.Exists(file))
                            {
                                content.Add(new StreamContent(new FileStream(file, FileMode.Open, FileAccess.Read)),
                                    Path.GetFileNameWithoutExtension(file), Path.GetFileName(file));
                            }
                        }
                    }
                    if (parameters != null)
                    {
                        foreach (var key in parameters.Keys)
                        {
                            content.Add(new StringContent(parameters[key], Encoding.UTF8), key);
                        }
                    }

                    response = await httpClient.PostAsync(url, content);
                }
                else
                {
                    response = await httpClient.GetAsync(url);
                }

                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }

        private async Task<T> GetResponseAsJson<T>(string url, bool isPost = false, IDictionary<string, string> parameters = null, string[] files = null)
        {
            var content = await GetResponseAsText(url, isPost, parameters, files);
            if (string.IsNullOrEmpty(content))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
