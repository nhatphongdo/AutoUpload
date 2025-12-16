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
    public class LayerSmartObject : LayerInfo
    {
        public override string Signature
        {
            get { return "8BIM"; }
        }

        public override string Key
        {
            get { return "SoLd"; }
        }

        public int Version { get; private set; }

        public byte[] Data { get; private set; }

        public int DescriptorVersion { get; private set; }

        public IDescriptorStructure DescriptorStructure { get; private set; }

        public LayerSmartObject(PsdBinaryReader reader, long dataLength)
        {
            var position = reader.BaseStream.Position;

            var key = reader.ReadAsciiChars(4);
            Version = reader.ReadInt32();
            DescriptorVersion = reader.ReadInt32();

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
