namespace PhotoshopFile.Actions
{
    public class EnumDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public string TypeId { get; set; }

        public string Enum { get; set; }

        public EnumDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;
            var typeLength = reader.ReadInt32();
            TypeId = typeLength == 0 ? reader.ReadAsciiChars(4) : reader.ReadAsciiChars(typeLength);
            var enumLength = reader.ReadInt32();
            Enum = enumLength == 0 ? reader.ReadAsciiChars(4) : reader.ReadAsciiChars(enumLength);
        }
    }
}
