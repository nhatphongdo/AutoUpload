using System.IO;
using System.Linq;
using ImageMagick;
using System;
using System.Collections.Generic;
using PhotoshopFile;
using PhotoshopFile.Actions;
using System.Drawing;
using System.Drawing.Imaging;
using Aspose.ThreeD;
using Aspose.ThreeD.Entities;
using Aspose.ThreeD.Render;
using Aspose.ThreeD.Shading;
using Aspose.ThreeD.Utilities;
using Color = System.Drawing.Color;
using Graphics = System.Drawing.Graphics;
using Image = System.Drawing.Image;
using Models = AutoUpload.Shared.Models;
using Pen = System.Drawing.Pen;
using PointF = System.Drawing.PointF;
using Rectangle = System.Drawing.Rectangle;
using RectangleF = System.Drawing.RectangleF;
using SeekOrigin = System.IO.SeekOrigin;

namespace AutoUpload.Photoshop
{
    public class PhotoshopHelper
    {
        public static Layer[] GetAllLayers(string psdFile)
        {
            using (var stream = new FileStream(psdFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var loadContext = new LoadContext();
                var image = new PsdFile(stream, loadContext);
                return image.Layers.ToArray();
            }
        }

        public static void CreateMockup(string psdFile, string designFile, string outputFile,
                                        Color designBackground = new Color(),
                                        Color shirtColor = new Color(), string shirtTexture = null,
                                        Color laceColor = new Color(), int maxWidth = 0, int maxHeight = 0,
                                        string horizontalAlign = "Center", string verticalAlign = "Middle")
        {
            using (var stream = new MemoryStream())
            {
                // Decrypt TIPS file
                EncryptedPhotoshopHeader header;
                var error = Decrypt(psdFile, stream, out header);

                if (header == null)
                {
                    throw new Exception(error);
                }
                stream.Seek(0, SeekOrigin.Begin);

                var loadContext = new LoadContext();
                var image = new PsdFile(stream, loadContext);

                if (header.DesignLayer >= 0 && header.DesignLayer < image.Layers.Count)
                {
                    using (var design = new MagickImage(designFile))
                    using (var thumbnail =
                        new
                            MagickImage(MagickColor.FromRgba(designBackground.R, designBackground.G, designBackground.B, designBackground.A),
                                        image.Layers[header.DesignLayer].Rect.Size.Width,
                                        image.Layers[header.DesignLayer].Rect.Size.Height))
                    {
                        design.BackgroundColor = MagickColors.Transparent;

                        var boundary = new RectangleF();

                        var placedLayer =
                            image.Layers[header.DesignLayer].AdditionalInfo.FindByKey("PlLd") as PlacedLayerInfo;

                        if (placedLayer != null)
                        {
                            var boundaryStructures =
                                ((placedLayer.DescriptorStructure as DescriptorStructure)
                                 ?.Structures?.FirstOrDefault(item => item.ItemKey == "bounds") as DescriptorStructure)
                                ?.Structures;
                            if (boundaryStructures != null)
                            {
                                boundary.X =
                                    (float) ((boundaryStructures.FirstOrDefault(item => item.ItemKey == "Left") as
                                                  UnitDoubleDescriptor)?.Value ?? 0);
                                boundary.Y =
                                    (float) ((boundaryStructures.FirstOrDefault(item => item.ItemKey == "Top") as
                                                  UnitDoubleDescriptor)?.Value ?? 0);
                                boundary.Width =
                                    (float) ((boundaryStructures.FirstOrDefault(item => item.ItemKey == "Rght") as
                                                  UnitDoubleDescriptor)?.Value ?? image.ColumnCount);
                                boundary.Height =
                                    (float) ((boundaryStructures.FirstOrDefault(item => item.ItemKey == "Btom") as
                                                  UnitDoubleDescriptor)?.Value ?? image.RowCount);
                            }
                        }

                        // Resize image to match with boundary
                        var width = header.DesignBound.Width == 0
                                        ? image.Layers[header.DesignLayer].Rect.Size.Width
                                        : header.DesignBound.Width;
                        var height = header.DesignBound.Height == 0
                                         ? image.Layers[header.DesignLayer].Rect.Size.Height
                                         : header.DesignBound.Height;

                        if (placedLayer != null)
                        {
                            if (header.DesignBound.Width == 0)
                            {
                                width = (int) boundary.Width;
                            }
                            if (header.DesignBound.Height == 0)
                            {
                                height = (int) boundary.Height;
                            }
                        }

                        var size = new MagickGeometry(image.ColumnCount, image.RowCount)
                                   {
                                       IgnoreAspectRatio = false,
                                   };
                        design.Thumbnail(size);
                        var boundingBox = FindBoundingBox(design);
                        design.Crop(boundingBox.X, boundingBox.Y, boundingBox.Width, boundingBox.Height);

                        size = new MagickGeometry(width, height)
                               {
                                   IgnoreAspectRatio = false,
                               };
                        if (horizontalAlign.ToLower() == "stretch" && verticalAlign.ToLower() != "stretch")
                        {
                            size = new MagickGeometry(width, 0)
                                   {
                                       IgnoreAspectRatio = false,
                                   };
                        }
                        else if (horizontalAlign.ToLower() != "stretch" && verticalAlign.ToLower() == "stretch")
                        {
                            size = new MagickGeometry(0, height)
                                   {
                                       IgnoreAspectRatio = false,
                                   };
                        }
                        else if (horizontalAlign.ToLower() == "stretch" && verticalAlign.ToLower() == "stretch")
                        {
                            size = new MagickGeometry(width, height)
                                   {
                                       IgnoreAspectRatio = true,
                                   };
                        }
                        design.Thumbnail(size);
                        var x = header.DesignBound.X;
                        var y = header.DesignBound.Y;

                        if (placedLayer != null)
                        {
                            using (var linkedLayer =
                                new
                                    MagickImage(MagickColor.FromRgba(designBackground.R, designBackground.G, designBackground.B, designBackground.A),
                                                (int) boundary.Width, (int) boundary.Height))
                            using (var boundaryLayer = new MagickImage(MagickColor.FromRgba(0, 0, 255, 255),
                                                                       (int) boundary.Width, (int) boundary.Height))
                            {
                                boundaryLayer.BackgroundColor = MagickColors.Transparent;
                                boundaryLayer.VirtualPixelMethod = VirtualPixelMethod.Transparent;
                                boundaryLayer.Transparent(MagickColors.White);

                                if (horizontalAlign.ToLower() == "center")
                                {
                                    x += (int) Math.Round((width - design.Width) / 2.0);
                                }
                                else if (horizontalAlign.ToLower() == "right")
                                {
                                    x += width - design.Width;
                                }
                                if (verticalAlign.ToLower() == "middle")
                                {
                                    y += (int) Math.Round((height - design.Height) / 2.0);
                                }
                                else if (verticalAlign.ToLower() == "bottom")
                                {
                                    y += height - design.Height;
                                }

                                linkedLayer.Composite(design, x, y, CompositeOperator.Over);
#if DEBUG
                                linkedLayer.Write(@"test0.png");
#endif

                                var warpBoundingBox = new RectangleF(0, 0, linkedLayer.Width, linkedLayer.Height);
                                linkedLayer.VirtualPixelMethod = VirtualPixelMethod.Transparent;

                                var controlPoints =
                                    (((placedLayer.DescriptorStructure as DescriptorStructure)
                                      ?.Structures?.FirstOrDefault(item => item.ItemKey == "customEnvelopeWarp")
                                      as DescriptorStructure)?.Structures[0] as ObjectArrayDescriptor)?.Items;
                                if (controlPoints != null)
                                {
                                    var xPoints = (controlPoints[0] as UnitDoublesDescriptor)
                                        ?.Values?.Select(item => (float) item).ToList();
                                    var yPoints = (controlPoints[1] as UnitDoublesDescriptor)
                                        ?.Values?.Select(item => (float) item).ToList();
                                    if (xPoints != null && yPoints != null)
                                    {
                                        // Change coordinates to real size
                                        var xMin = xPoints.Min();
                                        var xMax = xPoints.Max();
                                        var yMin = yPoints.Min();
                                        var yMax = yPoints.Max();
                                        var newWidth = (int) Math.Ceiling(xMax - xMin);
                                        var newHeight = (int) Math.Ceiling(yMax - yMin);

                                        var xScale = (float) linkedLayer.Width / newWidth;
                                        var yScale = (float) linkedLayer.Height / newHeight;

                                        for (var i = 0; i < xPoints.Count; i++)
                                        {
                                            xPoints[i] -= xMin;
                                            if (xScale < 1)
                                            {
                                                xPoints[i] *= xScale;
                                            }
                                        }
                                        for (var i = 0; i < yPoints.Count; i++)
                                        {
                                            yPoints[i] -= yMin;
                                            yPoints[i] *= yScale;
                                        }

                                        var points = new PointF[4][];
                                        points[0] = new PointF[4];
                                        points[0][0] = new PointF(boundary.X + xPoints[0], boundary.Y + yPoints[0]);
                                        points[0][1] = new PointF(boundary.X + xPoints[1], boundary.Y + yPoints[1]);
                                        points[0][2] = new PointF(boundary.X + xPoints[2], boundary.Y + yPoints[2]);
                                        points[0][3] = new PointF(boundary.X + xPoints[3], boundary.Y + yPoints[3]);
                                        points[1] = new PointF[4];
                                        points[1][0] = new PointF(boundary.X + xPoints[4], boundary.Y + yPoints[4]);
                                        points[1][1] = new PointF(boundary.X + xPoints[5], boundary.Y + yPoints[5]);
                                        points[1][2] = new PointF(boundary.X + xPoints[6], boundary.Y + yPoints[6]);
                                        points[1][3] = new PointF(boundary.X + xPoints[7], boundary.Y + yPoints[7]);
                                        points[2] = new PointF[4];
                                        points[2][0] = new PointF(boundary.X + xPoints[8], boundary.Y + yPoints[8]);
                                        points[2][1] = new PointF(boundary.X + xPoints[9], boundary.Y + yPoints[9]);
                                        points[2][2] = new PointF(boundary.X + xPoints[10], boundary.Y + yPoints[10]);
                                        points[2][3] = new PointF(boundary.X + xPoints[11], boundary.Y + yPoints[11]);
                                        points[3] = new PointF[4];
                                        points[3][0] = new PointF(boundary.X + xPoints[12], boundary.Y + yPoints[12]);
                                        points[3][1] = new PointF(boundary.X + xPoints[13], boundary.Y + yPoints[13]);
                                        points[3][2] = new PointF(boundary.X + xPoints[14], boundary.Y + yPoints[14]);
                                        points[3][3] = new PointF(boundary.X + xPoints[15], boundary.Y + yPoints[15]);

                                        var oldPoint01 = new PointF(0, 0);
                                        var oldPoint04 = new PointF(linkedLayer.Width, 0);
                                        var oldPoint13 = new PointF(0, linkedLayer.Height);
                                        var oldPoint16 = new PointF(linkedLayer.Width, linkedLayer.Height);
                                        //var oldPoint02 = GetPointOnLine(oldPoint01, oldPoint04, 1 / 3.0f);
                                        //var oldPoint03 = GetPointOnLine(oldPoint01, oldPoint04, 2 / 3.0f);
                                        //var oldPoint05 = GetPointOnLine(oldPoint01, oldPoint13, 1 / 3.0f);
                                        //var oldPoint08 = GetPointOnLine(oldPoint04, oldPoint16, 1 / 3.0f);
                                        //var oldPoint06 = GetPointOnLine(oldPoint05, oldPoint08, 1 / 3.0f);
                                        //var oldPoint07 = GetPointOnLine(oldPoint05, oldPoint08, 2 / 3.0f);
                                        //var oldPoint09 = GetPointOnLine(oldPoint01, oldPoint13, 2 / 3.0f);
                                        //var oldPoint12 = GetPointOnLine(oldPoint04, oldPoint16, 2 / 3.0f);
                                        //var oldPoint10 = GetPointOnLine(oldPoint09, oldPoint12, 1 / 3.0f);
                                        //var oldPoint11 = GetPointOnLine(oldPoint09, oldPoint12, 2 / 3.0f);
                                        //var oldPoint14 = GetPointOnLine(oldPoint13, oldPoint16, 1 / 3.0f);
                                        //var oldPoint15 = GetPointOnLine(oldPoint13, oldPoint16, 2 / 3.0f);

                                        //var point01 = DeCasteljau(points, 0, 0);
                                        //var point02 = DeCasteljau(points, 0, 1 / 3.0f);
                                        //var point03 = DeCasteljau(points, 0, 2 / 3.0f);
                                        //var point04 = DeCasteljau(points, 0, 1);
                                        //var point05 = DeCasteljau(points, 1 / 3.0f, 0);
                                        //var point06 = DeCasteljau(points, 1 / 3.0f, 1 / 3.0f);
                                        //var point07 = DeCasteljau(points, 1 / 3.0f, 2 / 3.0f);
                                        //var point08 = DeCasteljau(points, 1 / 3.0f, 1);
                                        //var point09 = DeCasteljau(points, 2 / 3.0f, 0);
                                        //var point10 = DeCasteljau(points, 2 / 3.0f, 1 / 3.0f);
                                        //var point11 = DeCasteljau(points, 2 / 3.0f, 2 / 3.0f);
                                        //var point12 = DeCasteljau(points, 2 / 3.0f, 1);
                                        //var point13 = DeCasteljau(points, 1, 0);
                                        //var point14 = DeCasteljau(points, 1, 1 / 3.0f);
                                        //var point15 = DeCasteljau(points, 1, 2 / 3.0f);
                                        //var point16 = DeCasteljau(points, 1, 1);

                                        //linkedLayer.Distort(DistortMethod.Shepards, true,
                                        //    oldPoint01.X, oldPoint01.Y, point01.X, point01.Y,
                                        //    oldPoint02.X, oldPoint02.Y, point02.X, point02.Y,
                                        //    oldPoint03.X, oldPoint03.Y, point03.X, point03.Y,
                                        //    oldPoint04.X, oldPoint04.Y, point04.X, point04.Y,
                                        //    oldPoint05.X, oldPoint05.Y, point05.X, point05.Y,
                                        //    oldPoint06.X, oldPoint06.Y, point06.X, point06.Y,
                                        //    oldPoint07.X, oldPoint07.Y, point07.X, point07.Y,
                                        //    oldPoint08.X, oldPoint08.Y, point08.X, point08.Y,
                                        //    oldPoint09.X, oldPoint09.Y, point09.X, point09.Y,
                                        //    oldPoint10.X, oldPoint10.Y, point10.X, point10.Y,
                                        //    oldPoint11.X, oldPoint11.Y, point11.X, point11.Y,
                                        //    oldPoint12.X, oldPoint12.Y, point12.X, point12.Y,
                                        //    oldPoint13.X, oldPoint13.Y, point13.X, point13.Y,
                                        //    oldPoint14.X, oldPoint14.Y, point14.X, point14.Y,
                                        //    oldPoint15.X, oldPoint15.Y, point15.X, point15.Y,
                                        //    oldPoint16.X, oldPoint16.Y, point16.X, point16.Y);
                                        Warp(linkedLayer, points);
                                        Warp(boundaryLayer, points);

#if DEBUG
                                        //DrawDebugGrid(point01, point02, point03, point04, point05, point06, point07, point08, point09, point10, point11, point12, point13, point14, point15, point16);
                                        //DrawDebugGrid(points, oldPoint01, oldPoint04, oldPoint13, oldPoint16);
                                        boundaryLayer.Write(@"grid1.png");
#endif
                                    }
#if DEBUG
                                    linkedLayer.Write(@"test1.png");
#endif
                                }

                                var transXMin = 0.0;
                                var transYMin = 0.0;
                                if (placedLayer.Transformation != null)
                                {
                                    var linkedWidth = linkedLayer.Width;
                                    var linkedHeight = linkedLayer.Height;
                                    var xMin =
                                        Math.Min(Math.Min(Math.Min(placedLayer.Transformation[0], placedLayer.Transformation[2]), placedLayer.Transformation[4]),
                                                 placedLayer.Transformation[6]);
                                    var yMin =
                                        Math.Min(Math.Min(Math.Min(placedLayer.Transformation[1], placedLayer.Transformation[3]), placedLayer.Transformation[5]),
                                                 placedLayer.Transformation[7]);
                                    var xMax =
                                        Math.Max(Math.Max(Math.Max(placedLayer.Transformation[0], placedLayer.Transformation[2]), placedLayer.Transformation[4]),
                                                 placedLayer.Transformation[6]);
                                    var yMax =
                                        Math.Max(Math.Max(Math.Max(placedLayer.Transformation[1], placedLayer.Transformation[3]), placedLayer.Transformation[5]),
                                                 placedLayer.Transformation[7]);
                                    var newWidth = (int) Math.Ceiling(xMax - xMin);
                                    var newHeight = (int) Math.Ceiling(yMax - yMin);
                                    linkedLayer.Extent(Math.Max(Math.Max(linkedWidth, newWidth), image.ColumnCount),
                                                       Math.Max(Math.Max(linkedHeight, newHeight), image.RowCount),
                                                       Gravity.Northwest);
                                    linkedLayer.Distort(DistortMethod.Perspective, false,
                                                        0, 0, placedLayer.Transformation[0] - xMin,
                                                        placedLayer.Transformation[1] - yMin,
                                                        linkedWidth, 0, placedLayer.Transformation[2] - xMin,
                                                        placedLayer.Transformation[3] - yMin,
                                                        linkedWidth, linkedHeight, placedLayer.Transformation[4] - xMin,
                                                        placedLayer.Transformation[5] - yMin,
                                                        0, linkedHeight, placedLayer.Transformation[6] - xMin,
                                                        placedLayer.Transformation[7] - yMin);
                                    linkedLayer.Crop(0, 0, newWidth, newHeight);

                                    boundaryLayer.Extent(Math.Max(Math.Max(linkedWidth, newWidth), image.ColumnCount),
                                                         Math.Max(Math.Max(linkedHeight, newHeight), image.RowCount),
                                                         Gravity.Northwest);
                                    boundaryLayer.Distort(DistortMethod.Perspective, false,
                                                          0, 0, placedLayer.Transformation[0] - xMin,
                                                          placedLayer.Transformation[1] - yMin,
                                                          linkedWidth, 0, placedLayer.Transformation[2] - xMin,
                                                          placedLayer.Transformation[3] - yMin,
                                                          linkedWidth, linkedHeight,
                                                          placedLayer.Transformation[4] - xMin,
                                                          placedLayer.Transformation[5] - yMin,
                                                          0, linkedHeight, placedLayer.Transformation[6] - xMin,
                                                          placedLayer.Transformation[7] - yMin);
                                    boundaryLayer.Crop(0, 0, newWidth, newHeight);

                                    if (xMin < 0)
                                    {
                                        transXMin = image.Layers[header.DesignLayer].Rect.X - xMin;
                                    }
                                    if (yMin < 0)
                                    {
                                        transYMin = image.Layers[header.DesignLayer].Rect.Y - yMin;
                                    }
#if DEBUG
                                    boundaryLayer.Write(@"grid2.png");
                                    linkedLayer.Write(@"test2.png");
#endif
                                }

                                warpBoundingBox = FindBoundingBox(boundaryLayer);
                                thumbnail.Composite(linkedLayer, -(int) Math.Round(warpBoundingBox.X + transXMin),
                                                    -(int) Math.Round(warpBoundingBox.Y + transYMin),
                                                    CompositeOperator.Over);

                                linkedLayer.Dispose();
                                boundaryLayer.Dispose();
                            }
                        }
                        else
                        {
                            if (horizontalAlign.ToLower() == "center")
                            {
                                x += (width - design.Width) / 2;
                            }
                            else if (horizontalAlign.ToLower() == "right")
                            {
                                x += (width - design.Width);
                            }
                            if (verticalAlign.ToLower() == "middle")
                            {
                                y += (height - design.Height) / 2;
                            }
                            else if (verticalAlign.ToLower() == "bottom")
                            {
                                y += (height - design.Height);
                            }
                            thumbnail.Composite(design, x, y, CompositeOperator.Over);
                        }

#if DEBUG
                        thumbnail.Write(@"test.png");
#endif
                        image.Layers[header.DesignLayer].FromImage(thumbnail);

                        thumbnail.Dispose();
                        design.Dispose();
                    }

                    GC.Collect();
                    GC.WaitForFullGCComplete();
                }

                if (header.ShirtColorLayer >= 0 && header.ShirtColorLayer < image.Layers.Count)
                {
                    using (var color =
                        new MagickImage(MagickColor.FromRgba(shirtColor.R, shirtColor.G, shirtColor.B, shirtColor.A),
                                        image.Layers[header.ShirtColorLayer].Rect.Size.Width,
                                        image.Layers[header.ShirtColorLayer].Rect.Size.Height))
                    {
                        image.Layers[header.ShirtColorLayer].FromImage(color);
                        color.Dispose();
                    }

                    GC.Collect();
                    GC.WaitForFullGCComplete();
                }

                if (header.LaceColorLayer >= 0 && header.LaceColorLayer < image.Layers.Count)
                {
                    using (var color =
                        new MagickImage(MagickColor.FromRgba(laceColor.R, laceColor.G, laceColor.B, laceColor.A),
                                        image.Layers[header.LaceColorLayer].Rect.Size.Width,
                                        image.Layers[header.LaceColorLayer].Rect.Size.Height))
                    {
                        image.Layers[header.LaceColorLayer].FromImage(color);
                        color.Dispose();
                    }

                    GC.Collect();
                    GC.WaitForFullGCComplete();
                }

                using (var outputImage =
                    new MagickImage(MagickColor.FromRgba(0, 0, 0, 0), image.ColumnCount, image.RowCount))
                {
                    // Ignore top layer (it covers for reducing size)
                    for (var i = image.GroupedLayers.Count - 1; i >= 1; i--)
                    {
                        var layer = image.GroupedLayers[i];
                        if (layer.Visible == false)
                        {
                            continue;
                        }

                        var layerImage = layer is LayerGroup ? ((LayerGroup) layer).ToImage() : layer.ToImage();

                        if (i > 0)
                        {
                            var clippingIndex = i - 1;
                            while (clippingIndex >= 0 && image.GroupedLayers[clippingIndex].Clipping)
                            {
                                --clippingIndex;
                            }
                            var temp = new MagickImage(layerImage);
                            for (var j = i - 1; j >= clippingIndex + 1; j--)
                            {
                                if (image.GroupedLayers[j].Visible == false)
                                {
                                    continue;
                                }
                                var clipImage = image.GroupedLayers[j].ToImage();
                                var x = layer.Rect.Left - image.GroupedLayers[j].Rect.Left;
                                var y = layer.Rect.Top - image.GroupedLayers[j].Rect.Top;

                                clipImage.Composite(temp, x, y, CompositeOperator.DstAtop);
                                clipImage.Crop(x, y, layer.Rect.Width, layer.Rect.Height);
#if DEBUG
                                clipImage.Write($"clip-{j}.png");
#endif
                                layerImage.Blend(clipImage, image.GroupedLayers[j].BlendModeKey, x < 0 ? -x : 0,
                                                 y < 0 ? -y : 0);
                                clipImage.Dispose();
                            }
                            i = clippingIndex + 1;
                            temp.Dispose();
                        }

#if DEBUG
                        layerImage.Write($"grouplayer-{i}.png");
#endif

                        var blendMode = layer.BlendModeKey;
                        if (layer is LayerGroup && layer.AdditionalInfo.FindByKey("lsct") != null)
                        {
                            blendMode = (layer.AdditionalInfo.FindByKey("lsct") as LayerSectionInfo).BlendModeKey;
                        }
                        outputImage.Blend(layerImage, blendMode, layer.Rect.Left, layer.Rect.Top);
                        layerImage.Dispose();
                    }

                    if (maxWidth == 0 && maxHeight > 0)
                    {
                        var newSize = new MagickGeometry(maxHeight, maxHeight)
                                      {
                                          IgnoreAspectRatio = false,
                                      };
                        outputImage.Thumbnail(newSize);
                    }
                    else if (maxWidth > 0 && maxHeight == 0)
                    {
                        var newSize = new MagickGeometry(maxWidth, maxWidth)
                                      {
                                          IgnoreAspectRatio = false,
                                      };
                        outputImage.Thumbnail(newSize);
                    }
                    else if (maxWidth > 0 && maxHeight > 0)
                    {
                        var newSize = new MagickGeometry(maxWidth, maxHeight)
                                      {
                                          IgnoreAspectRatio = false,
                                      };
                        outputImage.Thumbnail(newSize);
                    }
                    outputImage.Write(outputFile);
                    outputImage.Dispose();
                }

                stream.Close();
                image = null;
            }

            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        public static string Encrypt(string inputFile, string outputFile, int designLayer, int shirtColorLayer,
                                     int shirtTextureLayer, int laceColorLayer, Rectangle designBound)
        {
            try
            {
                var random = new Random((int) DateTime.Now.Ticks);
                using (var inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    // Load input stream to memory
                    var buffer = new byte[inputStream.Length];
                    inputStream.Read(buffer, 0, buffer.Length);

                    var outputArray = new List<byte>();

                    // Create header block
                    var header = new EncryptedPhotoshopHeader();
                    header.DesignLayer = designLayer;
                    header.ShirtColorLayer = shirtColorLayer;
                    header.ShirtTextureLayer = shirtTextureLayer;
                    header.LaceColorLayer = laceColorLayer;
                    header.DesignBound = designBound;

                    header.Checksum = buffer.GetChecksum();

                    // Encrypt blocks
                    var offset = 0;
                    header.NumberOfBlocks = random.Next(30, 51);
                    var maxSize = buffer.Length / header.NumberOfBlocks;
                    for (var i = 0; i < header.NumberOfBlocks; i++)
                    {
                        if (offset >= buffer.Length)
                        {
                            break;
                        }
                        var block = new EncryptedPhotoshopBlock();
                        block.BlockType = (int) BlockTypes.Encoded;
                        block.BlockOffset = offset + random.Next(0, maxSize / 2);
                        block.BlockSize = Math.Min(1024 * 1024, random.Next(maxSize / 4, maxSize / 2));

                        // Encrypt
                        if (block.BlockOffset > offset)
                        {
                            outputArray.AddRange(buffer.GetSubArray(offset, block.BlockOffset - offset));
                        }
                        var tempArray = new byte[block.BlockSize];
                        Array.Copy(buffer, block.BlockOffset, tempArray, 0, tempArray.Length);
                        byte[] key;
                        byte[] iv;
                        var encodedArray = tempArray.Encode(out key, out iv);
                        block.Key = Convert.ToBase64String(key);
                        block.InitialVector = Convert.ToBase64String(iv);
                        var blockOffset = outputArray.Count;
                        outputArray.AddRange(encodedArray);
                        if ((i + 1) * maxSize > block.BlockOffset + block.BlockSize)
                        {
                            outputArray.AddRange(buffer.GetSubArray(block.BlockOffset + block.BlockSize,
                                                                    (i + 1) * maxSize - block.BlockOffset -
                                                                    block.BlockSize));
                            offset = (i + 1) * maxSize;
                        }
                        else
                        {
                            offset = block.BlockOffset + block.BlockSize;
                        }

                        block.BlockOffset = (int) blockOffset;
                        block.BlockSize = encodedArray.Length;

                        header.Blocks.Add(block);
                    }

                    if (offset < buffer.Length)
                    {
                        outputArray.AddRange(buffer.GetSubArray(offset, buffer.Length - offset));
                    }

                    var headerArray = header.ToBytes();
                    outputStream.Write(headerArray, 0, headerArray.Length);
                    outputStream.Write(outputArray.ToArray(), 0, outputArray.Count);

                    // Dispose
                    inputStream.Close();
                    outputStream.Flush();
                    outputStream.Close();
                }
            }
            catch (Exception exc)
            {
                return exc.Message;
            }

            return string.Empty;
        }

        public static string Decrypt(string inputFile, Stream outputStream, out EncryptedPhotoshopHeader header)
        {
            try
            {
                using (var inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    // Load input stream to memory
                    var buffer = new byte[inputStream.Length];
                    inputStream.Read(buffer, 0, buffer.Length);

                    var outputArray = new List<byte>();

                    // Read header block
                    header = new EncryptedPhotoshopHeader(buffer, 0);

                    // Decrypt blocks
                    var offset = header.HeaderSize;
                    for (var i = 0; i < header.NumberOfBlocks; i++)
                    {
                        var block = header.Blocks[i];

                        // Decrypt
                        if (block.BlockOffset + header.HeaderSize > offset)
                        {
                            outputArray.AddRange(buffer.GetSubArray(offset,
                                                                    block.BlockOffset + header.HeaderSize - offset));
                            offset = block.BlockOffset + header.HeaderSize;
                        }
                        var tempArray = new byte[block.BlockSize];
                        Array.Copy(buffer, offset, tempArray, 0, tempArray.Length);
                        if (block.BlockType == (int) BlockTypes.Encoded)
                        {
                            var key = Convert.FromBase64String(block.Key);
                            var iv = Convert.FromBase64String(block.InitialVector);
                            var decodedArray = tempArray.Decode(key, iv);
                            outputArray.AddRange(decodedArray);
                        }
                        offset += tempArray.Length;
                    }

                    if (offset < buffer.Length)
                    {
                        outputArray.AddRange(buffer.GetSubArray(offset, buffer.Length - offset));
                    }

                    // Validate checksum
                    var writeArray = outputArray.ToArray();
                    var checksum = writeArray.GetChecksum();
                    if (checksum != header.Checksum)
                    {
                        inputStream.Close();
                        outputStream.Close();
                        header = null;
                        return "File is corrupted";
                    }

                    outputStream.Write(writeArray, 0, writeArray.Length);

                    // Dispose
                    inputStream.Close();
                }
            }
            catch (Exception exc)
            {
                header = null;
                return exc.Message;
            }

            return string.Empty;
        }

        private static Image CreateThumbnail(MagickImage image, int width, int height, Color backgroundColor,
                                             string horizontalAlign = "Center", string verticalAlign = "Middle")
        {
            Image result;
            using (var thumbnail = new MagickImage(MagickColor.FromRgba(backgroundColor.R, backgroundColor.G, backgroundColor.B, backgroundColor.A), width, height))
            {
                // Resize image to match with boundary
                var size = new MagickGeometry(width, height)
                           {
                               IgnoreAspectRatio = false,
                           };
                image.Thumbnail(size);

                var gravity = Gravity.Center;
                if (horizontalAlign.Equals("left", StringComparison.OrdinalIgnoreCase))
                {
                    if (verticalAlign.Equals("top", StringComparison.OrdinalIgnoreCase))
                    {
                        gravity = Gravity.Northwest;
                    }
                    else if (verticalAlign.Equals("middle", StringComparison.OrdinalIgnoreCase))
                    {
                        gravity = image.Width < width ? Gravity.West : Gravity.Center;
                    }
                    else if (verticalAlign.Equals("bottom", StringComparison.OrdinalIgnoreCase))
                    {
                        gravity = Gravity.Southwest;
                    }
                }
                else if (horizontalAlign.Equals("center", StringComparison.OrdinalIgnoreCase))
                {
                    if (verticalAlign.Equals("top", StringComparison.OrdinalIgnoreCase))
                    {
                        gravity = image.Width < width ? Gravity.Center : Gravity.North;
                    }
                    else if (verticalAlign.Equals("middle", StringComparison.OrdinalIgnoreCase))
                    {
                        gravity = Gravity.Center;
                    }
                    else if (verticalAlign.Equals("bottom", StringComparison.OrdinalIgnoreCase))
                    {
                        gravity = image.Width < width ? Gravity.Center : Gravity.South;
                    }
                }
                else if (horizontalAlign.Equals("right", StringComparison.OrdinalIgnoreCase))
                {
                    if (verticalAlign.Equals("top", StringComparison.OrdinalIgnoreCase))
                    {
                        gravity = Gravity.Northeast;
                    }
                    else if (verticalAlign.Equals("middle", StringComparison.OrdinalIgnoreCase))
                    {
                        gravity = image.Width < width ? Gravity.East : Gravity.Center;
                    }
                    else if (verticalAlign.Equals("bottom", StringComparison.OrdinalIgnoreCase))
                    {
                        gravity = Gravity.Southeast;
                    }
                }

                thumbnail.Composite(image, gravity, CompositeOperator.Over);

                result = thumbnail.ToBitmap();

                thumbnail.Dispose();
            }

            return result;
        }

        public static Image CreateThumbnail(string file, int width, int height, string horizontalAlign = "Center",
                                            string verticalAlign = "Middle")
        {
            try
            {
                Image result;
                using (var image = new MagickImage(file))
                {
                    result = CreateThumbnail(image, width, height, Color.Transparent, horizontalAlign, verticalAlign);
                    image.Dispose();
                }

                GC.Collect();
                GC.WaitForFullGCComplete();

                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static Image CreateThumbnail(Bitmap bitmap, int width, int height, Color backgroundColor, string horizontalAlign = "Center",
                                            string verticalAlign = "Middle")
        {
            try
            {
                Image result;
                using (var image = new MagickImage(bitmap))
                {
                    result = CreateThumbnail(image, width, height, backgroundColor, horizontalAlign, verticalAlign);
                    image.Dispose();
                }

                GC.Collect();
                GC.WaitForFullGCComplete();

                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static Image Convert3DToImage(string objFile, string textureFile, int width, int height, 
                                             Models.Point3D cameraPos, Models.Point3D cameraRotation, List<Models.Light> lights, 
                                             Bitmap design, Color objectColor, int drawWidth, int drawHeight, 
                                             int designWidth, int designHeight, int x = 0, int y = 0,
                                             int textureWidth = 0, int textureHeight = 0,
                                             string horizontalAlign = "Center", string verticalAlign = "Middle")
        {
            const string lData =
                "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjxMaWNlbnNlPg0KICAgIDxEYXRhPg0KICAgICAgICA8TGljZW5zZWRUbz5pckRldmVsb3BlcnMuY29tPC9MaWNlbnNlZFRvPg0KICAgICAgICA8RW1haWxUbz5pbmZvQGlyRGV2ZWxvcGVycy5jb208L0VtYWlsVG8+DQogICAgICAgIDxMaWNlbnNlVHlwZT5EZXZlbG9wZXIgT0VNPC9MaWNlbnNlVHlwZT4NCiAgICAgICAgPExpY2Vuc2VOb3RlPkxpbWl0ZWQgdG8gMTAwMCBkZXZlbG9wZXIsIHVubGltaXRlZCBwaHlzaWNhbCBsb2NhdGlvbnM8L0xpY2Vuc2VOb3RlPg0KICAgICAgICA8T3JkZXJJRD43ODQzMzY0Nzc4NTwvT3JkZXJJRD4NCiAgICAgICAgPFVzZXJJRD4xMTk0NDkyNDM3OTwvVXNlcklEPg0KICAgICAgICA8T0VNPlRoaXMgaXMgYSByZWRpc3RyaWJ1dGFibGUgbGljZW5zZTwvT0VNPg0KICAgICAgICA8UHJvZHVjdHM+DQogICAgICAgICAgICA8UHJvZHVjdD5Bc3Bvc2UuVG90YWwgUHJvZHVjdCBGYW1pbHk8L1Byb2R1Y3Q+DQogICAgICAgIDwvUHJvZHVjdHM+DQogICAgICAgIDxFZGl0aW9uVHlwZT5FbnRlcnByaXNlPC9FZGl0aW9uVHlwZT4NCiAgICAgICAgPFNlcmlhbE51bWJlcj57RjJCOTcwNDUtMUIyOS00QjNGLUJENTMtNjAxRUZGQTE1QUE5fTwvU2VyaWFsTnVtYmVyPg0KICAgICAgICA8U3Vic2NyaXB0aW9uRXhwaXJ5PjIwOTkxMjMxPC9TdWJzY3JpcHRpb25FeHBpcnk+DQogICAgICAgIDxMaWNlbnNlVmVyc2lvbj4zLjA8L0xpY2Vuc2VWZXJzaW9uPg0KICAgIDwvRGF0YT4NCiAgICA8U2lnbmF0dXJlPlFYTndiM05sTGxSdmRHRnNMb1B5YjJSMVkzUWdSbUZ0YVd4NTwvU2lnbmF0dXJlPg0KPC9MaWNlbnNlPg==";

            var stream = new MemoryStream(Convert.FromBase64String(lData));

            stream.Seek(0, SeekOrigin.Begin);
            new Aspose.ThreeD.License().SetLicense(stream);

            var scene = new Scene(objFile);

            // create a camera for capturing the cube map
            var camera = new Camera(ProjectionType.Perspective)
                         {
                             NearPlane = 0.1,
                             FarPlane = 1000,
                             FieldOfView = 75,
                             LookAt = Vector3.Origin,
                             AspectRatio = (double) width / height
                         };
            scene.RootNode.CreateChildNode("camera", camera);
            camera.ParentNode.Transform.Translation = new Vector3(cameraPos.X, cameraPos.Y, cameraPos.Z);
            //camera.ParentNode.Transform.EulerAngles = new Vector3(cameraRotation.X, cameraRotation.Y, cameraRotation.Z);

            var material = new PhongMaterial()
                           {
                               Shininess = 9999999,
                           };

            // Initialize texture object
            using (var designTexture = new MagickImage(MagickColor.FromRgba(objectColor.R, objectColor.G, objectColor.B, objectColor.A), drawWidth, drawHeight))
            {
                if (design != null)
                {
                    designTexture.Composite(new MagickImage(CreateThumbnail(design, designWidth, designHeight, Color.Transparent, horizontalAlign, verticalAlign) as Bitmap),
                                            x, y, CompositeOperator.Over);
                }
                
                if (!string.IsNullOrEmpty(textureFile))
                {
                    var textureImage = new MagickImage(textureFile);
                    designTexture.Composite(textureImage, Gravity.Center, CompositeOperator.Multiply);
                }
                
                designTexture.Crop(x, y, designWidth, designHeight);

                if (textureWidth > 0 && textureHeight > 0)
                {
                    designTexture.Resize(textureWidth, textureHeight);
                }
                
                var buffer = new MemoryStream();
                designTexture.ToBitmap().Save(buffer, ImageFormat.Png);
                var texture = new Texture
                              {
                                  Content = buffer.ToArray(),
                              };

                // Set texture of the material
                material.SetTexture("DiffuseColor", texture);
            }

            scene.RootNode.ChildNodes[0].Material = material;
            
            // Add lights
            foreach (var light in lights)
            {
                switch (light.Type)
                {
                    case "hemisphere":
                        scene.RootNode.CreateChildNode("light", new Light
                                                                {
                                                                    Color = new Vector3(Color.FromArgb(light.Color)),
                                                                    LightType = LightType.Volume,
                                                                    LookAt = new Vector3(0, 0, 0),
                                                                    Intensity = light.Intensity,
                                                                    NearPlane = 0.1,
                                                                    FarPlane = 1000,
                                                                    Falloff = 90,
                                                                    ConstantAttenuation = 1.2,
                                                                    LinearAttenuation = 0
                                                                }).Transform.Translation = new Vector3(0, 1000, 0);
                        scene.RootNode.CreateChildNode("light", new Light
                                                                {
                                                                    Color = new Vector3(Color.FromArgb(light.BackgroundColor)),
                                                                    LookAt = new Vector3(0, 0, 0),
                                                                    LightType = LightType.Volume,
                                                                    Intensity = light.Intensity,
                                                                    NearPlane = 0.1,
                                                                    FarPlane = 1000,
                                                                    Falloff = 90,
                                                                    ConstantAttenuation = 3,
                                                                    LinearAttenuation = 0
                                                                }).Transform.Translation = new Vector3(0, -1000, 0);
                        break;
                    case "ambient":
                        scene.RootNode.CreateChildNode("light", new Light
                                                                {
                                                                    Color = new Vector3(Color.FromArgb(light.Color)),
                                                                    LookAt = new Vector3(0, 0, 0),
                                                                    Intensity = light.Intensity,
                                                                    NearPlane = 0.1,
                                                                    FarPlane = 1000,
                                                                    Falloff = 90,
                                                                    ConstantAttenuation = 1.2,
                                                                    LinearAttenuation = 0
                                                                }).Transform.Translation = new Vector3(0, 1000, 0);
                        break;
                    case "spot":
                        scene.RootNode.CreateChildNode("light", new Light
                                                                {
                                                                    Color = new Vector3(Color.FromArgb(light.Color)),
                                                                    LightType = LightType.Spot,
                                                                    LookAt = new Vector3(-4.3, 0, 0),
                                                                    Intensity = light.Intensity,
                                                                    Falloff = 45
                                                                }).Transform.Translation = new Vector3(light.Position.X, light.Position.Y, light.Position.Z);
                        break;
                    case "point":
                        scene.RootNode.CreateChildNode("light", new Light
                                                                {
                                                                    Color = new Vector3(Color.FromArgb(light.Color)),
                                                                    LightType = LightType.Point,
                                                                    LookAt = new Vector3(0, 0, 0),
                                                                    Intensity = light.Intensity,
                                                                    LinearAttenuation = 0.01
                                                                }).Transform.Translation = new Vector3(light.Position.X, light.Position.Y, light.Position.Z);
                        break;
                    case "directional":
                        scene.RootNode.CreateChildNode("light", new Light
                                                                {
                                                                    Color = new Vector3(Color.FromArgb(light.Color)),
                                                                    LightType = LightType.Directional,
                                                                    LookAt = new Vector3(0, 0, 0),
                                                                    Intensity = light.Intensity
                                                                }).Transform.Translation = new Vector3(light.Position.X, light.Position.Y, light.Position.Z);
                        break;
                }
            }

            using (var renderer = Renderer.CreateRenderer())
            {
                renderer.EnableShadows = false;
                
                var renderTexture = renderer.RenderFactory.CreateRenderTexture(new RenderParameters(false), 1, width, height);
                // This render target has one viewport to render, the viewport occupies the 100% width and 100% height
                var vp = renderTexture.CreateViewport(camera, new RelativeRectangle()
                                                              {
                                                                  ScaleWidth = 1,
                                                                  ScaleHeight = 1
                                                              });
                vp.BackgroundColor = Color.Transparent;

                // Render the target and save the target texture to external file
                renderer.Render(renderTexture);

                var output = new Bitmap(width, height);
                renderTexture.Targets[0].Save(output);
                
                return output;
            }
        }

        #region Helpers

        /// <summary>
        /// Gets coordinate of Bezier curve point by parameter 't'.
        /// </summary>
        /// <param name="t">Parameter [0..1]</param>
        /// <returns>Point of a curve.</returns>
        private static PointF GetQuadraticBezierPoint(float x1, float y1, float x2, float y2, float x3, float y3,
                                                      float x4, float y4, float t)
        {
            if (t < 0)
            {
                t = 0;
            }
            if (t > 1)
            {
                t = 1;
            }

            var cx = 3 * (x2 - x1);
            var bx = 3 * (x3 - x2) - cx;
            var ax = x4 - x1 - cx - bx;

            var cy = 3 * (y2 - y1);
            var by = 3 * (y3 - y2) - cy;
            var ay = y4 - y1 - cy - by;

            var tSquared = t * t;
            var tCubed = tSquared * t;
            var resultX = (ax * tCubed) + (bx * tSquared) + (cx * t) + x1;
            var resultY = (ay * tCubed) + (by * tSquared) + (cy * t) + y1;
            return new PointF(resultX, resultY);
        }

        /// <summary>
        /// Gets 1st derivate of a Bezier curve in a point specified by parameter t.
        /// </summary>
        /// <param name="t">Parameter [0..1].</param>
        /// <returns>Vector of 1st Bezier curve derivate in a specified point.</returns>
        private static PointF GetDerivate(float x1, float y1, float x2, float y2, float x3, float y3, float x4,
                                          float y4, float t)
        {
            if (t < 0)
            {
                t = 0;
            }
            if (t > 1)
            {
                t = 1;
            }

            var tSquared = t * t;
            var s0 = -3 + 6 * t - 3 * tSquared;
            var s1 = 3 - 12 * t + 9 * tSquared;
            var s2 = 6 * t - 9 * tSquared;
            var s3 = 3 * tSquared;
            var resultX = x1 * s0 + x2 * s1 + x3 * s2 + x4 * s3;
            var resultY = y1 * s0 + y2 * s1 + y3 * s2 + y4 * s3;
            return new PointF(resultX, resultY);
        }

        /// <summary>
        /// Gets 2nd derivate of a Bezier curve in a point specified by parameter t.
        /// </summary>
        /// <param name="t">Parameter [0..1].</param>
        /// <returns>Vector of 2nd Bezier curve derivate in a specified point.</returns>
        private static PointF GetSecondDerivate(float x1, float y1, float x2, float y2, float x3, float y3, float x4,
                                                float y4, float t)
        {
            if (t < 0)
            {
                t = 0;
            }
            if (t > 1)
            {
                t = 1;
            }

            var s0 = 6 - 6 * t;
            var s1 = -12 + 18 * t;
            var s2 = 6 - 18 * t;
            var s3 = 6 * t;
            var resultX = x1 * s0 + x2 * s1 + x3 * s2 + x4 * s3;
            var resultY = y1 * s0 + y2 * s1 + y3 * s2 + y4 * s3;
            return new PointF(resultX, resultY);
        }

        /// <summary>
        /// Gets curvature radius of a Bezier curve in specified point.
        /// </summary>
        /// <param name="t">Parameter [0..1].</param>
        /// <returns>Curvature radius.</returns>
        /// <remarks>Curvature = 1/CurvatureRadius.</remarks>
        private static double GetCurvatureRadius(float x1, float y1, float x2, float y2, float x3, float y3, float x4,
                                                 float y4, float t)
        {
            if (t < 0)
            {
                t = 0;
            }
            if (t > 1)
            {
                t = 1;
            }

            var d1 = GetDerivate(x1, y1, x2, y2, x3, y3, x4, y4, t);
            var d2 = GetSecondDerivate(x1, y1, x2, y2, x3, y3, x4, y4, t);

            var r1 = Math.Sqrt(Math.Pow(d1.X * d1.X + d1.Y * d1.Y, 3));
            var r2 = Math.Abs(d1.X * d2.Y - d2.X * d1.Y);
            return r1 / r2;
        }

        private static PointF DeCasteljau(PointF[] points, float t)
        {
            var temp = new List<PointF>();
            temp.AddRange(points);
            for (var i = 1; i < temp.Count; i++)
            {
                for (var j = 0; j < temp.Count - i; j++)
                {
                    temp[j] = new PointF((1 - t) * temp[j].X + t * temp[j + 1].X,
                                         (1 - t) * temp[j].Y + t * temp[j + 1].Y);
                }
            }
            return temp[0];
        }

        private static PointF DeCasteljau(PointF[][] points, float u, float v)
        {
            var q = new List<PointF>();
            for (var i = 0; i < points.Length; i++)
            {
                var rows = points[i];
                var point = DeCasteljau(rows, v);
                q.Add(point);
            }

            var result = DeCasteljau(q.ToArray(), u);
            return result;
        }

        private static void DrawDebugGrid(PointF p1, PointF p2, PointF p3, PointF p4, PointF p5, PointF p6, PointF p7,
                                          PointF p8, PointF p9, PointF p10, PointF p11, PointF p12, PointF p13,
                                          PointF p14, PointF p15, PointF p16)
        {
            var width = (int) Math.Ceiling(Math.Max(Math.Max(Math.Max(p4.X, p8.X), p12.X), p16.X));
            var height = (int) Math.Ceiling(Math.Max(Math.Max(Math.Max(p13.Y, p14.Y), p15.Y), p16.Y));
            var img = new Bitmap(width, height);
            var g = Graphics.FromImage(img);

            var pen = new Pen(Brushes.White, 3);

            g.DrawLine(pen, p1, p2);
            g.DrawLine(pen, p2, p3);
            g.DrawLine(pen, p3, p4);

            g.DrawLine(pen, p5, p6);
            g.DrawLine(pen, p6, p7);
            g.DrawLine(pen, p7, p8);

            g.DrawLine(pen, p9, p10);
            g.DrawLine(pen, p10, p11);
            g.DrawLine(pen, p11, p12);

            g.DrawLine(pen, p13, p14);
            g.DrawLine(pen, p14, p15);
            g.DrawLine(pen, p15, p16);

            g.DrawLine(pen, p1, p5);
            g.DrawLine(pen, p5, p9);
            g.DrawLine(pen, p9, p13);

            g.DrawLine(pen, p2, p6);
            g.DrawLine(pen, p6, p10);
            g.DrawLine(pen, p10, p14);

            g.DrawLine(pen, p3, p7);
            g.DrawLine(pen, p7, p11);
            g.DrawLine(pen, p11, p15);

            g.DrawLine(pen, p4, p8);
            g.DrawLine(pen, p8, p12);
            g.DrawLine(pen, p12, p16);

            img.Save(@"grid.png");
        }

        private static void DrawDebugGrid(PointF[][] points, PointF point00, PointF point10, PointF point01,
                                          PointF point11)
        {
            var width = (int) point10.X;
            var height = (int) point01.Y;
            var img = new Bitmap(width, height);
            var g = Graphics.FromImage(img);

            var pen = new Pen(Brushes.White, 1);

            for (var y = 0; y < height; y++)
            {
                var lastPoint = new PointF(float.NaN, float.NaN);
                var u = (float) y / height;
                for (var x = 0; x < width; x++)
                {
                    var v = (float) x / width;
                    var newPoint = DeCasteljau(points, u, v);
                    if (float.IsNaN(lastPoint.X) || float.IsNaN(lastPoint.Y))
                    {
                        lastPoint = newPoint;
                    }
                    else
                    {
                        g.DrawLine(pen, lastPoint, newPoint);
                    }
                }
            }

            img.Save(@"grid.png");
        }

        private static PointF GetPointOnLine(PointF p1, PointF p2, float t)
        {
            var x = (1 - t) * p1.X + t * p2.X;
            var y = (1 - t) * p1.Y + t * p2.Y;
            return new PointF(x, y);
        }

        private static RectangleF Warp(MagickImage image, PointF[][] points)
        {
            var boundingBox = new RectangleF(float.MaxValue, float.MaxValue, 0, 0);

            var pixels = Array.ConvertAll(image.GetPixels().ToByteArray("RGBA"), a => (short) a);
            var newPixels = new short[pixels.Length];

            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    newPixels[4 * (x + y * image.Width)] = image.BackgroundColor.R;
                    newPixels[4 * (x + y * image.Width) + 1] = image.BackgroundColor.G;
                    newPixels[4 * (x + y * image.Width) + 2] = image.BackgroundColor.B;
                    newPixels[4 * (x + y * image.Width) + 3] = -1;
                }
            }

            //PointF[] lastRow = null;
            for (var y = 0; y < image.Height; y++)
            {
                var u = (float) y / image.Height;
                for (var x = 0; x < image.Width; x++)
                {
                    var v = (float) x / image.Width;
                    var newPoint = DeCasteljau(points, u, v);
                    var newX = (int) Math.Round(newPoint.X);
                    var newY = (int) Math.Round(newPoint.Y);
                    if (newX >= 0 && newX < image.Width && newY >= 0 && newY < image.Height)
                    {
                        newPixels[4 * (newX + newY * image.Width)] = pixels[4 * (x + y * image.Width)];
                        newPixels[4 * (newX + newY * image.Width) + 1] = pixels[4 * (x + y * image.Width) + 1];
                        newPixels[4 * (newX + newY * image.Width) + 2] = pixels[4 * (x + y * image.Width) + 2];
                        newPixels[4 * (newX + newY * image.Width) + 3] = pixels[4 * (x + y * image.Width) + 3];
                    }

                    if (newX < boundingBox.X)
                    {
                        boundingBox.X = newX;
                    }
                    if (newY < boundingBox.Y)
                    {
                        boundingBox.Y = newY;
                    }
                    if (newX > boundingBox.Right)
                    {
                        boundingBox.Width = newX - boundingBox.X + 1;
                    }
                    if (newY > boundingBox.Bottom)
                    {
                        boundingBox.Height = newY - boundingBox.Y + 1;
                    }
                }
            }

            Array.Copy(newPixels, pixels, newPixels.Length);
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    if (newPixels[4 * (x + y * image.Width) + 3] == -1)
                    {
                        // Compute color
                        var r = 0;
                        var g = 0;
                        var b = 0;
                        var a = 0;
                        var count = 0;
                        if (y - 1 >= 0 && pixels[4 * (x + (y - 1) * image.Width) + 3] != -1)
                        {
                            ++count;
                            r += pixels[4 * (x + (y - 1) * image.Width)];
                            g += pixels[4 * (x + (y - 1) * image.Width) + 1];
                            b += pixels[4 * (x + (y - 1) * image.Width) + 2];
                            a += pixels[4 * (x + (y - 1) * image.Width) + 3];
                        }
                        if (x - 1 >= 0 && pixels[4 * (x - 1 + y * image.Width) + 3] != -1)
                        {
                            ++count;
                            r += pixels[4 * (x - 1 + y * image.Width)];
                            g += pixels[4 * (x - 1 + y * image.Width) + 1];
                            b += pixels[4 * (x - 1 + y * image.Width) + 2];
                            a += pixels[4 * (x - 1 + y * image.Width) + 3];
                        }
                        if (x + 1 < image.Width && pixels[4 * (x + 1 + y * image.Width) + 3] != -1)
                        {
                            ++count;
                            r += pixels[4 * (x + 1 + y * image.Width)];
                            g += pixels[4 * (x + 1 + y * image.Width) + 1];
                            b += pixels[4 * (x + 1 + y * image.Width) + 2];
                            a += pixels[4 * (x + 1 + y * image.Width) + 3];
                        }
                        if (y + 1 < image.Height && pixels[4 * (x + (y + 1) * image.Width) + 3] != -1)
                        {
                            ++count;
                            r += pixels[4 * (x + (y + 1) * image.Width)];
                            g += pixels[4 * (x + (y + 1) * image.Width) + 1];
                            b += pixels[4 * (x + (y + 1) * image.Width) + 2];
                            a += pixels[4 * (x + (y + 1) * image.Width) + 3];
                        }
                        if (count > 0)
                        {
                            newPixels[4 * (x + y * image.Width)] = (byte) (r / count);
                            newPixels[4 * (x + y * image.Width) + 1] = (byte) (g / count);
                            newPixels[4 * (x + y * image.Width) + 2] = (byte) (b / count);
                            newPixels[4 * (x + y * image.Width) + 3] = (byte) (a / count);
                        }
                        else
                        {
                            newPixels[4 * (x + y * image.Width) + 3] = image.BackgroundColor.A;
                        }
                    }
                }
            }

            image.GetPixels().Set(newPixels.Select(x => (byte) x).ToArray());

            return boundingBox;
        }

        private static bool PointInPolygon(PointF p, PointF[] polygon)
        {
            var inside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if ((polygon[i].Y > p.Y) != (polygon[j].Y > p.Y) &&
                    p.X < (polygon[j].X - polygon[i].X) * (p.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) +
                    polygon[i].X)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        private static Rectangle FindBoundingBox(MagickImage image)
        {
            var pixels = image.GetPixels().ToByteArray("A");
            var bound = new Rectangle(int.MaxValue, int.MaxValue, 0, 0);

            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    if (pixels[(x + y * image.Width)] > 0)
                    {
                        if (x < bound.X)
                        {
                            bound.X = x;
                        }
                        if (y < bound.Y)
                        {
                            bound.Y = y;
                        }
                        if (x > bound.Right)
                        {
                            bound.Width = x - bound.X + 1;
                        }
                        if (y > bound.Bottom)
                        {
                            bound.Height = y - bound.Y + 1;
                        }
                    }
                }
            }

            pixels = null;
            GC.Collect();
            GC.WaitForFullGCComplete();

            return bound;
        }

        #endregion
    }
}
