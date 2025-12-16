using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AutoUpload.Photoshop
{
    public class EncryptedPhotoshopHeader
    {
        public string Signature
        {
            get { return "TIPS"; }
        }

        public int Version
        {
            get; set;
        }

        public int HeaderSize
        {
            get; set;
        }

        public int NumberOfBlocks
        {
            get; set;
        }

        public List<EncryptedPhotoshopBlock> Blocks
        {
            get; set;
        }

        public string Checksum
        {
            get; set;
        }

        public int DesignLayer
        {
            get; set;
        }

        public int ShirtColorLayer
        {
            get; set;
        }

        public int ShirtTextureLayer
        {
            get; set;
        }

        public int LaceColorLayer
        {
            get; set;
        }

        public Rectangle DesignBound
        {
            get; set;
        }

        public EncryptedPhotoshopHeader()
        {
            Version = 1;
            Blocks = new List<EncryptedPhotoshopBlock>();
            DesignLayer = -1;
            ShirtColorLayer = -1;
            ShirtTextureLayer = -1;
            LaceColorLayer = -1;
            DesignBound = new Rectangle();
        }

        public EncryptedPhotoshopHeader(byte[] buffer, int offset)
            : this()
        {
            var size = 0;
            var signature = buffer.ToString(ref offset, out size);
            if (signature != Signature)
            {
                throw new Exception("Wrong file format");
            }

            Version = buffer.ToInt(ref offset);
            HeaderSize = buffer.ToInt(ref offset);
            NumberOfBlocks = buffer.ToInt(ref offset);
            for (var i = 0; i < NumberOfBlocks; i++)
            {
                var block = new EncryptedPhotoshopBlock(buffer, ref offset);
                Blocks.Add(block);
            }
            Checksum = buffer.ToString(ref offset, out size);

            DesignLayer = buffer.ToInt(ref offset);
            ShirtColorLayer = buffer.ToInt(ref offset);
            ShirtTextureLayer = buffer.ToInt(ref offset);
            LaceColorLayer = buffer.ToInt(ref offset);
            var x = buffer.ToInt(ref offset);
            var y = buffer.ToInt(ref offset);
            var width = buffer.ToInt(ref offset);
            var height = buffer.ToInt(ref offset);
            DesignBound = new Rectangle(x, y, width, height);
        }

        public byte[] ToBytes()
        {
            NumberOfBlocks = Blocks.Count;

            var buffer = Signature.ToBytes()
                .Concat(Version.ToBytes())
                .Concat(HeaderSize.ToBytes())
                .Concat(NumberOfBlocks.ToBytes());
            foreach (var block in Blocks)
            {
                buffer = buffer.Concat(block.ToBytes());
            }
            buffer = buffer.Concat(Checksum.ToBytes())
                .Concat(DesignLayer.ToBytes())
                .Concat(ShirtColorLayer.ToBytes())
                .Concat(ShirtTextureLayer.ToBytes())
                .Concat(LaceColorLayer.ToBytes())
                .Concat(DesignBound.X.ToBytes())
                .Concat(DesignBound.Y.ToBytes())
                .Concat(DesignBound.Width.ToBytes())
                .Concat(DesignBound.Height.ToBytes());

            // Overwrite size
            var sizeByte = buffer.Length.ToBytes();
            Array.Copy(sizeByte, 0, buffer, Signature.ToBytes().Length + 4, sizeByte.Length);

            return buffer;
        }
    }
}
