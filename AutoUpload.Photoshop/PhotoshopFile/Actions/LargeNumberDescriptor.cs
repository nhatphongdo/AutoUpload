namespace PhotoshopFile.Actions
{
    public class LargeNumberDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public long Value { get; private set; }

        public LargeNumberDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;
            Value = reader.ReadInt64();
        }
    }
}
