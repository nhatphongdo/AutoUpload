/////////////////////////////////////////////////////////////////////////////////
//
// Photoshop PSD FileType Plugin for Paint.NET
// http://psdplugin.codeplex.com/
//
// This software is provided under the MIT License:
//   Copyright (c) 2006-2007 Frank Blumenberg
//   Copyright (c) 2010-2014 Tao Yue
//
// See LICENSE.txt for complete licensing and attribution information.
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using PhotoshopFile.Actions;

namespace PhotoshopFile
{
    public class PlacedLayerInfo : LayerInfo
    {
        public override string Signature
        {
            get { return "8BIM"; }
        }

        public override string Key
        {
            get { return "PlLd"; }
        }

        public int Version { get; private set; }

        public byte[] Data { get; private set; }

        public string UniqueId { get; private set; }

        public int PageNumber { get; private set; }

        public int TotalPages { get; private set; }

        public int AntiAliasPolicy { get; private set; }

        public int PlacedLayerType { get; private set; }

        public double[] Transformation { get; private set; }

        public int WarpVersion { get; private set; }

        public int WarpDescriptorVersion { get; private set; }

        public IDescriptorStructure DescriptorStructure { get; private set; }

        public PlacedLayerInfo(PsdBinaryReader reader, long dataLength)
        {
            var position = reader.BaseStream.Position;

            var key = reader.ReadAsciiChars(4);
            Version = reader.ReadInt32();
            UniqueId = reader.ReadPascalString(1);
            PageNumber = reader.ReadInt32();
            TotalPages = reader.ReadInt32();
            AntiAliasPolicy = reader.ReadInt32();
            PlacedLayerType = reader.ReadInt32();
            Transformation = new double[8];
            for (var i = 0; i < Transformation.Length; i++)
            {
                Transformation[i] = reader.ReadDouble();
            }
            WarpVersion = reader.ReadInt32();
            WarpDescriptorVersion = reader.ReadInt32();

            DescriptorStructure = new DescriptorStructure(reader);

            reader.BaseStream.Position = position;
            Data = reader.ReadBytes((int) dataLength);
        }

        protected override void WriteData(PsdBinaryWriter writer)
        {
            writer.Write(Data);
        }
    }
}
