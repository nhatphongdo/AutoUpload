namespace PhotoshopFile.Actions
{
    public class OffsetDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public string Name { get; private set; }

        public string ClassId { get; private set; }

        public int Value { get; private set; }

        public OffsetDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;
            Name = reader.ReadUnicodeString();
            var classLength = reader.ReadInt32();
            ClassId = classLength == 0 ? reader.ReadAsciiChars(4) : reader.ReadAsciiChars(classLength);
            Value = reader.ReadInt32();
        }
    }
}
