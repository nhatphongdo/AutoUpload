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
    public class LinkedLayer : LayerInfo
    {
        public override string Signature
        {
            get { return "8BIM"; }
        }

        public override string Key
        {
            get { return "lnkD"; }
        }

        public string Type { get; private set; }

        public int Version { get; private set; }

        public string Id { get; private set; }

        public string OriginalFileName { get; private set; }

        public string FileType { get; private set; }

        public string FileCreator { get; private set; }

        public IDescriptorStructure FileOpenDescriptor { get; private set; }

        public IDescriptorStructure LinkedFileDescriptor { get; private set; }

        public int Year { get; private set; }

        public byte Month { get; private set; }

        public byte Day { get; private set; }

        public byte Hour { get; private set; }

        public byte Minute { get; private set; }

        public double Second { get; private set; }

        public long FileSize { get; private set; }

        public byte[] FileData { get; private set; }

        public PsdFile LinkedFile { get; private set; }

        public string ChildDocumentId { get; private set; }

        public double AssetModTime { get; private set; }

        public byte AssetLockedState { get; private set; }

        public byte[] Data { get; private set; }

        public LinkedLayer(PsdBinaryReader reader, LoadContext loadContext)
        {
            var position = reader.BaseStream.Position;

            var dataLength = reader.ReadInt64();
            Type = reader.ReadAsciiChars(4);
            Version = reader.ReadInt32();
            Id = reader.ReadPascalString(1);
            OriginalFileName = reader.ReadUnicodeString();
            FileType = reader.ReadAsciiChars(4);
            FileCreator = reader.ReadAsciiChars(4);
            var length = reader.ReadInt64();
            var fileOpenDescriptorExists = reader.ReadByte();
            if (fileOpenDescriptorExists == 1)
            {
                var undocumented = reader.ReadInt32();
                FileOpenDescriptor = new DescriptorStructure(reader);
            }
            if (Type == "liFE")
            {
                LinkedFileDescriptor = new DescriptorStructure(reader);
                if (Version > 3)
                {
                    Year = reader.ReadInt32();
                    Month = reader.ReadByte();
                    Day = reader.ReadByte();
                    Hour = reader.ReadByte();
                    Minute = reader.ReadByte();
                    Second = reader.ReadDouble();
                }
            }
            if (Type == "liFE")
            {
                FileSize = reader.ReadInt64();
            }
            if (Type == "liFA")
            {
                reader.ReadInt64();
            }
            if (Type == "liFD")
            {
                var curPos = reader.BaseStream.Position;
                var fileType = reader.ReadAsciiChars(4);
                reader.BaseStream.Position = curPos;
                if (fileType == "8BPS")
                {
                    LinkedFile = new PsdFile(reader, loadContext);
                }
                else
                {
                    FileData = reader.ReadBytes((int)length);
                }
            }
            //if (Version >= 5)
            //{
            //    ChildDocumentId = reader.ReadUnicodeString();
            //}
            //if (Version >= 6)
            //{
            //    AssetModTime = reader.ReadDouble();
            //}
            //if (Version >= 7)
            //{
            //    AssetLockedState = reader.ReadByte();
            //}

            reader.BaseStream.Position = position;
            Data = reader.ReadBytes((int) dataLength);
        }

        protected override void WriteData(PsdBinaryWriter writer)
        {
            writer.Write(Data);
        }
    }
}
