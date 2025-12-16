namespace PhotoshopFile.Actions
{
    public class DoubleDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public double Value { get; private set; }

        public DoubleDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;
            Value = reader.ReadDouble();
        }
    }
}
