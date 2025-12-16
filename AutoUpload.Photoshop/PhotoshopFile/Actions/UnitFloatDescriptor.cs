namespace PhotoshopFile.Actions
{
    public class UnitFloatDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public string UnitType { get; private set; }

        public float Value { get; private set; }

        public UnitFloatDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;
            UnitType = reader.ReadAsciiChars(4);
            Value = reader.ReadFloat();
        }
    }
}
