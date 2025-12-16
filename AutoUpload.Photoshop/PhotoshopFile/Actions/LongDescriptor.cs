namespace PhotoshopFile.Actions
{
    public class LongDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public int Value { get; private set; }

        public LongDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;
            Value = reader.ReadInt32();
        }
    }
}
