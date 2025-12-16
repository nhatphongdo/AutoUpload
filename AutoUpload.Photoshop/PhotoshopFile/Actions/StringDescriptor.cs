namespace PhotoshopFile.Actions
{
    public class StringDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public string Text { get; private set; }

        public StringDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;
            Text = reader.ReadUnicodeString();
        }
    }
}
