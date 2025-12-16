using AutoUpload.Photoshop;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PhotoshopFile
{
    [DebuggerDisplay("Name = {Name}")]
    public class LayerGroup : Layer
    {
        public List<Layer> SubLayers { get; private set; }

        public bool IsClosed { get; set; }

        public LayerGroup(PsdFile psdFile)
            : base(psdFile)
        {
            SubLayers = new List<Layer>();
        }

        public LayerGroup(Layer layer)
            : base(layer)
        {
            SubLayers = new List<Layer>();
        }

        public new MagickImage ToImage()
        {
            var size = Rect.Size;
            if (size.Width == 0 || size.Height == 0)
            {
                if (Masks.LayerMask != null)
                {
                    size = Masks.LayerMask.Rect.Size;
                }
            }
            var layerImage = new MagickImage(MagickColor.FromRgba(0, 0, 0, 0), size.Width, size.Height);
            if (size.Width == 0 || size.Height == 0)
            {
                return layerImage;
            }

            for (var i = SubLayers.Count - 1; i >= 0; i--)
            {
                var layer = SubLayers[i];
                if (layer.Visible == false)
                {
                    continue;
                }
                var subLayerImage = layer is LayerGroup ? ((LayerGroup)layer).ToImage() : layer.ToImage();
                if (i > 0)
                {
                    var clippingIndex = i - 1;
                    while (clippingIndex >= 0 && SubLayers[clippingIndex].Clipping)
                    {
                        --clippingIndex;
                    }
                    var temp = new MagickImage(subLayerImage);
                    for (var j = i - 1; j >= clippingIndex + 1; j--)
                    {
                        if (SubLayers[j].Visible == false)
                        {
                            continue;
                        }
                        var clipImage = SubLayers[j].ToImage();
                        clipImage.Composite(temp, 
                            layer.Rect.Left - SubLayers[j].Rect.Left, layer.Rect.Top - SubLayers[j].Rect.Top,
                            CompositeOperator.DstAtop);
                        clipImage.Crop(layer.Rect.Left - SubLayers[j].Rect.Left, layer.Rect.Top - SubLayers[j].Rect.Top, layer.Rect.Width, layer.Rect.Height);
                        subLayerImage.Blend(clipImage, SubLayers[j].BlendModeKey);
                        clipImage.Dispose();
                    }
                    i = clippingIndex + 1;
                    temp.Dispose();
                }
                var blendMode = layer.BlendModeKey;
                if (layer is LayerGroup && layer.AdditionalInfo.FindByKey("lsct") != null)
                {
                    blendMode = (layer.AdditionalInfo.FindByKey("lsct") as LayerSectionInfo).BlendModeKey;
                }
                layerImage.Blend(subLayerImage, blendMode, layer.Rect.Left, layer.Rect.Top);
                subLayerImage.Dispose();
            }

            // Apply mask
            var mask = Masks.LayerMask;
            if (mask != null && mask.Disabled == false)
            {
                var pixels = layerImage.GetPixels().ToByteArray("RGBA");
                for (var y = 0; y < size.Height; y++)
                    for (var x = 0; x < size.Width; x++)
                    {
                        //if (mask.PositionVsLayer)
                        //{
                        //    if (x < mask.Rect.Left || x >= mask.Rect.Left + mask.Rect.Width
                        //        || y < mask.Rect.Top || y >= mask.Rect.Top + mask.Rect.Height)
                        //    {
                        //        pixels[4 * (x + y * size.Width) + 3] = 0;
                        //    }
                        //}
                        //else
                        {
                            if (x < (mask.Rect.Left - Rect.Left) || x >= (mask.Rect.Left - Rect.Left + mask.Rect.Width)
                                || y < (mask.Rect.Top - Rect.Top) || y >= (mask.Rect.Top - Rect.Top + mask.Rect.Height))
                            {
                                if (mask.BackgroundColor == 0)
                                {
                                    pixels[4 * (x + y * size.Width) + 3] = 0;
                                }
                            }
                        }
                    }

                for (var y = 0; y < mask.Rect.Height; y++)
                {
                    for (var x = 0; x < mask.Rect.Width; x++)
                    {
                        if (x + (mask.Rect.Left - Rect.Left) < 0 || x + (mask.Rect.Left - Rect.Left) >= size.Width &&
                            (y + mask.Rect.Top - Rect.Top) < 0 || (y + mask.Rect.Top - Rect.Top) >= size.Height)
                        {
                            continue;
                        }

                        var position = 4 * (x + (mask.Rect.Left - Rect.Left) + (y + (mask.Rect.Top - Rect.Top)) * size.Width) + 3;
                        if (position < 0 || position >= pixels.Length)
                        {
                            continue;
                        }
                        //if (mask.PositionVsLayer)
                        //{
                        //    pixels[4 * (mask.Rect.Left + x + (mask.Rect.Top + y) * size.Width) + 3] =
                        //        (byte)(pixels[4 * (mask.Rect.Left + x + (mask.Rect.Top + y) * size.Width) + 3]
                        //        * mask.ImageData[x + y * mask.Rect.Size.Width] / 255.0);
                        //}
                        //else
                        {
                            if (mask.BackgroundColor == 0)
                            {
                                pixels[position] = (byte)(pixels[position] * mask.ImageData[x + y * mask.Rect.Size.Width] / 255.0);
                            }
                            else
                            {
                                pixels[position] = (byte)(pixels[position] * mask.ImageData[x + y * mask.Rect.Size.Width] / 255.0);
                            }
                        }
                    }
                }

                layerImage.GetPixels().Set(pixels);

                pixels = null;
                GC.Collect();
                GC.WaitForFullGCComplete();
            }

            return layerImage;
        }
    }
}
