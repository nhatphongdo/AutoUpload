namespace PhotoshopFile.Actions
{
    public class BooleanDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public bool Value { get; private set; }

        public BooleanDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;
            Value = reader.ReadBoolean();
        }
    }
}
