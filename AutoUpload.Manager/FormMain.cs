using AutoUpload.Photoshop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoUpload.Shared.Models;

namespace AutoUpload.Manager
{
    public partial class FormMain : Form
    {
        private List<FileInfo> mockupFiles;

        public FormMain()
        {
            InitializeComponent();
        }

        private void ButtonChecksum_Click(object sender, EventArgs e)
        {
            if (OpenFileDialogChecksum.ShowDialog() == DialogResult.OK)
            {
                TextBoxChecksum.Text = GetFileChecksum(OpenFileDialogChecksum.FileName);
            }
        }

        private static string GetFileChecksum(string filePath)
        {
            try
            {
                var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var buffer = new byte[file.Length];
                file.Read(buffer, 0, buffer.Length);
                using (var md5 = MD5.Create())
                {
                    var md5Hash = md5.ComputeHash(buffer, 0, buffer.Length);
                    return GetHexString(md5Hash);
                }
            }
            catch (Exception exc)
            {
                return "";
            }
        }

        private static string GetHexString(byte[] bt)
        {
            var s = string.Empty;
            for (var i = 0; i < bt.Length; i++)
            {
                byte b = bt[i];
                int n, n1, n2;
                n = (int) b;
                n1 = n & 15;
                n2 = (n >> 4) & 15;
                if (n2 > 9)
                    s += ((char) (n2 - 10 + (int) 'A')).ToString();
                else
                    s += n2.ToString();
                if (n1 > 9)
                    s += ((char) (n1 - 10 + (int) 'A')).ToString();
                else
                    s += n1.ToString();
                if ((i + 1) != bt.Length && (i + 1) % 2 == 0) s += "-";
            }

            return s;
        }

        private void ButtonSelectPsdSource_Click(object sender, EventArgs e)
        {
            if (OpenFileDialogPhotoshop.ShowDialog() == DialogResult.OK)
            {
                TextBoxPsdSource.Text = OpenFileDialogPhotoshop.FileName;
            }
        }

        private void ButtonLoadPsd_Click(object sender, EventArgs e)
        {
            if (TextBoxPsdSource.Text.Trim().Length == 0)
            {
                MessageBox.Show("You must choose a file");
                return;
            }

            try
            {
                var layers = PhotoshopHelper.GetAllLayers(TextBoxPsdSource.Text);
                RichTextBoxPsdLayers.Clear();
                for (int i = 0; i < layers.Length; i++)
                {
                    RichTextBoxPsdLayers.SelectionBullet = true;
                    RichTextBoxPsdLayers.AppendText($@"- Layer index: ");
                    RichTextBoxPsdLayers.SelectionFont = new Font(RichTextBoxPsdLayers.SelectionFont, FontStyle.Bold);
                    RichTextBoxPsdLayers.AppendText($@"{i}");
                    RichTextBoxPsdLayers.SelectionFont = new Font(RichTextBoxPsdLayers.SelectionFont, FontStyle.Regular);
                    RichTextBoxPsdLayers.AppendText($@": ");
                    RichTextBoxPsdLayers.SelectionFont = new Font(RichTextBoxPsdLayers.SelectionFont, FontStyle.Bold);
                    RichTextBoxPsdLayers.AppendText($@"{layers[i].Name}");
                    RichTextBoxPsdLayers.SelectionFont = new Font(RichTextBoxPsdLayers.SelectionFont, FontStyle.Regular);
                    RichTextBoxPsdLayers.SelectionBullet = false;
                    RichTextBoxPsdLayers.AppendText("\n");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void ButtonCreateMockup_Click(object sender, EventArgs e)
        {
            if (TextBoxPsdSource.Text.Trim().Length == 0)
            {
                MessageBox.Show("You must choose a file");
                return;
            }

            ProgressBarCreateMockup.Value = 0;
            ButtonCreateMockup.Enabled = false;
            BackgroundWorkerCreateMockup.RunWorkerAsync();
        }

        private void BackgroundWorkerCreateMockup_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var fileInfo = new FileInfo(TextBoxPsdSource.Text);
                if (fileInfo.Extension.ToLower() != ".psd")
                {
                    BackgroundWorkerCreateMockup.ReportProgress(100, @"File is not PSD file.");
                    return;
                }

                var outputFile = Path.Combine(fileInfo.DirectoryName, string.Format("{0}.tips", fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length)));
                var result = PhotoshopHelper.Encrypt(TextBoxPsdSource.Text, outputFile, (int)NumericUpDownDesignLayer.Value,
                    (int)NumericUpDownShirtColorLayer.Value, (int)NumericUpDownShirtTextureLayer.Value, (int)NumericUpDownLaceColorLayer.Value, 
                    new Rectangle((int)NumericUpDownDesignX.Value, (int)NumericUpDownDesignY.Value, (int)NumericUpDownDesignWidth.Value, (int)NumericUpDownDesignHeight.Value));
                
                BackgroundWorkerCreateMockup.ReportProgress(100);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void BackgroundWorkerCreateMockup_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBarCreateMockup.Value = 100;
            ButtonCreateMockup.Enabled = true;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            pictureBox1.Image =
                PhotoshopHelper.Convert3DToImage(@"D:\OneDrive\Projects\AutoUpload\AutoUpload.Windows\bin\Debug\Platform\Teespring\Shirts\token-iphone-case.obj",
                                                 @"D:\OneDrive\Projects\AutoUpload\AutoUpload.Windows\bin\Debug\Platform\Teespring\Shirts\token-iphone-case.png",
                                                 514, 560,
                                                 new Point3D()
                                                 {
                                                     X = -6.366812898336933e-8,
                                                     Y = 10.416426028079831,
                                                     Z = 0.000010416694459028774
                                                 },
                                                 new Point3D()
                                                 {
                                                     X = -1.5707953377851194,
                                                     Y = -6.1129568074136387E-09,
                                                     Z = -0.0061120476712239356
                                                 },
                                                 new List<Light>()
                                                 {
                                                     new Light()
                                                     {
                                                         Color = 16777215,
                                                         BackgroundColor = 16777215,
                                                         Intensity = 0.7215555217985723F,
                                                         Type = "hemisphere"
                                                     },
                                                     new Light()
                                                     {
                                                         Color = 16777215,
                                                         Intensity = 0.5F,
                                                         Type = "ambient"
                                                     },
                                                     //new Light()
                                                     //{
                                                     //    Color = 16777215,
                                                     //    Type = "point",
                                                     //    Intensity = 0.5F,
                                                     //    Position = new Point3D()
                                                     //               {
                                                     //                   X = -10.645536587230062,
                                                     //                   Y = -12,
                                                     //                   Z = 98.6236070740465
                                                     //               }
                                                     //},
                                                     //new Light()
                                                     //{
                                                     //    Color = 16777215,
                                                     //    Type = "directional",
                                                     //    Intensity = 0.5F,
                                                     //    Position = new Point3D()
                                                     //               {
                                                     //                   X = 0.6,
                                                     //                   Y = 10,
                                                     //                   Z = 1
                                                     //               }
                                                     //}
                                                 },
                                                 Bitmap.FromFile(@"D:\OneDrive\Projects\AutoUpload\Publish\003.png") as Bitmap,
                                                 Color.FromArgb(12, 79, 50),
                                                 417, 708,
                                                 287, 580, 65, 64,
                                                 430, 871);
        }

        private void BackgroundWorkerCreateMockup_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarCreateMockup.Value = e.ProgressPercentage;
            if (e.UserState != null && e.UserState.ToString() != "")
            {
                MessageBox.Show(e.UserState.ToString());
            }
        }
        
        private void ButtonTestMockups_Click(object sender, EventArgs e)
        {
            if (OpenFileDialogPhotoshop.ShowDialog() == DialogResult.OK)
            {
                var fileInfo = new FileInfo(TextBoxPsdSource.Text);
                if (fileInfo.Extension.ToLower() != ".psd")
                {
                    MessageBox.Show(@"File is not PSD file.");
                    return;
                }

                var tipsFile = Path.Combine(fileInfo.DirectoryName, string.Format("{0}.tips", fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length)));
                if (!File.Exists(tipsFile))
                {
                    MessageBox.Show(@"Cannot find TIPS file.");
                    return;
                }

                ProgressBarCreateMockup.Value = 0;
                RichTextBoxPsdLayers.Text = "";
                mockupFiles = new List<FileInfo>();
                mockupFiles.Add(fileInfo);

                ButtonTestMockups.Enabled = false;
                BackgroundWorkerTestMockups.RunWorkerAsync();
            }
        }

        private void ButtonShirtColor_Click(object sender, EventArgs e)
        {
            if (ColorDialogDesign.ShowDialog() == DialogResult.OK)
            {
                ButtonShirtColor.BackColor = ColorDialogDesign.Color;
            }
        }

        private void ButtonLaceColor_Click(object sender, EventArgs e)
        {
            if (ColorDialogDesign.ShowDialog() == DialogResult.OK)
            {
                ButtonLaceColor.BackColor = ColorDialogDesign.Color;
            }
        }

        private void ButtonDesignBackground_Click(object sender, EventArgs e)
        {
            if (ColorDialogDesign.ShowDialog() == DialogResult.OK)
            {
                ButtonDesignBackground.BackColor = ColorDialogDesign.Color;
            }
        }

        private void ButtonLoadTipsHeader_Click(object sender, EventArgs e)
        {
            try
            {
                var fileInfo = new FileInfo(TextBoxPsdSource.Text);
                if (fileInfo.Extension.ToLower() != ".psd")
                {
                    MessageBox.Show(@"File is not PSD file.");
                    return;
                }

                var outputFile = Path.Combine(fileInfo.DirectoryName, string.Format("{0}.tips", fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length)));
                using (var stream = new MemoryStream())
                {
                    EncryptedPhotoshopHeader header;
                    var error = PhotoshopHelper.Decrypt(outputFile, stream, out header);
                    if (string.IsNullOrEmpty(error))
                    {
                        NumericUpDownDesignLayer.Value = header.DesignLayer;
                        NumericUpDownShirtColorLayer.Value = header.ShirtColorLayer;
                        NumericUpDownShirtTextureLayer.Value = header.ShirtTextureLayer;
                        NumericUpDownLaceColorLayer.Value = header.LaceColorLayer;
                        NumericUpDownDesignX.Value = header.DesignBound.X;
                        NumericUpDownDesignY.Value = header.DesignBound.Y;
                        NumericUpDownDesignWidth.Value = header.DesignBound.Width;
                        NumericUpDownDesignHeight.Value = header.DesignBound.Height;
                    }
                    else
                    {
                        MessageBox.Show(error);
                    }
                    stream.Close();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void ButtonAesKeys_Click(object sender, EventArgs e)
        {
            using (var cipher = Aes.Create())
            {
                cipher.GenerateIV();
                cipher.GenerateKey();

                TextBoxAesKey.Text = Convert.ToBase64String(cipher.Key);
                TextBoxAesInitialVector.Text = Convert.ToBase64String(cipher.IV);
            }
        }

        private void ButtonCreateThumbnail_Click(object sender, EventArgs e)
        {
            if (OpenFileDialogPhotoshop.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var fileInfo = new FileInfo(TextBoxPsdSource.Text);
                    if (fileInfo.Extension.ToLower() != ".psd")
                    {
                        MessageBox.Show(@"File is not PSD file.");
                        return;
                    }

                    var tipsFile = Path.Combine(fileInfo.DirectoryName, string.Format("{0}.tips", fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length)));
                    if (!File.Exists(tipsFile))
                    {
                        MessageBox.Show(@"Cannot find TIPS file.");
                        return;
                    }

                    ButtonCreateThumbnail.Enabled = false;
                    var outputFile = Path.Combine(fileInfo.DirectoryName, string.Format("{0}.jpg", fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length)));

                    PhotoshopHelper.CreateMockup(tipsFile, OpenFileDialogPhotoshop.FileName, outputFile,
                        ButtonDesignBackground.BackColor, ButtonShirtColor.BackColor, null, ButtonLaceColor.BackColor, 512, 512);
                    ButtonCreateThumbnail.Enabled = true;

                    MessageBox.Show(@"Done");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    ButtonCreateThumbnail.Enabled = true;
                }
            }
        }

        private void ButtonFillDesign_Click(object sender, EventArgs e)
        {
            if (OpenFileDialogPhotoshop.ShowDialog() == DialogResult.OK)
            {
                var fileInfo = new FileInfo(TextBoxPsdSource.Text);
                if (fileInfo.Extension.ToLower() != ".psd")
                {
                    MessageBox.Show(@"File is not PSD file.");
                    return;
                }

                var tipsFile = Path.Combine(fileInfo.DirectoryName, string.Format("{0}.tips", fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length)));
                if (!File.Exists(tipsFile))
                {
                    MessageBox.Show(@"Cannot find TIPS file.");
                    return;
                }

                ProgressBarCreateMockup.Value = 0;
                RichTextBoxPsdLayers.Text = "";
                mockupFiles = new List<FileInfo>();
                mockupFiles.Add(fileInfo);

                ButtonFillDesign.Enabled = false;
                BackgroundWorkerTestMockups.RunWorkerAsync("Fill");
            }
        }

        private void ButtonTestAll_Click(object sender, EventArgs e)
        {
            if (OpenFileDialogPhotoshop.ShowDialog() == DialogResult.OK)
            {
                var fileInfo = new FileInfo(TextBoxPsdSource.Text);
                mockupFiles = new List<FileInfo>(fileInfo.Directory.GetFiles("*.psd"));
                ProgressBarCreateMockup.Value = 0;
                RichTextBoxPsdLayers.Text = "";
                ButtonTestAll.Enabled = false;
                BackgroundWorkerTestMockups.RunWorkerAsync();
            }
        }

        private void ButtonTestFillAll_Click(object sender, EventArgs e)
        {
            if (OpenFileDialogPhotoshop.ShowDialog() == DialogResult.OK)
            {
                var fileInfo = new FileInfo(TextBoxPsdSource.Text);
                mockupFiles = new List<FileInfo>(fileInfo.Directory.GetFiles("*.psd"));
                ProgressBarCreateMockup.Value = 0;
                RichTextBoxPsdLayers.Text = "";
                ButtonTestFillAll.Enabled = false;
                BackgroundWorkerTestMockups.RunWorkerAsync("Fill");
            }
        }

        private void BackgroundWorkerTestMockups_DoWork(object sender, DoWorkEventArgs e)
        {
            var count = 0;
            foreach (var fileInfo in mockupFiles)
            {
                try
                {
                    var outputFile = Path.Combine(fileInfo.DirectoryName, $"{fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length)}-design.png");
                    var tipsFile = Path.Combine(fileInfo.DirectoryName, $"{fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length)}.tips");
                    if (!File.Exists(tipsFile))
                    {
                        ++count;
                        BackgroundWorkerTestMockups.ReportProgress(count * 100 / mockupFiles.Count, $"No tips file: {tipsFile}");
                        continue;
                    }

                    PhotoshopHelper.CreateMockup(tipsFile, OpenFileDialogPhotoshop.FileName, outputFile, ButtonDesignBackground.BackColor, ButtonShirtColor.BackColor,
                        null, ButtonLaceColor.BackColor, 0, 0, 
                        e.Argument?.ToString()?.ToLower() == "fill" ? "Stretch" : "Center", e.Argument?.ToString()?.ToLower() == "fill" ? "Stretch" : "Middle");

                    ++count;
                    BackgroundWorkerTestMockups.ReportProgress(count * 100 / mockupFiles.Count);
                }
                catch (Exception exc)
                {
                    ++count;
                    BackgroundWorkerTestMockups.ReportProgress(count * 100 / mockupFiles.Count, exc.Message);
                }
            }
        }

        private void BackgroundWorkerTestMockups_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarCreateMockup.Value = e.ProgressPercentage;
            if (e.UserState != null && e.UserState.ToString() != "")
            {
                RichTextBoxPsdLayers.Text += e.UserState + "\n";
            }
        }

        private void BackgroundWorkerTestMockups_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ButtonTestMockups.Enabled = true;
            ButtonFillDesign.Enabled = true;
            ButtonTestAll.Enabled = true;
            ButtonTestFillAll.Enabled = true;
        }
    }
}
