using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace AutoFacebook
{
    public class FacebookApi
    {
        #region Constants

        private const string ApplicationId = "309325062735951";
        private const string ApplicationSecret = "34e511b63e6f0180d8bc61e95a462826";

        private const int WaitTimeout = 5000;

        private static readonly string GetClientAccessTokenUrl = $@"https://graph.facebook.com/oauth/access_token?client_id={ApplicationId}&client_secret={ApplicationSecret}&grant_type=client_credentials";
        private static readonly string GetObjectIdUrl = @"https://graph.facebook.com/{0}?access_token={1}";

        //private static readonly string LoginUrl = $@"https://www.facebook.com/dialog/oauth?client_id={AppId}&display=popup&response_type=token&scope=publish_actions,ads_management,ads_read&redirect_uri=https://www.facebook.com/connect/login_success.html";
        private const string LoginUrl = "https://www.facebook.com";

        #endregion

        #region Properties

        public IWebDriver InternalWebBrowser { get; set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Graph Api

        public static string GetClientAccessToken()
        {
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetStringAsync(GetClientAccessTokenUrl).Result;
                if (response.ToLower().StartsWith("access_token="))
                {
                    return response.Substring("access_token=".Length);
                }
            }

            return "";
        }

        public static string GetObjectId(string name)
        {
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetStringAsync(GetClientAccessTokenUrl).Result;
                if (response.ToLower().StartsWith("access_token="))
                {
                    return response.Substring("access_token=".Length);
                }
            }

            return "";
        }

        public static string GetLinkContent(string url)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2;WOW64; Trident / 6.0)");
                var response = httpClient.GetStringAsync(url).Result;
                return response;
            }
        }

        #endregion

        public static IWebDriver CreateWebDriver()
        {
            var options = new ChromeOptions();
            options.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2);
            var webdriver = new ChromeDriver(options);
            return webdriver;
        }

        public FacebookApi(IWebDriver internalWebBrowser)
        {
            InternalWebBrowser = internalWebBrowser;
        }

        public bool Login(string email, string password)
        {
            InternalWebBrowser.Navigate().GoToUrl(LoginUrl);

            try
            {
                // Input account and login
                InternalWebBrowser.FindElement(By.Id("email")).SendKeys(email);
                InternalWebBrowser.FindElement(By.Id("pass")).SendKeys(password);
                InternalWebBrowser.FindElement(By.Id("loginbutton")).FindElement(By.CssSelector("[type=submit]")).Click();

                if (InternalWebBrowser.Url.ToLower().Contains("login") &&
                    InternalWebBrowser.Url.ToLower().Contains("next"))
                {
                    ErrorMessage = "Login failed (wrong username or password)";
                    return false;
                }

                return true;
            }
            catch (Exception exc)
            {
                ErrorMessage = exc.Message;
                return false;
            }
        }

        public bool PostOnWall(string message)
        {
            InternalWebBrowser.Navigate().GoToUrl("https://www.facebook.com");

            try
            {
                InternalWebBrowser.FindElement(By.Id("feedx_container"))
                    .FindElement(By.CssSelector("textarea[name=xhpc_message]"))
                    .SendKeys(message);

                InternalWebBrowser.FindElement(By.Id("feedx_container"))
                    .FindElement(By.CssSelector("button[type=submit]")).Click();

                return true;
            }
            catch (Exception exc)
            {
                ErrorMessage = exc.Message;
                return false;
            }
        }

        public bool PostOnPage(string page, string message)
        {
            InternalWebBrowser.Navigate().GoToUrl(page);

            try
            {
                InternalWebBrowser.FindElement(By.Id("pagelet_timeline_main_column"))
                    .FindElement(By.CssSelector("textarea[name=xhpc_message_text]"))
                    .SendKeys(message);

                InternalWebBrowser.FindElement(By.Id("pagelet_timeline_main_column"))
                    .FindElement(By.CssSelector("button[type=submit]")).Click();

                return true;
            }
            catch (Exception exc)
            {
                ErrorMessage = "Cannot post on this page.";
                return false;
            }
        }

        public bool PostOnGroup(string group, string message, params string[] files)
        {
            InternalWebBrowser.Navigate().GoToUrl(group);

            try
            {
                InternalWebBrowser.FindElement(By.Id("pagelet_group_composer"))
                    .FindElement(By.CssSelector("textarea[name=xhpc_message_text]"))
                    .SendKeys(message);

                new WebDriverWait(InternalWebBrowser, TimeSpan.FromSeconds(WaitTimeout))
                    .Until(ExpectedConditions.ElementExists(By.CssSelector("#pagelet_group_composer button[type=submit]")));

                // Upload files
                if (files.Length > 0)
                {
                    InternalWebBrowser.FindElement(By.Id("pagelet_group_composer"))
                        .FindElement(By.CssSelector("a[data-testid=media-attachment-selector]"))
                        .Click();

                    new WebDriverWait(InternalWebBrowser, TimeSpan.FromSeconds(WaitTimeout))
                        .Until(ExpectedConditions.ElementExists(By.CssSelector("#pagelet_group_composer input[type=file]")));

                    InternalWebBrowser.FindElement(By.Id("pagelet_group_composer"))
                        .FindElement(By.CssSelector("input[type=file]"))
                        .FindElement(By.XPath("./parent::*"))
                        .FindElement(By.XPath("./parent::*"))
                        .Click();

                    Thread.Sleep(1000);

                    InternalWebBrowser.SwitchTo();

                    // Construct proper string for SendKeys
                    var file = string.Join(" ", files.Select(x => '"' + x + '"'));
                    var filePath = file.Replace("{", "*{?");
                    filePath = filePath.Replace("}", "*}?");
                    filePath = filePath.Replace("+", "*+?");
                    filePath = filePath.Replace("^", "*^?");
                    filePath = filePath.Replace("%", "*%?");
                    filePath = filePath.Replace("~", "*~?");
                    filePath = filePath.Replace("^", "*^?");
                    filePath = filePath.Replace("(", "*(?");
                    filePath = filePath.Replace(")", "*)?");
                    filePath = filePath.Replace('*', '{');
                    filePath = filePath.Replace('?', '}');
                    SendKeys.SendWait(filePath);
                    SendKeys.SendWait("{ENTER}");

                    new WebDriverWait(InternalWebBrowser, TimeSpan.FromSeconds(WaitTimeout))
                                .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#pagelet_group_composer button[type=submit]")));
                }

                new WebDriverWait(InternalWebBrowser, TimeSpan.FromSeconds(WaitTimeout))
                            .Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#pagelet_group_composer button[type=submit]")));

                InternalWebBrowser.FindElement(By.Id("pagelet_group_composer"))
                    .FindElement(By.CssSelector("button[type=submit]")).Click();

                new WebDriverWait(InternalWebBrowser, TimeSpan.FromSeconds(WaitTimeout))
                    .Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("#pagelet_group_composer button[type=submit]")));

                return true;
            }
            catch (Exception exc)
            {
                ErrorMessage = "You must join to group to post.";
                return false;
            }
        }

        public bool JoinToGroup(string group)
        {
            InternalWebBrowser.Navigate().GoToUrl(group);

            try
            {
                var buttons = InternalWebBrowser.FindElement(By.Id("fbProfileCover"))
                    .FindElements(By.CssSelector("a[role=button]"));

                foreach (var button in buttons)
                {
                    if (button.Text.ToLower() == "join group")
                    {
                        button.Click();

                        return true;
                    }
                }

                ErrorMessage = "You already joined to this group.";
                return false;
            }
            catch (Exception exc)
            {
                ErrorMessage = exc.Message;
                return false;
            }
        }
    }
}
