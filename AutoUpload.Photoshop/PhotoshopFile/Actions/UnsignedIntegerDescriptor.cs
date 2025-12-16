namespace PhotoshopFile.Actions
{
    public class UnsignedIntegerDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public uint Value { get; private set; }

        public UnsignedIntegerDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;
            Value = reader.ReadUInt32();
        }
    }
}
