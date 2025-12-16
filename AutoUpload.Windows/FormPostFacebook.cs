using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoUpload.Photoshop;
using AutoUpload.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutoUpload.Windows
{
    public partial class FormPostFacebook : Form
    {
        public List<DataGridViewRow> Designs { get; set; }

        public JToken LicenseData { get; set; }

        private List<JToken> facebookPages;

        public FormPostFacebook()
        {
            Designs = new List<DataGridViewRow>();
            InitializeComponent();
        }

        private void ButtonPostFacebook_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxAccessToken.Text))
            {
                MessageBox
                    .Show("You must input your Facebook's Access Token to allow application posting to your Page. To retrieve Access Token please refer to this URL: https://developers.facebook.com/tools/explorer/",
                          "Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ComboBoxFacebookPages.SelectedIndex == -1)
            {
                MessageBox.Show("You must choose a page to post.", "Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ButtonPostFacebook.Enabled = false;
            ButtonClose.Enabled = false;
            ProgressBarPostFacebook.Value = 0;
            BackgroundWorkerPostFacebook.RunWorkerAsync(new object[]
                                                        {
                                                            TextBoxAccessToken.Text, ComboBoxFacebookPages.SelectedIndex, TextBoxContent.Text
                                                        });
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BackgroundWorkerPostFacebook_DoWork(object sender, DoWorkEventArgs e)
        {
            var parameters = e.Argument as object[];
            if (parameters == null || parameters.Length != 3 || (int) parameters[1] < 0 || (int) parameters[1] >= facebookPages.Count)
            {
                e.Result = 0;
                return;
            }
            var facebookPage = facebookPages[(int) parameters[1]];

            var count = 0;
            var success = 0;
            foreach (var design in Designs)
            {
                var mockup = design.Cells["MockupTemplateFile"].Value as MockupTemplate;
                if (mockup == null)
                {
                    ++count;
                    BackgroundWorkerPostFacebook.ReportProgress(count * 100 / Designs.Count, success);
                    continue;
                }

                if (string.IsNullOrEmpty(mockup.TempFilePath))
                {
                    mockup.TempFilePath = License.DecodeTipsFile(mockup.FilePath, LicenseData["mockup"]["key"].ToObject<string>(),
                                                                 LicenseData["mockup"]["iv"].ToObject<string>());
                }
                if (!string.IsNullOrEmpty(mockup.TempFilePath))
                {
                    var designFiles = design.Cells["DesignFiles"].Value.ToString().Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        foreach (var designFile in designFiles)
                        {
                            var outputFile = Path.GetTempFileName() + ".jpg";
                            PhotoshopHelper.CreateMockup(mockup.TempFilePath, designFile, outputFile,
                                                         design.Cells["DesignBackgroundColor"].Style.BackColor, design.Cells["ShirtColor"].Style.BackColor,
                                                         null, design.Cells["LaceColor"].Style.BackColor, 0, 0,
                                                         design.Cells["HorizontalAlign"].Value as string, design.Cells["VerticalAlign"].Value as string);

                            // Post to Facebook
                            var content = (string) parameters[2];
                            var uploadInfo =
                                (design.Cells["DesignUploadInfo"].Value as string ?? "").Split(new char[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

                            var link = "";
                            foreach (var info in uploadInfo)
                            {
                                if (info.StartsWith("Title = "))
                                {
                                    var title = info.Substring("Title = ".Length);
                                    content = content.Replace("{Title}", title);
                                    content = content.Replace("{title}", title.ToLower());
                                    content = content.Replace("{TITLE}", title.ToUpper());
                                }
                                if (info.StartsWith("Description = "))
                                {
                                    var description = info.Substring("Description = ".Length);
                                    content = content.Replace("{Description}", description);
                                    content = content.Replace("{description}", description.ToLower());
                                    content = content.Replace("{DESCRIPTION}", description.ToUpper());
                                }
                                if (info.StartsWith("Sunfrog Link = "))
                                {
                                    content = content.Replace("{SunfrogLink}", info.Substring("Sunfrog Link = ".Length));
                                    if (string.IsNullOrEmpty(link))
                                    {
                                        link = info.Substring("Sunfrog Link = ".Length);
                                    }
                                }
                                if (info.StartsWith("Teespring Link = "))
                                {
                                    content = content.Replace("{TeespringLink}", info.Substring("Teespring Link = ".Length));
                                    if (string.IsNullOrEmpty(link))
                                    {
                                        link = info.Substring("Teespring Link = ".Length);
                                    }
                                }
                                if (info.StartsWith("Teechip Link = "))
                                {
                                    content = content.Replace("{TeechipLink}", info.Substring("Teechip Link = ".Length));
                                    if (string.IsNullOrEmpty(link))
                                    {
                                        link = info.Substring("Teechip Link = ".Length);
                                    }
                                }
                                if (info.StartsWith("Viralstyle Link = "))
                                {
                                    content = content.Replace("{ViralstyleLink}", info.Substring("Viralstyle Link = ".Length));
                                    if (string.IsNullOrEmpty(link))
                                    {
                                        link = info.Substring("Viralstyle Link = ".Length);
                                    }
                                }
                                if (info.StartsWith("Teezily Link = "))
                                {
                                    content = content.Replace("{TeezilyLink}", info.Substring("Teezily Link = ".Length));
                                    if (string.IsNullOrEmpty(link))
                                    {
                                        link = info.Substring("Teezily Link = ".Length);
                                    }
                                }
                            }
                            content = content.Replace("{Link}", link);

                            var result = PostFacebook(facebookPage["access_token"].ToObject<string>(), facebookPage["id"].ToObject<string>(), content, outputFile);
                            if (result)
                            {
                                ++success;
                            }

                            File.Delete(outputFile);
                        }
                    }
                    catch (Exception exc)
                    {
                    }
                }

                ++count;
                BackgroundWorkerPostFacebook.ReportProgress(count * 100 / Designs.Count, success);
            }

            e.Result = success;
        }

        private void BackgroundWorkerPostFacebook_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarPostFacebook.Value = e.ProgressPercentage;
        }

        private void BackgroundWorkerPostFacebook_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ButtonPostFacebook.Enabled = true;
            ButtonClose.Enabled = true;
            var success = (int) e.Result;
            MessageBox.Show($"There are {success}/{Designs.Count} designs posted successfully.", "Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LinkLabelAccessToken_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sInfo = new ProcessStartInfo("https://developers.facebook.com/tools/explorer/");
            Process.Start(sInfo);
        }

        private void TextBoxAccessToken_TextChanged(object sender, EventArgs e)
        {
            LoadFacebookPages();
        }

        private void LoadFacebookPages()
        {
            ComboBoxFacebookPages.Items.Clear();
            facebookPages = new List<JToken>();
            try
            {
                var result =
                    Common
                        .GetJson<JToken
                        >($"https://graph.facebook.com/v2.8/me/accounts?access_token={TextBoxAccessToken.Text}&debug=all&format=json&method=get&pretty=0&suppress_http_code=1&limit=1000",
                          new System.Net.CookieContainer());
                foreach (var data in result["data"])
                {
                    facebookPages.Add(data);
                    ComboBoxFacebookPages.Items.Add(data["name"].ToObject<string>());
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Access Token is not valid or expired. Please generate new Access Token.", "Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool PostFacebook(string accessToken, string pageId, string content, string photo)
        {
            try
            {
                var input = new FileStream(photo, FileMode.Open, FileAccess.Read);
                var result = Common.PostTextAsMultipartForm($"https://graph.facebook.com/{pageId}/photos",
                                                            new List<KeyValuePair<string, object>>
                                                            {
                                                                new KeyValuePair<string, object>("file=" + Path.GetFileName(photo), input),
                                                                new KeyValuePair<string, object>("access_token", accessToken),
                                                                new KeyValuePair<string, object>("caption", content)
                                                            },
                                                            new System.Net.CookieContainer());

                input.Close();

                var jsonResult = JsonConvert.DeserializeObject<JToken>(result);
                return jsonResult["id"].ToObject<string>() != null;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        private void FormPostFacebook_Load(object sender, EventArgs e)
        {
            // Restore saved values
            var settings = new Settings();
            if (File.Exists("settings.config"))
            {
                var fileStream = new FileStream("settings.config", FileMode.Open, FileAccess.Read, FileShare.None);
                settings.Load(fileStream);
                fileStream.Close();
            }

            TextBoxAccessToken.Text = settings.FacebookAccessToken;
            TextBoxContent.Text = settings.FacebookPostContent;
        }

        private void FormPostFacebook_FormClosed(object sender, FormClosedEventArgs e)
        {
            var settings = new Settings();
            if (File.Exists("settings.config"))
            {
                var fileStream = new FileStream("settings.config", FileMode.Open, FileAccess.Read, FileShare.None);
                settings.Load(fileStream);
                fileStream.Close();
            }

            settings.FacebookAccessToken = TextBoxAccessToken.Text;
            settings.FacebookPostContent = TextBoxContent.Text;

            try
            {
                var fileStream = new FileStream("settings.config", FileMode.Create, FileAccess.Write, FileShare.None);
                settings.Save(fileStream);
                fileStream.Close();
            }
            catch (Exception exc)
            {
            }
        }
    }
}
