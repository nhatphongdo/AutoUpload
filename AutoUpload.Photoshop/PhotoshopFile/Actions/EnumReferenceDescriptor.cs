namespace PhotoshopFile.Actions
{
    public class EnumReferenceDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public string Name { get; private set; }

        public string ClassId { get; private set; }

        public string TypeId { get; set; }

        public string Enum { get; set; }

        public EnumReferenceDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;
            Name = reader.ReadUnicodeString();
            var classLength = reader.ReadInt32();
            ClassId = classLength == 0 ? reader.ReadAsciiChars(4) : reader.ReadAsciiChars(classLength);
            var typeLength = reader.ReadInt32();
            TypeId = typeLength == 0 ? reader.ReadAsciiChars(4) : reader.ReadAsciiChars(typeLength);
            var enumLength = reader.ReadInt32();
            Enum = enumLength == 0 ? reader.ReadAsciiChars(4) : reader.ReadAsciiChars(enumLength);
        }
    }
}
