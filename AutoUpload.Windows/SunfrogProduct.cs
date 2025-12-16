using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using AutoUpload.Shared;
using Newtonsoft.Json.Linq;

namespace AutoUpload.Windows
{
    public partial class SunfrogProduct : ProductControl<int>
    {
        private Image _shirtTexture;

        public JToken Product { get; set; }

        public SunfrogProduct()
        {
            InitializeComponent();
        }

        public void SetProduct(JToken product)
        {
            Product = product;

            GroupBoxProduct.Text = product["description"].ToObject<string>();
            if (!Directory.Exists("Platform/Sunfrog/Colors/"))
            {
                Directory.CreateDirectory("Platform/Sunfrog/Colors/");
            }
            foreach (var color in product["colors"].ToObject<JArray>())
            {
                var colorButton = new Button
                                  {
                                      Size = new Size(32, 32),
                                      BackColor = ColorTranslator.FromHtml(color["value"].ToObject<string>()),
                                      BackgroundImageLayout = ImageLayout.Stretch,
                                      Tag = color["id"].ToObject<int>()
                                  };
                if (color["texture"] != null && !string.IsNullOrEmpty(color["texture"].ToObject<string>()))
                {
                    // Texture file
                    var name = Path.GetFileName(color["texture"].ToObject<string>());
                    if (!File.Exists("Platform/Sunfrog/Colors/" + name))
                    {
                        var outputStream = new FileStream("Platform/Sunfrog/Colors/" + name, FileMode.OpenOrCreate, FileAccess.Write);
                        var downloadResult = Common.DownloadFile(Sunfrog.GetImageLink(color["texture"].ToObject<string>()), outputStream);
                        outputStream.Close();
                        if (downloadResult == false)
                        {
                            File.Delete("Platform/Sunfrog/Colors/" + name);
                        }
                    }

                    try
                    {
                        colorButton.BackgroundImage = Image.FromFile("Platform/Sunfrog/Colors/" + name);
                    }
                    catch (Exception exc)
                    {
                    }
                }
                FlowLayoutProductColors.Controls.Add(colorButton);

                TooltipProduct.SetToolTip(colorButton, TooltipProduct.GetToolTip(FlowLayoutProductColors));

                colorButton.Click += ButtonColorClicked;
            }

            if (!Directory.Exists("Platform/Sunfrog/Shirts/"))
            {
                Directory.CreateDirectory("Platform/Sunfrog/Shirts/");
            }
            var productName = Path.GetFileName(product["imageFront"].ToObject<string>());
            if (!File.Exists("Platform/Sunfrog/Shirts/" + productName))
            {
                var outputStream = new FileStream("Platform/Sunfrog/Shirts/" + productName, FileMode.OpenOrCreate, FileAccess.Write);
                var downloadResult = Common.DownloadFile(Sunfrog.GetImageLink(product["imageFront"].ToObject<string>()), outputStream);
                outputStream.Close();
                if (downloadResult == false)
                {
                    File.Delete("Platform/Sunfrog/Shirts/" + productName);
                }
            }
            try
            {
                BaseImage = Image.FromFile("Platform/Sunfrog/Shirts/" + productName);
            }
            catch (Exception ex)
            {
                BaseImage = null;
            }
            productName = Path.GetFileName(product["imageBack"].ToObject<string>());
            if (productName != "" && !File.Exists("Platform/Sunfrog/Shirts/" + productName))
            {
                var outputStream = new FileStream("Platform/Sunfrog/Shirts/" + productName, FileMode.OpenOrCreate, FileAccess.Write);
                var downloadResult = Common.DownloadFile(Sunfrog.GetImageLink(product["imageBack"].ToObject<string>()), outputStream);
                outputStream.Close();
                if (downloadResult == false)
                {
                    File.Delete("Platform/Sunfrog/Shirts/" + productName);
                }
            }
            try
            {
                BaseBackImage = Image.FromFile("Platform/Sunfrog/Shirts/" + productName);
            }
            catch (Exception ex)
            {
                BaseBackImage = null;
            }
            ReloadSample();

            NumericUpDownPrice.Minimum = product["basePrice"].ToObject<decimal>();
            NumericUpDownPrice.Maximum = product["basePrice"].ToObject<decimal>() < 23 ? 29 : 45;
        }

        private void ButtonColorClicked(object sender, EventArgs e)
        {
            var button = (Button) sender;

            if (button.Image == null && SelectedColors.Count >= 5)
            {
                MessageBox.Show(
                                @"You cannot select more than 5 colors for each style.",
                                @"Auto Upload",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            button.Image = button.Image == null ? Properties.Resources.Checked : null;
            if (button.Image != null)
            {
                _shirtColor = button.BackColor;
                _shirtTexture = button.BackgroundImage;

                SelectedColors.Add((int) button.Tag);
            }
            else
            {
                SelectedColors.Remove((int) button.Tag);
            }

            ReloadSample();

            OnSelectedColorsChanged(e);
        }

        public void ReloadSample(bool reloadImage = true)
        {
            if (Product == null || BaseImage == null)
            {
                return;
            }

            if (reloadImage)
            {
                var sampleScale = Math.Min((float) PictureBoxProductSample.Width / BaseImage.Width, (float) PictureBoxProductSample.Height / BaseImage.Height);
                var sample = new Bitmap((int) (sampleScale * BaseImage.Width), (int) (sampleScale * BaseImage.Height));
                var graphics = Graphics.FromImage(sample);

                // Draw color if any
                graphics.FillRectangle(new SolidBrush(_shirtColor), 0, 0, sample.Width, sample.Height);

                // Draw texture if any
                if (_shirtTexture != null)
                {
                    graphics.FillRectangle(
                                           new TextureBrush(_shirtTexture, WrapMode.Tile),
                                           0,
                                           0,
                                           sample.Width,
                                           sample.Height);
                }

                // Draw shirt
                if (_isFront && BaseImage != null)
                {
                    graphics.DrawImage(BaseImage, 0, 0, sample.Width, sample.Height);
                }
                else if (!_isFront && BaseBackImage != null)
                {
                    graphics.DrawImage(BaseBackImage, 0, 0, sample.Width, sample.Height);
                }

                // Draw art region
                if (Product?["printable"] != null)
                {
                    var width = _isFront ? Product["printable"]["svgFrontWidth"].ToObject<int>() : (int) Product["printable"]["svgBackWidth"].ToObject<int>();
                    var height = _isFront
                                     ? Product["printable"]["svgFrontHeight"].ToObject<int>()
                                     : Product["printable"]["svgBackHeight"].ToObject<int>();
                    width = (int) (width * sampleScale);
                    height = (int) (height * sampleScale);
                    var y = _isFront ? Product["printable"]["yOffsetFront"].ToObject<int>() : Product["printable"]["yOffsetBack"].ToObject<int>();
                    var x = (sample.Width - width) / 2;
                    y = (int)(y * sampleScale);
                    graphics.DrawRectangle(new Pen(Color.FromArgb(180, 100, 100, 100), 2), x, y, width, height);

                    // Draw art
                    if (_isFront && ArtImage != null)
                    {
                        var scale = Product["printable"]["scaleFront"].ToObject<double>() * sampleScale;
                        if (ArtImage.Width > Product["printable"]["width"].ToObject<int>() || ArtImage.Height > Product["printable"]["height"].ToObject<int>())
                        {
                            var xScale = width / (double) ArtImage.Width;
                            var yScale = height / (double) ArtImage.Height;
                            scale = Math.Min(xScale, yScale);
                        }
                        var rWidth = (float) (scale * ArtImage.Width);
                        var rHeight = (float) (scale * ArtImage.Height);
                        if (FrontLeftAlign.Equals("center", StringComparison.OrdinalIgnoreCase))
                        {
                            x += (int) ((float) width / 2 - rWidth / 2);
                        }
                        else if (FrontLeftAlign.Equals("right", StringComparison.OrdinalIgnoreCase))
                        {
                            x += (int) (width - rWidth);
                        }
                        if (FrontTopAlign.Equals("middle", StringComparison.OrdinalIgnoreCase))
                        {
                            y += (int) ((float) height / 2 - rHeight / 2);
                        }
                        else if (FrontTopAlign.Equals("bottom", StringComparison.OrdinalIgnoreCase))
                        {
                            y += (int) (height - rHeight);
                        }
                        graphics.DrawImage(ArtImage, new RectangleF(x, y, rWidth, rHeight));
                    }
                    else if (!_isFront && BackArtImage != null)
                    {
                        var scale = Product["printable"]["scaleBack"].ToObject<double>() * sampleScale;
                        if (BackArtImage.Width > Product["printable"]["width"].ToObject<int>() || BackArtImage.Height > Product["printable"]["height"].ToObject<int>())
                        {
                            var xScale = width / (double) BackArtImage.Width;
                            var yScale = height / (double) BackArtImage.Height;
                            scale = Math.Min(xScale, yScale);
                        }
                        var rWidth = (float) (scale * BackArtImage.Width);
                        var rHeight = (float) (scale * BackArtImage.Height);
                        if (BackLeftAlign.Equals("center", StringComparison.OrdinalIgnoreCase))
                        {
                            x += (int) ((float) width / 2 - rWidth / 2);
                        }
                        else if (BackLeftAlign.Equals("right", StringComparison.OrdinalIgnoreCase))
                        {
                            x += (int) (width - rWidth);
                        }
                        if (BackTopAlign.Equals("middle", StringComparison.OrdinalIgnoreCase))
                        {
                            y += (int) ((float) height / 2 - rHeight / 2);
                        }
                        else if (BackTopAlign.Equals("bottom", StringComparison.OrdinalIgnoreCase))
                        {
                            y += (int) (height - rHeight);
                        }
                        graphics.DrawImage(BackArtImage, new RectangleF(x, y, rWidth, rHeight));
                    }
                }

                graphics.Dispose();
                PictureBoxProductSample.Image = sample;
            }

            if (ArtImage != null && BackArtImage != null)
            {
                NumericUpDownPrice.Minimum = Product["basePrice"].ToObject<decimal>() + Product["backPrintPrice"].ToObject<decimal>();
                NumericUpDownPrice.Maximum = (Product["basePrice"].ToObject<decimal>() < 23 ? 29 : 45) + Product["backPrintPrice"].ToObject<decimal>();
            }
            else
            {
                NumericUpDownPrice.Minimum = Product["basePrice"].ToObject<decimal>();
                NumericUpDownPrice.Maximum = Product["basePrice"].ToObject<decimal>() < 23 ? 29 : 45;
                if (NumericUpDownPrice.Value > NumericUpDownPrice.Maximum)
                {
                    NumericUpDownPrice.Value = NumericUpDownPrice.Maximum;
                }
            }
        }

        private void ButtonClearSelectedColors_Click(object sender, EventArgs e)
        {
            foreach (var control in FlowLayoutProductColors.Controls)
            {
                var buttonColor = control as Button;
                if (buttonColor != null)
                {
                    buttonColor.Image = null;
                }
            }

            SelectedColors.Clear();
            _shirtColor = Color.Transparent;
            _shirtTexture = null;
            ReloadSample();

            OnSelectedColorsChanged(e);
        }

        private void ButtonChangeSide_Click(object sender, EventArgs e)
        {
            _isFront = !_isFront;
            ReloadSample();
        }

        public void SetSelectedColors(int[] colors)
        {
            foreach (var control in FlowLayoutProductColors.Controls)
            {
                var buttonColor = control as Button;
                if (buttonColor != null)
                {
                    buttonColor.Image = null;
                }
            }

            SelectedColors.Clear();
            _shirtColor = Color.Transparent;
            _shirtTexture = null;

            foreach (var color in colors)
            {
                foreach (var control in FlowLayoutProductColors.Controls)
                {
                    if (control is Button && (int) ((Button) control).Tag == color)
                    {
                        ((Button) control).Image = Properties.Resources.Checked;
                        _shirtColor = ((Button) control).BackColor;
                        _shirtTexture = ((Button) control).BackgroundImage;

                        SelectedColors.Add((int) ((Button) control).Tag);
                    }
                }
            }

            OnSelectedColorsChanged(new EventArgs());
        }

        private void CheckBoxIsDefault_CheckedChanged(object sender, EventArgs e)
        {
            OnIsDefaultChanged(e);
        }
    }
}
