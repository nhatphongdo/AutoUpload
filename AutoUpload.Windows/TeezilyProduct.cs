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
    public partial class TeezilyProduct : ProductControl<string>
    {
        private Image _shirtTexture;

        public JToken Product { get; set; }

        public TeezilyProduct()
        {
            InitializeComponent();
        }

        public void SetProduct(JToken product)
        {
            Product = product;

            GroupBoxProduct.Text = product["name"].ToObject<string>();
            if (!Directory.Exists("Platform/Teezily/Colors/"))
            {
                Directory.CreateDirectory("Platform/Teezily/Colors/");
            }
            foreach (var color in product["colors"].ToObject<JArray>())
            {
                var colorButton = new Button
                                  {
                                      Size = new Size(32, 32),
                                      BackColor = ColorTranslator.FromHtml("#" + color["hex_code"].ToObject<string>()),
                                      BackgroundImageLayout = ImageLayout.Stretch,
                                      Tag = color["name"].ToObject<string>()
                                  };
                if (color["texture"] != null && !string.IsNullOrEmpty(color["texture"].ToObject<string>()))
                {
                    // Texture file
                    var name = Path.GetFileName(color["texture"].ToObject<string>());
                    if (!File.Exists("Platform/Teezily/Colors/" + name))
                    {
                        var outputStream = new FileStream("Platform/Teezily/Colors/" + name, FileMode.OpenOrCreate, FileAccess.Write);
                        var downloadResult = Common.DownloadFile(Teezily.GetImageLink(color["texture"].ToObject<string>()), outputStream);
                        outputStream.Close();
                        if (downloadResult == false)
                        {
                            File.Delete("Platform/Teezily/Colors/" + name);
                        }
                    }

                    try
                    {
                        colorButton.BackgroundImage = Image.FromFile("Platform/Teezily/Colors/" + name);
                    }
                    catch (Exception exc)
                    {
                    }
                }
                FlowLayoutProductColors.Controls.Add(colorButton);

                TooltipProduct.SetToolTip(colorButton, TooltipProduct.GetToolTip(FlowLayoutProductColors));

                colorButton.Click += ButtonColorClicked;
            }

            if (!Directory.Exists("Platform/Teezily/Shirts/"))
            {
                Directory.CreateDirectory("Platform/Teezily/Shirts/");
            }
            foreach (var side in product["sides"].ToObject<JArray>())
            {
                var productName = Path.GetFileName(side["normal_image"].ToObject<string>());
                productName = productName.IndexOf('?') == -1 ? productName : productName.Substring(0, productName.IndexOf('?'));
                if (!File.Exists("Platform/Teezily/Shirts/" + productName))
                {
                    var outputStream = new FileStream("Platform/Teezily/Shirts/" + productName, FileMode.OpenOrCreate, FileAccess.Write);
                    var downloadResult = Common.DownloadFile(side["normal_image"].ToObject<string>(), outputStream);
                    outputStream.Close();
                    if (downloadResult == false)
                    {
                        File.Delete("Platform/Teezily/Shirts/" + productName);
                    }
                }

                foreach (var sideColor in side["side_color_images"].ToObject<JArray>())
                {
                    var colorName = Path.GetFileName(sideColor["normal_image"].ToObject<string>());
                    colorName = colorName.IndexOf('?') == -1 ? colorName : colorName.Substring(0, colorName.IndexOf('?'));
                    if (!File.Exists("Platform/Teezily/Shirts/" + colorName))
                    {
                        var outputStream = new FileStream("Platform/Teezily/Shirts/" + colorName, FileMode.OpenOrCreate, FileAccess.Write);
                        var downloadResult = Common.DownloadFile(sideColor["normal_image"].ToObject<string>(), outputStream);
                        outputStream.Close();
                        if (downloadResult == false)
                        {
                            File.Delete("Platform/Teezily/Shirts/" + colorName);
                        }
                    }
                }

                try
                {
                    if (side["name"].ToObject<string>().ToLower() == "back")
                    {
                        BaseBackImage = Image.FromFile("Platform/Teezily/Shirts/" + productName);
                    }
                    else
                    {
                        BaseImage = Image.FromFile("Platform/Teezily/Shirts/" + productName);
                    }
                }
                catch (Exception ex)
                {
                    if (side["name"].ToObject<string>().ToLower() == "back")
                    {
                        BaseBackImage = null;
                    }
                    else
                    {
                        BaseImage = null;
                    }
                }
            }
            ReloadSample();
        }

        private void ButtonColorClicked(object sender, EventArgs e)
        {
            var button = (Button) sender;

            button.Image = button.Image == null ? Properties.Resources.Checked : null;
            if (button.Image != null)
            {
                _shirtColor = button.BackColor;
                _shirtTexture = button.BackgroundImage;

                SelectedColors.Add((string) button.Tag);
            }
            else
            {
                SelectedColors.Remove((string) button.Tag);
            }

            // Load new base image corresponding to color
            foreach (var side in Product["sides"].ToObject<JArray>())
            {
                if ((_isFront && side["name"].ToObject<string>().ToLower() == "front")
                    || (!_isFront && side["name"].ToObject<string>().ToLower() == "front"))
                {
                    foreach (var sideColor in side["side_color_images"].ToObject<JArray>())
                    {
                        if (sideColor["color"].ToObject<string>() == (string) button.Tag)
                        {
                            var colorName = Path.GetFileName(sideColor["normal_image"].ToObject<string>());
                            colorName = colorName.IndexOf('?') == -1 ? colorName : colorName.Substring(0, colorName.IndexOf('?'));
                            Image image = null;
                            try
                            {
                                image = Image.FromFile("Platform/Teezily/Shirts/" + colorName);
                            }
                            catch (Exception exc)
                            {
                                image = null;
                            }
                            if (_isFront && image != null)
                            {
                                BaseImage = image;
                            }
                            else if (!_isFront && image != null)
                            {
                                BaseBackImage = image;
                            }
                        }
                    }
                }
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
                JToken side = null;
                foreach (var s in Product["sides"].ToObject<JArray>())
                {
                    if ((_isFront && s["name"].ToObject<string>().ToLower() == "front")
                        || (!_isFront && s["name"].ToObject<string>().ToLower() == "back"))
                    {
                        side = s;
                        break;
                    }
                }
                if (side != null)
                {
                    var width = side["drawable_width_px"].ToObject<int>();
                    var height = side["drawable_height_px"].ToObject<int>();
                    width = (int) (width * sampleScale);
                    height = (int) (height * sampleScale);
                    var y = side["offset_top"].ToObject<int>();
                    var x = side["offset_left"].ToObject<int>();
                    x = (int)(x * sampleScale);
                    y = (int)(y * sampleScale);
                    graphics.DrawRectangle(new Pen(Color.FromArgb(180, 100, 100, 100), 2), x, y, width, height);

                    // Draw art
                    if (_isFront && ArtImage != null)
                    {
                        var scale = sampleScale;
                        if (ArtImage.Width > width || ArtImage.Height > height)
                        {
                            var xScale = width / (float) ArtImage.Width;
                            var yScale = height / (float) ArtImage.Height;
                            scale = Math.Min(xScale, yScale);
                        }
                        var rWidth = (scale * ArtImage.Width);
                        var rHeight = (scale * ArtImage.Height);
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
                        var scale = sampleScale;
                        if (BackArtImage.Width > width || BackArtImage.Height > height)
                        {
                            var xScale = width / (float) BackArtImage.Width;
                            var yScale = height / (float) BackArtImage.Height;
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

        public void SetSelectedColors(string[] colors)
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
                    if (control is Button && (string) ((Button) control).Tag == color)
                    {
                        ((Button) control).Image = Properties.Resources.Checked;
                        _shirtColor = ((Button) control).BackColor;
                        _shirtTexture = ((Button) control).BackgroundImage;

                        SelectedColors.Add((string) ((Button) control).Tag);
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
