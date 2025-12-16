using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoUpload.Shared;
using Newtonsoft.Json.Linq;

namespace AutoUpload.Windows
{
    public partial class FormRegistration : Form
    {
        public static readonly string CheckUpdateUrl = "http://autoupload.teeinsight.com/api/users/checkupdate";

        public FormRegistration()
        {
            InitializeComponent();
        }

        private void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxEmail.Text))
            {
                MessageBox.Show(@"You must input your email used to register license.", @"Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(TextBoxLicenseKey.Text))
            {
                MessageBox.Show(@"You must input your registered license key.", @"Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ButtonSubmit.Enabled = false;

            // Check license
            var error = License.CheckLicense(TextBoxEmail.Text, TextBoxLicenseKey.Text);
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, @"Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ButtonSubmit.Enabled = true;
                return;
            }

            ButtonSubmit.Enabled = true;

            // Save to file
            File.WriteAllText("license.key", $@"{TextBoxEmail.Text}{char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10)}{TextBoxLicenseKey.Text}");

            (new FormMain()).Show();
            Hide();
        }

        private void FormRegistration_Load(object sender, EventArgs e)
        {
            // Check version first to update if needed
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            var result = Common.GetText(CheckUpdateUrl, new CookieContainer());
            if (result != null)
            {
                var parts = result.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    if (!version.Equals(parts[0]))
                    {
                        // Need to update
                        if (MessageBox.Show(
                                            @"There is an update of application. You should update to work with new changes of platforms. Application will be closed automatically if you agree to update. Please wait until update completes. Do you want to update now?",
                                            @"Auto Upload",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
                            var updater = Path.Combine(path, "AutoUpload.Updater.Windows.exe");
                            if (!File.Exists(updater))
                            {
                                updater = Path.Combine(path, "Updater.exe");
                                if (!File.Exists(updater))
                                {
                                    MessageBox.Show($"Cannot find update application. Please download update application from our website and copy to current directory\n\n{Path.GetFullPath(path)}\n\nApplication will be closed.",
                                                    @"Auto Upload",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Error);
                                    Application.Exit();
                                    return;
                                }
                            }

                            Process.Start(updater);
                            Application.Exit();
                        }
                    }
                }
            }
        }

        private void FormRegistration_Shown(object sender, EventArgs e)
        {
            // Load saved data
            try
            {
                // Check DLL files
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
                var dllFiles = new[] { "Magick.NET-Q8-AnyCPU.dll" };
                var missingFiles = dllFiles.Where(file => !File.Exists(Path.Combine(path, file))).ToList();
                if (missingFiles.Count > 0)
                {
                    MessageBox.Show($"Those file(s) are needed to run this program but cannot be found: \n\n{string.Join("\n", missingFiles)}\n\nPlease download them from our website and copy to current directory\n\n{Path.GetFullPath(path)}\n\nApplication will be closed.", 
                        @"Auto Upload",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }

                var content = File.ReadAllText("license.key");
                var parts = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    TextBoxEmail.Text = parts[0];
                    TextBoxLicenseKey.Text = parts[1];
                    ButtonSubmit.PerformClick();
                }
            }
            catch (Exception exc)
            {
            }
        }
    }
}