using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AutoUpload.Shared;
using Newtonsoft.Json.Linq;
using AutoUpload.Shared.Models;

namespace AutoUpload.Windows
{
    public partial class TeechipProduct : ProductControl<string>
    {
        public ProductsInfo ProductsInfo { get; private set; }

        public string ProductId { get; private set; }

        public string ProductType { get; private set; }

        public Image BorderBaseImage { get; private set; }

        public Image LandscapeBaseImage { get; private set; }

        public Image BorderLandscapeBaseImage { get; private set; }

        public string Orientation { get; private set; }

        public bool Border { get; private set; }

        public TeechipProduct()
        {
            InitializeComponent();
            Orientation = "Portrait";
            Border = false;
        }

        public void SetProduct(string productId, ProductsInfo productsInfo)
        {
            ProductId = productId;
            ProductsInfo = productsInfo;
            ProductType = ProductsInfo.GetProductType(ProductId);

            GroupBoxProduct.Text = ProductsInfo.GetProductName(ProductId);
            foreach (var color in ProductsInfo.GetColors(ProductId))
            {
                var colorButton = new Button
                                  {
                                      Size = new Size(32, 32),
                                      BackColor = ColorTranslator.FromHtml("#" + ((JProperty) color).Name),
                                      BackgroundImageLayout = ImageLayout.Stretch,
                                      Tag = ((JProperty) color).Name
                                  };
                TooltipProduct.SetToolTip(colorButton, ((JProperty) color).Value.ToObject<string>());

                FlowLayoutProductColors.Controls.Add(colorButton);

                colorButton.Click += ButtonColorClicked;
            }

            if (!Directory.Exists("Platform/Teechip/Shirts/"))
            {
                Directory.CreateDirectory("Platform/Teechip/Shirts/");
            }
            var productFileName = ProductId + ".png";
            if (!File.Exists("Platform/Teechip/Shirts/" + productFileName))
            {
                var outputStream = new FileStream("Platform/Teechip/Shirts/" + productFileName, FileMode.OpenOrCreate, FileAccess.Write);
                var downloadResult = Common.DownloadFile(Teechip.GetImageLink(productFileName), outputStream);
                outputStream.Close();
                if (downloadResult == false)
                {
                    File.Delete("Platform/Teechip/Shirts/" + productFileName);
                }
            }
            try
            {
                BaseImage = Image.FromFile("Platform/Teechip/Shirts/" + productFileName);
            }
            catch (Exception ex)
            {
                BaseImage = null;
            }
            if (ProductType.Equals("mug", StringComparison.OrdinalIgnoreCase) ||
                ProductType.Equals("case", StringComparison.OrdinalIgnoreCase))
            {
                ButtonChangeSide.Visible = false;
            }
            else if (ProductType.Equals("poster", StringComparison.OrdinalIgnoreCase))
            {
                ButtonChangeSide.Visible = false;
                ButtonChangeOrientation.Visible = true;
                ButtonChangeBorder.Visible = true;
                productFileName = ProductId + "-border.png";
                if (!File.Exists("Platform/Teechip/Shirts/" + productFileName))
                {
                    var outputStream = new FileStream(
                                                      "Platform/Teechip/Shirts/" + productFileName,
                                                      FileMode.OpenOrCreate,
                                                      FileAccess.Write);
                    var downloadResult = Common.DownloadFile(Teechip.GetImageLink(productFileName), outputStream);
                    outputStream.Close();
                    if (downloadResult == false)
                    {
                        File.Delete("Platform/Teechip/Shirts/" + productFileName);
                    }
                }
                try
                {
                    BorderBaseImage = Image.FromFile("Platform/Teechip/Shirts/" + productFileName);
                }
                catch (Exception ex)
                {
                    BorderBaseImage = null;
                }
                productFileName = ProductId + "-landscape.png";
                if (!File.Exists("Platform/Teechip/Shirts/" + productFileName))
                {
                    var outputStream = new FileStream(
                                                      "Platform/Teechip/Shirts/" + productFileName,
                                                      FileMode.OpenOrCreate,
                                                      FileAccess.Write);
                    var downloadResult = Common.DownloadFile(Teechip.GetImageLink(productFileName), outputStream);
                    outputStream.Close();
                    if (downloadResult == false)
                    {
                        File.Delete("Platform/Teechip/Shirts/" + productFileName);
                    }
                }
                try
                {
                    LandscapeBaseImage = Image.FromFile("Platform/Teechip/Shirts/" + productFileName);
                }
                catch (Exception ex)
                {
                    LandscapeBaseImage = null;
                }
                productFileName = ProductId + "-landscape-border.png";
                if (!File.Exists("Platform/Teechip/Shirts/" + productFileName))
                {
                    var outputStream = new FileStream(
                                                      "Platform/Teechip/Shirts/" + productFileName,
                                                      FileMode.OpenOrCreate,
                                                      FileAccess.Write);
                    var downloadResult = Common.DownloadFile(Teechip.GetImageLink(productFileName), outputStream);
                    outputStream.Close();
                    if (downloadResult == false)
                    {
                        File.Delete("Platform/Teechip/Shirts/" + productFileName);
                    }
                }
                try
                {
                    BorderLandscapeBaseImage = Image.FromFile("Platform/Teechip/Shirts/" + productFileName);
                }
                catch (Exception ex)
                {
                    BorderLandscapeBaseImage = null;
                }
            }
            else
            {
                productFileName = ProductId + "-back.png";
                if (!File.Exists("Platform/Teechip/Shirts/" + productFileName))
                {
                    var outputStream = new FileStream(
                                                      "Platform/Teechip/Shirts/" + productFileName,
                                                      FileMode.OpenOrCreate,
                                                      FileAccess.Write);
                    var downloadResult = Common.DownloadFile(Teechip.GetImageLink(productFileName), outputStream);
                    outputStream.Close();
                    if (downloadResult == false)
                    {
                        File.Delete("Platform/Teechip/Shirts/" + productFileName);
                    }
                }
                try
                {
                    BaseBackImage = Image.FromFile("Platform/Teechip/Shirts/" + productFileName);
                }
                catch (Exception ex)
                {
                    BaseBackImage = null;
                }
            }
            ReloadSample();

            NumericUpDownPrice.Minimum = ProductsInfo.GetPrice(ProductId, false);
        }

        private void ButtonColorClicked(object sender, EventArgs e)
        {
            var button = (Button) sender;

            if (button.Image == null && ProductType.Equals("garment", StringComparison.OrdinalIgnoreCase) && SelectedColors.Count >= 15)
            {
                MessageBox.Show(
                                @"You cannot select more than 15 color and garment combinations.",
                                @"Auto Upload",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            button.Image = button.Image == null ? Properties.Resources.Checked : null;
            if (button.Image != null)
            {
                _shirtColor = button.BackColor;
                SelectedColors.Add((string) button.Tag);
            }
            else
            {
                SelectedColors.Remove((string) button.Tag);
            }

            ReloadSample();

            OnSelectedColorsChanged(e);
        }

        public void ReloadSample(bool reloadImage = true)
        {
            if (reloadImage)
            {
                var baseWidth = BaseImage?.Width ?? 0;
                var baseHeight = BaseImage?.Height ?? 0;
                if (ProductType.Equals("poster", StringComparison.OrdinalIgnoreCase) && Orientation == "Landscape" &&
                    LandscapeBaseImage != null)
                {
                    baseWidth = LandscapeBaseImage.Width;
                    baseHeight = LandscapeBaseImage.Height;
                }
                if (baseWidth == 0 || baseHeight == 0)
                {
                    return;
                }
                var sampleScale = Math.Min((float) PictureBoxProductSample.Width / baseWidth, (float) PictureBoxProductSample.Height / baseHeight);
                baseWidth = (int) (sampleScale * baseWidth);
                baseHeight = (int) (sampleScale * baseHeight);
                var sample = new Bitmap(baseWidth, baseHeight);
                var graphics = Graphics.FromImage(sample);

                // Draw color if any
                graphics.FillRectangle(new SolidBrush(_shirtColor), 0, 0, sample.Width, sample.Height);

                // Draw shirt
                if (ProductType.Equals("garment", StringComparison.OrdinalIgnoreCase))
                {
                    if (_isFront && BaseImage != null)
                    {
                        graphics.DrawImage(BaseImage, 0, 0, sample.Width, sample.Height);
                    }
                    else if (!_isFront && BaseBackImage != null)
                    {
                        graphics.DrawImage(BaseBackImage, 0, 0, sample.Width, sample.Height);
                    }
                }

                // Draw art region
                var printableArea = Teechip.GetPrintableArea(ProductId, _isFront, ProductType);
                var width = baseWidth * printableArea[2];
                var height = baseHeight * printableArea[3];
                var y = baseHeight * printableArea[0];
                var x = (baseWidth - width) / 2 + baseWidth * printableArea[1];
                width -= width * Math.Abs(printableArea[4]);
                if (printableArea[4] > 0)
                {
                    x += baseWidth * printableArea[4];
                }

                if (ProductType.Equals("garment", StringComparison.OrdinalIgnoreCase))
                {
                    graphics.DrawRectangle(
                                           new Pen(Color.FromArgb(180, 100, 100, 100), 2),
                                           (float) x,
                                           (float) y,
                                           (float) width,
                                           (float) height);
                }

                // Draw art
                if (_isFront && ArtImage != null)
                {
                    var scale = 1D;
                    if (ArtImage.Width > width || ArtImage.Height > height)
                    {
                        var xScale = width / ArtImage.Width;
                        var yScale = height / ArtImage.Height;
                        scale = Math.Min(xScale, yScale);
                    }
                    var rWidth = (float) (scale * ArtImage.Width);
                    var rHeight = (float) (scale * ArtImage.Height);
                    var rx = x;
                    var ry = y;
                    if (FrontLeftAlign.Equals("center", StringComparison.OrdinalIgnoreCase))
                    {
                        rx += (int) ((float) width / 2 - rWidth / 2);
                    }
                    else if (FrontLeftAlign.Equals("right", StringComparison.OrdinalIgnoreCase))
                    {
                        rx += (int) (width - rWidth);
                    }
                    if (FrontTopAlign.Equals("middle", StringComparison.OrdinalIgnoreCase))
                    {
                        ry += (int) ((float) height / 2 - rHeight / 2);
                    }
                    else if (FrontTopAlign.Equals("bottom", StringComparison.OrdinalIgnoreCase))
                    {
                        ry += (int) (height - rHeight);
                    }
                    graphics.DrawImage(ArtImage, new RectangleF((float) rx, (float) ry, rWidth, rHeight));
                }
                else if (!_isFront && BackArtImage != null)
                {
                    var scale = 1D;
                    if (BackArtImage.Width > width || BackArtImage.Height > width)
                    {
                        var xScale = width / BackArtImage.Width;
                        var yScale = height / BackArtImage.Height;
                        scale = Math.Min(xScale, yScale);
                    }
                    var rWidth = (float) (scale * BackArtImage.Width);
                    var rHeight = (float) (scale * BackArtImage.Height);
                    var rx = x;
                    var ry = y;
                    if (BackLeftAlign.Equals("center", StringComparison.OrdinalIgnoreCase))
                    {
                        rx += (int) ((float) width / 2 - rWidth / 2);
                    }
                    else if (BackLeftAlign.Equals("right", StringComparison.OrdinalIgnoreCase))
                    {
                        rx += (int) (width - rWidth);
                    }
                    if (BackTopAlign.Equals("middle", StringComparison.OrdinalIgnoreCase))
                    {
                        ry += (int) ((float) height / 2 - rHeight / 2);
                    }
                    else if (BackTopAlign.Equals("bottom", StringComparison.OrdinalIgnoreCase))
                    {
                        ry += (int) (height - rHeight);
                    }
                    graphics.DrawImage(BackArtImage, new RectangleF((float) rx, (float) ry, rWidth, rHeight));
                }

                // Draw mug, phone case or poster
                if (ProductType.Equals("mug", StringComparison.OrdinalIgnoreCase) ||
                    ProductType.Equals("case", StringComparison.OrdinalIgnoreCase))
                {
                    if (BaseImage != null)
                    {
                        graphics.DrawImage(BaseImage, 0, 0, baseWidth, baseHeight);
                    }
                    graphics.DrawRectangle(
                                           new Pen(Color.FromArgb(180, 100, 100, 100), 2),
                                           (float) x,
                                           (float) y,
                                           (float) width,
                                           (float) height);
                }
                else if (ProductType.Equals("poster", StringComparison.OrdinalIgnoreCase))
                {
                    if (Orientation == "Portrait")
                    {
                        if (Border && BorderBaseImage != null)
                        {
                            graphics.DrawImage(BorderBaseImage, 0, 0, baseWidth, baseHeight);
                        }
                        else if (!Border && BaseImage != null)
                        {
                            graphics.DrawImage(BaseImage, 0, 0, baseWidth, baseHeight);
                        }
                    }
                    else
                    {
                        if (Border && BorderLandscapeBaseImage != null)
                        {
                            graphics.DrawImage(BorderLandscapeBaseImage, 0, 0, baseWidth, baseHeight);
                        }
                        else if (!Border && LandscapeBaseImage != null)
                        {
                            graphics.DrawImage(LandscapeBaseImage, 0, 0, baseWidth, baseHeight);
                        }
                    }
                    graphics.DrawRectangle(
                                           new Pen(Color.FromArgb(180, 100, 100, 100), 2),
                                           (float) x,
                                           (float) y,
                                           (float) width,
                                           (float) height);
                }

                graphics.Dispose();
                PictureBoxProductSample.Image = sample;
            }

            if (ArtImage != null && BackArtImage != null)
            {
                NumericUpDownPrice.Minimum = ProductsInfo.GetPrice(ProductId, true);
                if (NumericUpDownPrice.Value < NumericUpDownPrice.Minimum)
                {
                    NumericUpDownPrice.Value = NumericUpDownPrice.Minimum;
                }
            }
            else
            {
                NumericUpDownPrice.Minimum = ProductsInfo.GetPrice(ProductId, false);
                if (NumericUpDownPrice.Value < NumericUpDownPrice.Minimum)
                {
                    NumericUpDownPrice.Value = NumericUpDownPrice.Minimum;
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
            ReloadSample();

            OnSelectedColorsChanged(e);
        }

        private void ButtonChangeSide_Click(object sender, EventArgs e)
        {
            _isFront = !_isFront;
            ReloadSample();
        }

        private void ButtonChangeOrientation_Click(object sender, EventArgs e)
        {
            Orientation = Orientation == "Portrait" ? "Landscape" : "Portrait";
            ButtonChangeOrientation.Text = Orientation == "Portrait" ? "Landscape" : "Portrait";
            ReloadSample();
        }

        private void ButtonChangeBorder_Click(object sender, EventArgs e)
        {
            Border = !Border;
            ButtonChangeBorder.Text = Border ? "No border" : "Border";
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

            foreach (var color in colors)
            {
                foreach (var control in FlowLayoutProductColors.Controls)
                {
                    if (control is Button && (string) ((Button) control).Tag == color)
                    {
                        ((Button) control).Image = Properties.Resources.Checked;
                        _shirtColor = ((Button) control).BackColor;

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
