using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PCLCrypto;

namespace AutoUpload.Shared
{
    public static class Common
    {
        public static readonly int MaximumTries = 3;
        public static readonly int NetworkDelay = 500;
        public static readonly string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";

        public static T GetJson<T>(string link, CookieContainer cookies, Dictionary<string, string> headers = null)
        {
            try
            {
                var content = GetText(link, cookies, headers);
                if (content == null)
                {
                    return default(T);
                }

                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return default(T);
            }
        }

        public static string GetText(string link, CookieContainer cookies, Dictionary<string, string> headers = null, int tries = 1, bool autoRedirect = true)
        {
            using (var httpHandler = new HttpClientHandler()
                                     {
                                         CookieContainer = cookies,
                                         AllowAutoRedirect = autoRedirect,
                                         AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                                     })
            using (var httpClient = new HttpClient(httpHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                try
                {
                    //var response = httpClient.GetAsync(link).Result;
                    //var content = response.Content.ReadAsStringAsync().Result;
                    var content = httpClient.GetStringAsync(link).Result;
                    //content = httpClient.GetStringAsync("https://www.teezily.com/campaigns/C-GGGCA/publish").Result;
                    return content;
                }
                catch (HttpRequestException ex)
                {
                    Logger.WriteLog(ex);

                    if (tries == MaximumTries)
                    {
                        return null;
                    }

                    Task.Delay(TimeSpan.FromMilliseconds(NetworkDelay)).RunSynchronously();

                    return GetText(link, cookies, headers, tries + 1);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex);
                    return null;
                }
            }
        }

        public static HttpResponseMessage GetResponse(string link, CookieContainer cookies, Dictionary<string, string> headers = null, int tries = 1)
        {
            using (var httpHandler = new HttpClientHandler()
                                     {
                                         CookieContainer = cookies,
                                         AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                                     })
            using (var httpClient = new HttpClient(httpHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                                                                         "User-Agent",
                                                                         UserAgent);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                try
                {
                    var response = httpClient.GetAsync(link).Result;
                    return response;
                }
                catch (HttpRequestException ex)
                {
                    Logger.WriteLog(ex);

                    if (tries == MaximumTries)
                    {
                        return null;
                    }

                    Task.Delay(TimeSpan.FromMilliseconds(NetworkDelay)).RunSynchronously();

                    return GetResponse(link, cookies, headers, tries + 1);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex);
                    return null;
                }
            }
        }

        public static T PostJson<T>(string link, object parameters, CookieContainer cookies, Dictionary<string, string> headers = null)
        {
            try
            {
                var content = PostText(link, parameters, cookies, headers);
                if (content == null)
                {
                    return default(T);
                }

                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return default(T);
            }
        }

        public static T PatchJson<T>(string link, object parameters, CookieContainer cookies, Dictionary<string, string> headers = null)
        {
            try
            {
                var content = PatchText(link, parameters, cookies, headers);
                if (content == null)
                {
                    return default(T);
                }

                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return default(T);
            }
        }

        public static string PostText(string link, object parameters, CookieContainer cookies, Dictionary<string, string> headers = null, bool autoRedirect = true,
                                      int tries = 1)
        {
            using (var httpHandler = new HttpClientHandler()
                                     {
                                         CookieContainer = cookies,
                                         AllowAutoRedirect = autoRedirect,
                                         AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                                     })
            using (var httpClient = new HttpClient(httpHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                try
                {
                    var response = httpClient.PostAsync(
                                                        link,
                                                        new
                                                            StringContent(
                                                                          JsonConvert.SerializeObject(
                                                                                                      parameters,
                                                                                                      new JsonSerializerSettings
                                                                                                      {
                                                                                                          NullValueHandling = NullValueHandling.Ignore
                                                                                                      }),
                                                                          Encoding.UTF8,
                                                                          "application/json"))
                                             .Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }

                    var data = JsonConvert.SerializeObject(parameters);
                    Logger.WriteLog($"Url: {link}\nParameters:{data.Substring(0, Math.Min(data.Length, 102400))}\n{response.ReasonPhrase}: {response.Content.ReadAsStringAsync().Result}");
                    return null;
                }
                catch (HttpRequestException ex)
                {
                    Logger.WriteLog(ex);

                    if (tries == MaximumTries)
                    {
                        return null;
                    }

                    Task.Delay(TimeSpan.FromMilliseconds(NetworkDelay)).RunSynchronously();

                    return PostText(link, parameters, cookies, headers, autoRedirect, tries + 1);
                }
                catch (Exception ex)
                {
                    var data = JsonConvert.SerializeObject(parameters);
                    Logger.WriteLog(ex, $"Url: {link}\nParameters:{data.Substring(0, Math.Min(data.Length, 102400))}"); // Log max 100 KB
                    return null;
                }
            }
        }

        public static HttpResponseMessage PostResponse(string link, object parameters, CookieContainer cookies, Dictionary<string, string> headers = null, int tries = 1)
        {
            using (var httpHandler = new HttpClientHandler()
                                     {
                                         CookieContainer = cookies,
                                         AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                                     })
            using (var httpClient = new HttpClient(httpHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                try
                {
                    var response = httpClient.PostAsync(
                                                        link,
                                                        new
                                                            StringContent(
                                                                          JsonConvert.SerializeObject(
                                                                                                      parameters,
                                                                                                      new JsonSerializerSettings
                                                                                                      {
                                                                                                          NullValueHandling = NullValueHandling.Ignore
                                                                                                      }),
                                                                          Encoding.UTF8,
                                                                          "application/json"))
                                             .Result;
                    return response;
                }
                catch (HttpRequestException ex)
                {
                    Logger.WriteLog(ex);

                    if (tries == MaximumTries)
                    {
                        return null;
                    }

                    Task.Delay(TimeSpan.FromMilliseconds(NetworkDelay)).RunSynchronously();

                    return PostResponse(link, parameters, cookies, headers, tries + 1);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex);
                    return null;
                }
            }
        }

        public static string PostTextAsForm(string link, List<KeyValuePair<string, string>> parameters, CookieContainer cookies,
                                            Dictionary<string, string> headers = null, bool autoRedirect = false, int tries = 1)
        {
            using (var httpHandler = new HttpClientHandler()
                                     {
                                         CookieContainer = cookies,
                                         AllowAutoRedirect = autoRedirect,
                                         AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                                     })
            using (var httpClient = new HttpClient(httpHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                try
                {
                    var response = httpClient.PostAsync(
                                                        link,
                                                        new FormUrlEncodedContent(parameters))
                                             .Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }

                    var data = JsonConvert.SerializeObject(parameters);
                    Logger.WriteLog($"Url: {link}\nParameters:{data.Substring(0, Math.Min(data.Length, 102400))}\n{response.ReasonPhrase}: {response.Content.ReadAsStringAsync().Result}");
                    return null;
                }
                catch (HttpRequestException ex)
                {
                    Logger.WriteLog(ex);

                    if (tries == MaximumTries)
                    {
                        return null;
                    }

                    Task.Delay(TimeSpan.FromMilliseconds(NetworkDelay)).RunSynchronously();

                    return PostTextAsForm(link, parameters, cookies, headers, autoRedirect, tries + 1);
                }
                catch (Exception ex)
                {
                    var data = JsonConvert.SerializeObject(parameters);
                    Logger.WriteLog(ex, $"Url: {link}\nParameters:{data.Substring(0, Math.Min(data.Length, 102400))}"); // Log max 100 KB
                    return null;
                }
            }
        }

        public static string PostTextAsMultipartForm(string link, List<KeyValuePair<string, object>> parameters, CookieContainer cookies,
                                                     Dictionary<string, string> headers = null, int tries = 1)
        {
            using (var httpHandler = new HttpClientHandler()
                                     {
                                         CookieContainer = cookies,
                                         AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                                     })
            using (var httpClient = new HttpClient(httpHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                try
                {
                    var boundary = $"----{Guid.NewGuid()}";
                    var content = new MultipartFormDataContent(boundary);
                    foreach (var item in parameters)
                    {
                        if (item.Value is Stream)
                        {
                            var itemName = item.Key.IndexOf('=') < 0 ? item.Key : item.Key.Substring(0, item.Key.IndexOf('='));
                            var fileName = item.Key.IndexOf('=') < 0 ? item.Key : item.Key.Substring(item.Key.IndexOf('=') + 1);
                            var streamContent = new StreamContent((Stream) item.Value);
                            content.Add(streamContent, itemName, fileName);
                        }
                        else if (item.Value != null)
                        {
                            content.Add(new StringContent(item.Value.ToString(), Encoding.UTF8), item.Key);
                        }
                    }

                    var response = httpClient.PostAsync(link, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }

                    Logger.WriteLog(response.Content.ReadAsStringAsync().Result);
                    return null;
                }
                catch (HttpRequestException ex)
                {
                    Logger.WriteLog(ex);

                    if (tries == MaximumTries)
                    {
                        return null;
                    }

                    Task.Delay(TimeSpan.FromMilliseconds(NetworkDelay)).RunSynchronously();

                    return PostTextAsMultipartForm(link, parameters, cookies, headers, tries + 1);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex);
                    return null;
                }
            }
        }

        public static string PostTextAsFile(string link, string file, Stream input, CookieContainer cookies, Dictionary<string, string> headers = null, int tries = 1)
        {
            using (var httpHandler = new HttpClientHandler()
                                     {
                                         CookieContainer = cookies,
                                         AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                                     })
            using (var httpClient = new HttpClient(httpHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                try
                {
                    var content = new StreamContent(input);
                    var contentType = headers != null && headers.ContainsKey("Content-Type")
                                          ? headers["Content-Type"]
                                          : "image/" + Path.GetExtension(file).TrimStart('.');
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    var response = httpClient.PostAsync(link, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }

                    Logger.WriteLog(response.Content.ReadAsStringAsync().Result);
                    return null;
                }
                catch (HttpRequestException ex)
                {
                    Logger.WriteLog(ex);

                    if (tries == MaximumTries)
                    {
                        return null;
                    }

                    Task.Delay(TimeSpan.FromMilliseconds(NetworkDelay)).RunSynchronously();

                    return PutTextAsFile(link, file, input, cookies, headers, tries + 1);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex);
                    return null;
                }
            }
        }

        public static string PutTextAsForm(string link, List<KeyValuePair<string, string>> parameters, CookieContainer cookies, Dictionary<string, string> headers = null,
                                           int tries = 1)
        {
            using (var httpHandler = new HttpClientHandler()
                                     {
                                         CookieContainer = cookies,
                                         AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                                     })
            using (var httpClient = new HttpClient(httpHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                try
                {
                    var response = httpClient.PutAsync(link, new FormUrlEncodedLongContent(parameters)).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }

                    Logger.WriteLog(response.Content.ReadAsStringAsync().Result);
                    return null;
                }
                catch (HttpRequestException ex)
                {
                    Logger.WriteLog(ex);

                    if (tries == MaximumTries)
                    {
                        return null;
                    }

                    Task.Delay(TimeSpan.FromMilliseconds(NetworkDelay)).RunSynchronously();

                    return PutTextAsForm(link, parameters, cookies, headers, tries + 1);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex);
                    return null;
                }
            }
        }

        public static string PutTextAsFormString(string link, string parameters, CookieContainer cookies, Dictionary<string, string> headers = null, int tries = 1)
        {
            using (var httpHandler = new HttpClientHandler()
                                     {
                                         CookieContainer = cookies,
                                         AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                                     })
            using (var httpClient = new HttpClient(httpHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                try
                {
                    var content = new StringContent(parameters, Encoding.UTF8);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    var response = httpClient.PutAsync(link, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }

                    Logger.WriteLog(response.Content.ReadAsStringAsync().Result);
                    return null;
                }
                catch (HttpRequestException ex)
                {
                    Logger.WriteLog(ex);

                    if (tries == MaximumTries)
                    {
                        return null;
                    }

                    Task.Delay(TimeSpan.FromMilliseconds(NetworkDelay)).RunSynchronously();

                    return PutTextAsFormString(link, parameters, cookies, headers, tries + 1);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex);
                    return null;
                }
            }
        }

        public static string PutTextAsJson(string link, object parameters, CookieContainer cookies, Dictionary<string, string> headers = null, int tries = 1)
        {
            using (var httpHandler = new HttpClientHandler()
                                     {
                                         CookieContainer = cookies,
                                         AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                                     })
            using (var httpClient = new HttpClient(httpHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                try
                {
                    var json = JsonConvert.SerializeObject(parameters);
                    var response = httpClient.PutAsync(link, new StringContent(json, Encoding.UTF8, "application/json")).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }

                    Logger.WriteLog(response.Content.ReadAsStringAsync().Result);
                    return null;
                }
                catch (HttpRequestException ex)
                {
                    Logger.WriteLog(ex);

                    if (tries == MaximumTries)
                    {
                        return null;
                    }

                    Task.Delay(TimeSpan.FromMilliseconds(NetworkDelay)).RunSynchronously();

                    return PutTextAsJson(link, parameters, cookies, headers, tries + 1);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex);
                    return null;
                }
            }
        }

        public static string PutTextAsFile(string link, string file, Stream input, CookieContainer cookies, Dictionary<string, string> headers = null, int tries = 1)
        {
            using (var httpHandler = new HttpClientHandler()
                                     {
                                         CookieContainer = cookies,
                                         AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                                     })
            using (var httpClient = new HttpClient(httpHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                try
                {
                    var content = new StreamContent(input);
                    var contentType = headers != null && headers.ContainsKey("Content-Type")
                                          ? headers["Content-Type"]
                                          : "image/" + Path.GetExtension(file).TrimStart('.');
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    var response = httpClient.PutAsync(link, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }

                    Logger.WriteLog(response.Content.ReadAsStringAsync().Result);
                    return null;
                }
                catch (HttpRequestException ex)
                {
                    Logger.WriteLog(ex);

                    if (tries == MaximumTries)
                    {
                        return null;
                    }

                    Task.Delay(TimeSpan.FromMilliseconds(NetworkDelay)).RunSynchronously();

                    return PutTextAsFile(link, file, input, cookies, headers, tries + 1);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex);
                    return null;
                }
            }
        }

        public static string PatchText(string link, object parameters, CookieContainer cookies, Dictionary<string, string> headers = null, bool autoRedirect = true,
                                       int tries = 1)
        {
            using (var httpHandler = new HttpClientHandler()
                                     {
                                         CookieContainer = cookies,
                                         AllowAutoRedirect = autoRedirect,
                                         AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                                     })
            using (var httpClient = new HttpClient(httpHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                try
                {
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), link)
                                  {
                                      Content = new StringContent(
                                                                  JsonConvert.SerializeObject(
                                                                                              parameters,
                                                                                              new JsonSerializerSettings
                                                                                              {
                                                                                                  NullValueHandling = NullValueHandling.Ignore
                                                                                              }),
                                                                  Encoding.UTF8,
                                                                  "application/json")
                                  };
                    var response = httpClient.SendAsync(request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }

                    Logger.WriteLog(response.ReasonPhrase + ": " + response.Content.ReadAsStringAsync().Result);
                    return null;
                }
                catch (HttpRequestException ex)
                {
                    Logger.WriteLog(ex);

                    if (tries == MaximumTries)
                    {
                        return null;
                    }

                    Task.Delay(TimeSpan.FromMilliseconds(NetworkDelay)).RunSynchronously();

                    return PostText(link, parameters, cookies, headers, autoRedirect, tries + 1);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex);
                    return null;
                }
            }
        }

        public static bool DownloadFile(string link, Stream outputStream, int tries = 1)
        {
            if (!outputStream.CanWrite)
            {
                return false;
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                                                                         "User-Agent",
                                                                         UserAgent);

                try
                {
                    var stream = httpClient.GetStreamAsync(link).Result;
                    stream.CopyTo(outputStream);
                    outputStream.Flush();

                    return true;
                }
                catch (HttpRequestException ex)
                {
                    Logger.WriteLog(ex);

                    if (tries == MaximumTries)
                    {
                        return false;
                    }

                    Task.Delay(TimeSpan.FromMilliseconds(NetworkDelay)).RunSynchronously();

                    return DownloadFile(link, outputStream);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex);
                    return false;
                }
            }
        }

        public static string ToBase64(this byte[] data, string contentType)
        {
            var base64String = Convert.ToBase64String(data);
            return $"data:{contentType};base64,{base64String}";
        }

        public static string Encrypt(string data, string password, string salt)
        {
            var key = CreateDerivedKey(password, salt);

            var aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            var symetricKey = aes.CreateSymmetricKey(key);
            var bytes = WinRTCrypto.CryptographicEngine.Encrypt(symetricKey, Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(bytes);
        }

        public static string Decrypt(string data, string password, string salt)
        {
            var input = Convert.FromBase64String(data);
            var key = CreateDerivedKey(password, salt);

            var aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            var symetricKey = aes.CreateSymmetricKey(key);
            var bytes = WinRTCrypto.CryptographicEngine.Decrypt(symetricKey, input);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        public static string CreateSalt(int lengthInBytes)
        {
            return Convert.ToBase64String(WinRTCrypto.CryptographicBuffer.GenerateRandom(lengthInBytes));
        }

        private static byte[] CreateDerivedKey(string password, string salt, int keyLengthInBytes = 32, int iterations = 1000)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var key = NetFxCrypto.DeriveBytes.GetBytes(password, saltBytes, iterations, keyLengthInBytes);
            return key;
        }
    }
}
