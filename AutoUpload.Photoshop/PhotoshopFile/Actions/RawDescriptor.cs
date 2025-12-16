namespace PhotoshopFile.Actions
{
    public class RawDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public byte[] Data { get; private set; }

        public RawDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;

            var length = reader.ReadInt32();
            Data = reader.ReadBytes(length);
        }
    }
}
