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
    public partial class TeespringProduct : ProductControl<int>
    {
        private Image _shirtTexture;

        public JToken Product { get; private set; }

        public JToken ProductsInfo { get; private set; }

        public TeespringProduct()
        {
            InitializeComponent();
        }

        public void SetProduct(JToken product, JToken productsInfo, JToken bootstrap)
        {
            if (product == null || productsInfo == null || bootstrap == null)
            {
                return;
            }

            Product = product;
            ProductsInfo = productsInfo;

            GroupBoxProduct.Text = product["name"].ToObject<string>();

            foreach (var color in product["colors_available"].ToObject<JArray>())
            {
                var colour = GetColor(color.ToObject<int>());
                if (colour == null)
                {
                    continue;
                }

                var colorButton = new Button
                                  {
                                      Size = new Size(32, 32),
                                      BackColor = ColorTranslator.FromHtml("#" + colour["value"].ToObject<string>().TrimStart('#')),
                                      BackgroundImageLayout = ImageLayout.Stretch,
                                      Tag = color.ToObject<int>()
                                  };
                FlowLayoutProductColors.Controls.Add(colorButton);

                TooltipProduct.SetToolTip(colorButton, colour["name"].ToObject<string>());

                colorButton.Click += ButtonColorClicked;
            }

            if (!Directory.Exists("Platform/Teespring/Shirts/"))
            {
                Directory.CreateDirectory("Platform/Teespring/Shirts/");
            }

            var productName = "product_type_" + product["product_type_id"].ToObject<string>() + "_front.png";

            if (!File.Exists("Platform/Teespring/Shirts/" + productName))
            {
                var outputStream = new FileStream("Platform/Teespring/Shirts/" + productName, FileMode.OpenOrCreate, FileAccess.Write);
                var downloadResult = Common.DownloadFile(Teespring.GetImageLink(bootstrap["cdn"].ToObject<string>(), productName), outputStream);
                outputStream.Close();
                if (downloadResult == false)
                {
                    File.Delete("Platform/Teespring/Shirts/" + productName);
                }
            }
            try
            {
                BaseImage = Image.FromFile("Platform/Teespring/Shirts/" + productName);
            }
            catch (Exception ex)
            {
                BaseImage = null;
            }
            var productGroup = GetProductGroup(product["product_group_id"].ToObject<int>());
            if (productGroup == null || (productGroup["singular"].Type != JTokenType.Null && !productGroup["singular"].ToObject<string>().Equals("sticker", StringComparison.OrdinalIgnoreCase)))
            {
                productName = "product_type_" + product["product_type_id"].ToObject<string>() + "_back.png";
                if (!File.Exists("Platform/Teespring/Shirts/" + productName))
                {
                    var outputStream = new FileStream("Platform/Teespring/Shirts/" + productName, FileMode.OpenOrCreate, FileAccess.Write);
                    var downloadResult = Common.DownloadFile(Teespring.GetImageLink(bootstrap["cdn"].ToObject<string>(), productName), outputStream);
                    outputStream.Close();
                    if (downloadResult == false)
                    {
                        File.Delete("Platform/Teespring/Shirts/" + productName);
                    }
                }
                try
                {
                    BaseBackImage = Image.FromFile("Platform/Teespring/Shirts/" + productName);
                }
                catch (Exception ex)
                {
                    BaseBackImage = null;
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
                var productImage = GetProductType(Product["product_type_id"].ToObject<int>());
                if (productImage == null)
                {
                    return;
                }

                var sampleScale = Math.Min(
                                           (float) PictureBoxProductSample.Width / productImage["image_width"].ToObject<int>(),
                                           (float) PictureBoxProductSample.Height / productImage["image_height"].ToObject<int>());
                var sample = new Bitmap(
                                        (int) (sampleScale * productImage["image_width"].ToObject<int>()),
                                        (int) (sampleScale * productImage["image_height"].ToObject<int>()));
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
                    graphics.DrawImage(BaseImage, new Rectangle(0, 0, sample.Width, sample.Height));
                }
                else if (!_isFront && BaseBackImage != null)
                {
                    graphics.DrawImage(BaseBackImage, new Rectangle(0, 0, sample.Width, sample.Height));
                }

                // Draw art region
                var width = _isFront
                                ? productImage["printable_front_width"].ToObject<float>()
                                : productImage["printable_back_width"].ToObject<float>();
                var height = _isFront
                                 ? productImage["printable_front_height"].ToObject<float>()
                                 : productImage["printable_back_height"].ToObject<float>();
                var y = _isFront ? productImage["printable_front_top"].ToObject<float>() : productImage["printable_back_top"].ToObject<float>();
                var x = _isFront ? productImage["printable_front_left"].ToObject<float>() : productImage["printable_back_left"].ToObject<float>();
                width = (int) (sampleScale * width);
                height = (int) (sampleScale * height);
                x = (int) (sampleScale * x);
                y = (int) (sampleScale * y);
                graphics.DrawRectangle(new Pen(Color.FromArgb(180, 100, 100, 100), 2), x, y, width, height);

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
                        var xScale = width / BackArtImage.Width;
                        var yScale = height / BackArtImage.Height;
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

        public JToken GetColor(int id)
        {
            if (ProductsInfo != null)
            {
                foreach (var color in ProductsInfo["product_colors"].ToObject<JArray>())
                {
                    if (color["id"].ToObject<int>() == id)
                    {
                        return color;
                    }
                }
            }

            return null;
        }

        public JToken GetProductGroup(int id)
        {
            if (ProductsInfo != null)
            {
                foreach (var group in ProductsInfo["product_groups"].ToObject<JArray>())
                {
                    if (group["id"].ToObject<int>() == id)
                    {
                        return group;
                    }
                }
            }

            return null;
        }

        public JToken GetProductType(int id)
        {
            if (ProductsInfo != null)
            {
                foreach (var type in ProductsInfo["product_types"].ToObject<JArray>())
                {
                    if (type["id"].ToObject<int>() == id)
                    {
                        return type;
                    }
                }
            }

            return null;
        }

        public JToken GetProductImage(int id)
        {
            if (ProductsInfo != null)
            {
                foreach (var image in ProductsInfo["product_images"].ToObject<JArray>())
                {
                    if (image["product_id"].ToObject<int>() == id)
                    {
                        return image;
                    }
                }
            }

            return null;
        }

        public void SetDefaultPrice(decimal minimum, decimal recommendation)
        {
            if (NumericUpDownPrice.Value < minimum)
            {
                NumericUpDownPrice.Value = recommendation;
            }
            NumericUpDownPrice.Minimum = minimum;
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
