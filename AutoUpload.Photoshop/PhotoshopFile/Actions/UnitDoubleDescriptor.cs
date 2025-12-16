namespace PhotoshopFile.Actions
{
    public class UnitDoubleDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public string UnitType { get; private set; }

        public double Value { get; private set; }

        public UnitDoubleDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;
            UnitType = reader.ReadAsciiChars(4);
            Value = reader.ReadDouble();
        }
    }
}
