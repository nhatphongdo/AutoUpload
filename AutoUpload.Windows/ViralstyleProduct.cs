using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AutoUpload.Shared;
using Newtonsoft.Json.Linq;

namespace AutoUpload.Windows
{
    public partial class ViralStyleProduct : ProductControl<int>
    {
        public JToken Product { get; private set; }

        public JToken ProductsInfo { get; private set; }

        public JToken Pricing { get; private set; }
        
        public ViralStyleProduct()
        {
            InitializeComponent();
        }

        public void SetProduct(JToken product, JToken productsInfo, JToken pricing)
        {
            Product = product;
            ProductsInfo = productsInfo;
            Pricing = pricing;

            GroupBoxProduct.Text = product["name"].ToObject<string>();
            foreach (var color in product["product_colors"].ToObject<JArray>())
            {
                var colorButton = new Button
                                  {
                                      Size = new Size(32, 32),
                                      BackColor = ColorTranslator.FromHtml(color["hex"].ToObject<string>()),
                                      BackgroundImageLayout = ImageLayout.Stretch,
                                      Tag = color["id"].ToObject<int>()
                                  };
                FlowLayoutProductColors.Controls.Add(colorButton);

                TooltipProduct.SetToolTip(colorButton, color["name"].ToObject<string>());

                colorButton.Click += ButtonColorClicked;
            }

            if (!Directory.Exists("Platform/ViralStyle/Shirts/"))
            {
                Directory.CreateDirectory("Platform/ViralStyle/Shirts/");
            }
            var productName = Path.GetFileName(product["front_base"].ToObject<string>());
            if (!File.Exists("Platform/ViralStyle/Shirts/" + productName))
            {
                var outputStream = new FileStream("Platform/ViralStyle/Shirts/" + productName, FileMode.OpenOrCreate, FileAccess.Write);
                var downloadResult = Common.DownloadFile(product["base_url"].ToObject<string>() + productName, outputStream);
                outputStream.Close();
                if (downloadResult == false)
                {
                    File.Delete("Platform/ViralStyle/Shirts/" + productName);
                }
            }
            try
            {
                BaseImage = Image.FromFile("Platform/ViralStyle/Shirts/" + productName);
            }
            catch (Exception ex)
            {
                BaseImage = null;
            }
            productName = Path.GetFileName(product["back_base"].ToObject<string>());
            if (!string.IsNullOrEmpty(productName))
            {
                if (!File.Exists("Platform/ViralStyle/Shirts/" + productName))
                {
                    var outputStream = new FileStream(
                                                      "Platform/ViralStyle/Shirts/" + productName,
                                                      FileMode.OpenOrCreate,
                                                      FileAccess.Write);
                    var downloadResult = Common.DownloadFile(product["base_url"].ToObject<string>() + productName, outputStream);
                    outputStream.Close();
                    if (downloadResult == false)
                    {
                        File.Delete("Platform/ViralStyle/Shirts/" + productName);
                    }
                }
                try
                {
                    BaseBackImage = Image.FromFile("Platform/ViralStyle/Shirts/" + productName);
                }
                catch (Exception ex)
                {
                    BaseBackImage = null;
                }
            }
            ReloadSample();

            NumericUpDownPrice.Minimum = CalculatePrice(false, 1);
            try
            {
                NumericUpDownPrice.Value = decimal.Parse(product["suggested_price"].ToObject<string>());
            }
            catch (Exception exc)
            {
                NumericUpDownPrice.Value = NumericUpDownPrice.Minimum;
            }
        }

        private void ButtonColorClicked(object sender, EventArgs e)
        {
            var button = (Button) sender;

            if (button.Image == null && SelectedColors.Count >= 20)
            {
                MessageBox.Show(
                                @"You cannot select more than 20 colors for each style.",
                                @"Auto Upload",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            button.Image = button.Image == null ? Properties.Resources.Checked : null;
            if (button.Image != null)
            {
                _shirtColor = button.BackColor;

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
            if (Product == null)
            {
                return;
            }

            if (reloadImage)
            {
                if (BaseImage == null || BaseImage.Width == 0 || BaseImage.Height == 0)
                {
                    return;
                }
                var sampleScale = Math.Min((float) PictureBoxProductSample.Width / BaseImage.Width, (float) PictureBoxProductSample.Height / BaseImage.Height);
                var sample = new Bitmap((int) (sampleScale * BaseImage.Width), (int) (sampleScale * BaseImage.Height));
                var graphics = Graphics.FromImage(sample);

                // Draw color if any
                graphics.FillRectangle(new SolidBrush(_shirtColor), 0, 0, sample.Width, sample.Height);

                // Draw shirt
                if (_isFront && BaseImage != null)
                {
                    graphics.DrawImage(BaseImage, new RectangleF(0, 0, sample.Width, sample.Height));
                }
                else if (!_isFront && BaseBackImage != null)
                {
                    graphics.DrawImage(BaseBackImage, new RectangleF(0, 0, sample.Width, sample.Height));
                }

                // Draw art region
                var width = _isFront ? Product["front_width"].ToObject<int>() : Product["back_width"].ToObject<int>();
                var height = _isFront ? Product["front_height"].ToObject<int>() : Product["back_height"].ToObject<int>();
                var y = _isFront ? Product["front_top"].ToObject<int>() : Product["back_top"].ToObject<int>();
                var x = _isFront ? Product["front_left"].ToObject<int>() : Product["back_left"].ToObject<int>();
                width = (int)(sampleScale * width);
                height = (int)(sampleScale * height);
                x = (int)(sampleScale * x);
                y = (int)(sampleScale * y);
                graphics.DrawRectangle(new Pen(Color.FromArgb(180, 100, 100, 100), 2), x, y, width, height);

                // Draw art
                if (_isFront && ArtImage != null)
                {
                    var scale = 1D;
                    if (ArtImage.Width > width || ArtImage.Height > height)
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
                    var scale = 1D;
                    if (BackArtImage.Width > width || BackArtImage.Height > height)
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

                graphics.Dispose();
                PictureBoxProductSample.Image = sample;
            }

            NumericUpDownPrice.Minimum = CalculatePrice(ArtImage != null && BackArtImage != null, 1);
            if (NumericUpDownPrice.Value < NumericUpDownPrice.Minimum)
            {
                NumericUpDownPrice.Value = decimal.Parse(Product["suggested_price"].ToObject<string>());
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

        public decimal CalculatePrice(bool bothside, int goal)
        {
            if (Product == null || Pricing == null)
            {
                return 0;
            }

            try
            {
                var price = (decimal) 0;
                if (Product["is_non_apparel"].ToObject<int>() == 1 || Product["sublimation"].ToObject<int>() == 1 ||
                    Product["is_phone_case"].ToObject<int>() == 1 || Product["is_embroidery"].ToObject<int>() == 1 ||
                    (Product["sublimation"].ToObject<int>() == 0 && Product["special_printing"].ToObject<int>() == 1) ||
                    (goal <= 10))
                {
                    price = (Product["sublimation"].ToObject<int>() == 1 && Product["is_phone_case"].ToObject<int>() == 0) ||
                            (Product["sublimation"].ToObject<int>() == 0 && Product["special_printing"].ToObject<int>() == 1 &&
                             Product["is_embroidery"].ToObject<int>() == 0)
                                ? (bothside
                                       ? decimal.Parse(Product["sublimation_double_side_price"].ToObject<string>())
                                       : decimal.Parse(Product["sublimation_single_side_price"].ToObject<string>())) +
                                  decimal.Parse(Product["avg_cost"].ToObject<string>())
                                : Product["is_embroidery"].ToObject<int>() == 1
                                    ? decimal.Parse(Product["avg_cost"].ToObject<string>()) + decimal.Parse(Pricing["EMBROIDERY_PRICE"].ToObject<string>())
                                    : decimal.Parse(Product["avg_cost"].ToObject<string>()) + decimal.Parse(Pricing["DTG_PRICE"].ToObject<string>());
                    if (bothside)
                    {
                        price += Math.Ceiling(decimal.Parse(Pricing["DTG_PRICE"].ToObject<string>()) / 2);
                    }
                }
                else
                {
                }

                price += Product["special_printing"].ToObject<int>() == 1 && !string.IsNullOrEmpty(Product["special_print_setup_costs"].ToObject<string>()) && goal <= 20 && goal >= 1
                             ? decimal.Parse(Product["special_print_setup_costs"].ToObject<string>()) / goal
                             : 0;

                return price;
            }
            catch (Exception exc)
            {
                return 0;
            }
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

            foreach (var color in colors)
            {
                foreach (var control in FlowLayoutProductColors.Controls)
                {
                    if (control is Button && (int) ((Button) control).Tag == color)
                    {
                        ((Button) control).Image = Properties.Resources.Checked;
                        _shirtColor = ((Button) control).BackColor;

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
