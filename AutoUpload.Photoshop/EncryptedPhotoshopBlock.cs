using System;
using System.Collections.Generic;

namespace AutoUpload.Photoshop
{
    public enum BlockTypes
    {
        Normal,
        Compressed,
        Encoded
    }

    public class EncryptedPhotoshopBlock
    {
        public int BlockType
        {
            get; set;
        }

        public int BlockOffset
        {
            get; set;
        }

        public int BlockSize
        {
            get; set;
        }

        public string Key
        {
            get; set;
        }

        public string InitialVector
        {
            get; set;
        }

        public EncryptedPhotoshopBlock()
        {
        }

        public EncryptedPhotoshopBlock(byte[] buffer, ref int offset)
        {
            BlockType = buffer.ToInt(ref offset);
            BlockOffset = buffer.ToInt(ref offset);
            BlockSize = buffer.ToInt(ref offset);
            var size = 0;
            Key = buffer.ToString(ref offset, out size);
            InitialVector = buffer.ToString(ref offset, out size);
        }

        public byte[] ToBytes()
        {
            return BlockType.ToBytes()
                .Concat(BlockOffset.ToBytes())
                .Concat(BlockSize.ToBytes())
                .Concat(Key.ToBytes())
                .Concat(InitialVector.ToBytes());
        }
    }
}
