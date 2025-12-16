using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AutoUpload.Photoshop;
using AutoUpload.Shared;
using AutoUpload.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutoUpload.Windows
{
    public partial class TeespringNewProduct : ProductControl<string>
    {
        private Image _shirtTexture;

        public JToken Product { get; private set; }

        public JToken ProductsInfo { get; private set; }

        public string Object3DFile { get; private set; }

        public JToken Instructions { get; private set; }

        public Point3D CameraPoint { get; private set; }

        public Point3D CameraRotation { get; private set; }

        public List<Light> Lights { get; private set; }
        
        public TeespringNewProduct()
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

            foreach (var color in product["colors"].ToObject<JArray>())
            {
                var colorButton = new Button
                                  {
                                      Size = new Size(32, 32),
                                      BackColor = ColorTranslator.FromHtml("#" + color["hex_value"].ToObject<string>().TrimStart('#')),
                                      BackgroundImageLayout = ImageLayout.Stretch,
                                      Tag = color["id"].ToObject<string>()
                                  };
                FlowLayoutProductColors.Controls.Add(colorButton);

                TooltipProduct.SetToolTip(colorButton, color["name"].ToObject<string>());

                colorButton.Click += ButtonColorClicked;
            }

            var height = 0;
            foreach (var size in product["sizes"].ToObject<JArray>())
            {
                var sizeLabel = new Label()
                                {
                                    Location = new Point(3, height),
                                    Size = new Size(62, 20),
                                    Text = size["name"].ToObject<string>(),
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    AutoEllipsis = true,
                                };
                var numericUpDown = new NumericUpDown()
                                    {
                                        Location = new Point(63, height),
                                        Size = new Size(60, 20),
                                        Tag = size["id"].ToObject<string>(),
                                        Increment = (decimal) 0.01,
                                        DecimalPlaces = 2,
                                        Minimum = 0,
                                        Maximum = 10000
                                    };
                if (product["recommended_pricing"]["pricing_mode"].ToObject<string>().ToLower() == "size")
                {
                    foreach (var price in product["recommended_pricing"]["prices"].ToObject<JArray>())
                    {
                        if (price["product_size_id"].ToObject<string>().ToLower() == size["id"].ToObject<string>().ToLower())
                        {
                            numericUpDown.Minimum = (decimal) price["minimum_price_in_cents"].ToObject<int>() / 100;
                            numericUpDown.Value = (decimal) price["recommended_price_in_cents"].ToObject<int>() / 100;
                        }
                    }
                }
                else
                {
                    numericUpDown.Minimum = (decimal) product["recommended_pricing"]["minimum_price_in_cents"].ToObject<int>() / 100;
                    numericUpDown.Value = (decimal) product["recommended_pricing"]["recommended_price_in_cents"].ToObject<int>() / 100;
                }
                FlowLayoutPrices.Controls.Add(sizeLabel);
                FlowLayoutPrices.Controls.Add(numericUpDown);

                TooltipProduct.SetToolTip(sizeLabel, size["name"].ToObject<string>());

                height += 26;
            }
            Height = FlowLayoutPrices.Location.Y + height + 3;

            if (!Directory.Exists("Platform/Teespring/Shirts/"))
            {
                Directory.CreateDirectory("Platform/Teespring/Shirts/");
            }

            foreach (var template in product["mockup_templates"].ToObject<JArray>())
            {
                var productName = template["placements"][0]["crop"]["product_template_id"].ToObject<string>().ToLower();
                JToken mockup = null;
                foreach (var productMockup in product["product_templates"].ToObject<JArray>())
                {
                    if (productMockup["id"].ToObject<string>().ToLower() == productName)
                    {
                        mockup = productMockup;
                    }
                }
                if (mockup == null)
                {
                    continue;
                }

                string imageUrl;
                if (template["type"].ToObject<string>() == "3D")
                {
                    var instructions = JsonConvert.DeserializeObject<JToken>(template["instructions"].ToObject<string>());
                    imageUrl = instructions["textureOverlay"] != null
                                   ? instructions["textureOverlay"].ToObject<string>()
                                   : instructions["backgroundImage"] != null
                                       ? instructions["backgroundImage"].ToObject<string>()
                                       : mockup["image_url"].ToObject<string>();

                    // Download Obj file
                    if (instructions["subject"]["objUrl"] != null && !File.Exists($"Platform/Teespring/Shirts/{productName}.obj"))
                    {
                        var outputStream = new FileStream($"Platform/Teespring/Shirts/{productName}.obj", FileMode.OpenOrCreate, FileAccess.Write);
                        var downloadResult = Common.DownloadFile(instructions["subject"]["objUrl"].ToObject<string>(), outputStream);
                        outputStream.Close();
                        if (downloadResult == false)
                        {
                            File.Delete($"Platform/Teespring/Shirts/{productName}.obj");
                        }
                    }

                    if (File.Exists($"Platform/Teespring/Shirts/{productName}.obj"))
                    {
                        Object3DFile = $"Platform/Teespring/Shirts/{productName}.obj";
                    }
                }
                else
                {
                    imageUrl = template["image_url"].ToObject<string>();
                }
                productName += ".png";
                if (!File.Exists("Platform/Teespring/Shirts/" + productName))
                {
                    var outputStream = new FileStream("Platform/Teespring/Shirts/" + productName, FileMode.OpenOrCreate, FileAccess.Write);
                    var downloadResult = Common.DownloadFile(imageUrl, outputStream);
                    outputStream.Close();
                    if (downloadResult == false)
                    {
                        File.Delete("Platform/Teespring/Shirts/" + productName);
                    }
                }
                try
                {
                    if (mockup["name"].ToObject<string>().ToLower() == "front")
                    {
                        BaseImage = Image.FromFile("Platform/Teespring/Shirts/" + productName);
                    }
                    else
                    {
                        BaseBackImage = Image.FromFile("Platform/Teespring/Shirts/" + productName);
                    }
                }
                catch (Exception ex)
                {
                    if (mockup["name"].ToObject<string>().ToLower() == "front")
                    {
                        BaseImage = null;
                    }
                    else
                    {
                        BaseBackImage = null;
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
                JToken productImage = null;
                JToken mockup = null;
                foreach (var template in Product["mockup_templates"].ToObject<JArray>())
                {
                    foreach (var productMockup in Product["product_templates"].ToObject<JArray>())
                    {
                        if (productMockup["id"].ToObject<string>().ToLower() == template["placements"][0]["crop"]["product_template_id"].ToObject<string>().ToLower())
                        {
                            mockup = productMockup;
                        }
                    }

                    if (mockup != null
                        && ((mockup["name"].ToObject<string>().ToLower() == "front" && _isFront)
                            || (mockup["name"].ToObject<string>().ToLower() == "back" && !_isFront)))
                    {
                        productImage = template;
                    }
                }

                if (productImage == null)
                {
                    return;
                }
                
                if (productImage["type"].ToObject<string>() == "3D" && !string.IsNullOrEmpty(Object3DFile))
                {
                    Instructions = JsonConvert.DeserializeObject<JToken>(productImage["instructions"].ToObject<string>());
                    CameraPoint = new Point3D
                                  {
                                      X = Instructions["camera"]["position"]["x"].ToObject<float>(),
                                      Y = Instructions["camera"]["position"]["y"].ToObject<float>(),
                                      Z = Instructions["camera"]["position"]["z"].ToObject<float>()
                                  };
                    CameraRotation = new Point3D
                                     {
                                         X = Instructions["camera"]["rotation"]["x"].ToObject<float>(),
                                         Y = Instructions["camera"]["rotation"]["y"].ToObject<float>(),
                                         Z = Instructions["camera"]["rotation"]["z"].ToObject<float>()
                                     };
                    Lights = new List<Light>();
                    if (Instructions["hemisphereLight"] != null)
                    {
                        var hemisphereLight = new Light
                                              {
                                                  BackgroundColor = Instructions["hemisphereLight"]["skyColor"].ToObject<int>(),
                                                  Color = Instructions["hemisphereLight"]["groundColor"].ToObject<int>(),
                                                  Intensity = Instructions["hemisphereLight"]["intensity"].ToObject<float>(),
                                                  Type = "hemisphere",
                                                  Position = new Point3D
                                                             {
                                                                 X = 0,
                                                                 Y = 0,
                                                                 Z = 0
                                                             }
                                              };
                        Lights.Add(hemisphereLight);
                    }
                    if (Instructions["ambientLight"] != null)
                    {
                        var ambientLight = new Light
                                           {
                                               Color = Instructions["ambientLight"]["color"].ToObject<int>(),
                                               Intensity = Instructions["ambientLight"]["intensity"].ToObject<float>(),
                                               Type = "ambient",
                                               Position = new Point3D
                                                          {
                                                              X = 0,
                                                              Y = 0,
                                                              Z = 0
                                                          }
                                           };
                        Lights.Add(ambientLight);
                    }
                    if (Instructions["spotLights"] != null)
                    {
                        Lights.AddRange(Instructions["spotLights"]
                                            .ToObject<JArray>()
                                            .Select(light => new Light
                                                             {
                                                                 Color = light["color"].ToObject<int>(),
                                                                 Position = new Point3D
                                                                            {
                                                                                X = light["position"]["x"].ToObject<double>(),
                                                                                Y = light["position"]["y"].ToObject<double>(),
                                                                                Z = light["position"]["z"].ToObject<double>()
                                                                            },
                                                                 Intensity = light["intensity"].ToObject<float>(),
                                                                 Type = "spot"
                                            }));
                    }
                    if (Instructions["pointLights"] != null)
                    {
                        Lights.AddRange(Instructions["pointLights"]
                                            .ToObject<JArray>()
                                            .Select(light => new Light
                                                             {
                                                                 Color = light["color"].ToObject<int>(),
                                                                 Position = new Point3D
                                                                            {
                                                                                X = light["position"]["x"].ToObject<double>(),
                                                                                Y = light["position"]["y"].ToObject<double>(),
                                                                                Z = light["position"]["z"].ToObject<double>()
                                                                            },
                                                                 Intensity = light["intensity"].ToObject<float>(),
                                                                 Type = "point"
                                                             }));
                    }
                    if (Instructions["directionalLights"] != null)
                    {
                        Lights.AddRange(Instructions["directionalLights"]
                                            .ToObject<JArray>()
                                            .Select(light => new Light
                                                             {
                                                                 Color = light["color"].ToObject<int>(),
                                                                 Position = new Point3D
                                                                            {
                                                                                X = light["position"]["x"].ToObject<double>(),
                                                                                Y = light["position"]["y"].ToObject<double>(),
                                                                                Z = light["position"]["z"].ToObject<double>()
                                                                            },
                                                                 Intensity = light["intensity"].ToObject<float>(),
                                                                 Type = "directional"
                                                             }));
                    }

                    var productName = productImage["placements"][0]["crop"]["product_template_id"].ToObject<string>().ToLower();
                    PictureBoxPreview.Image = PhotoshopHelper.Convert3DToImage(Object3DFile,
                                                                               Instructions["textureOverlay"] != null
                                                                                   ? ("Platform/Teespring/Shirts/" + productName + ".png")
                                                                                   : null,
                                                                               Instructions["canvasSize"]["width"].ToObject<int>(),
                                                                               Instructions["canvasSize"]["height"].ToObject<int>(),
                                                                               CameraPoint,
                                                                               CameraRotation,
                                                                               Lights,
                                                                               ArtImage != null ? new Bitmap(ArtImage) : null,
                                                                               _shirtColor,
                                                                               mockup["width"].ToObject<int>(),
                                                                               mockup["height"].ToObject<int>(),
                                                                               productImage["placements"][0]["crop"]["width"].ToObject<int>(),
                                                                               productImage["placements"][0]["crop"]["height"].ToObject<int>(),
                                                                               productImage["placements"][0]["crop"]["x_offset"].ToObject<int>(),
                                                                               productImage["placements"][0]["crop"]["y_offset"].ToObject<int>());
                }
                else if (productImage["type"].ToObject<string>() == "2D")
                {
                    
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

                        SelectedColors.Add((string) ((Button) control).Tag);
                    }
                }
            }

            OnSelectedColorsChanged(new EventArgs());
        }

        public Dictionary<string, decimal> GetPrices()
        {
            return FlowLayoutPrices.Controls.OfType<NumericUpDown>().ToDictionary(control => control.Tag.ToString(), control => control.Value);
        }

        public void SetPrices(Dictionary<string, decimal> prices)
        {
            foreach (var price in prices)
            {
                foreach (var control in FlowLayoutPrices.Controls)
                {
                    if (control is NumericUpDown && (control as NumericUpDown).Tag.ToString() == price.Key)
                    {
                        (control as NumericUpDown).Value = price.Value;
                    }
                }
            }
        }

        private void CheckBoxIsDefault_CheckedChanged(object sender, EventArgs e)
        {
            OnIsDefaultChanged(e);
        }
        
    }
}
