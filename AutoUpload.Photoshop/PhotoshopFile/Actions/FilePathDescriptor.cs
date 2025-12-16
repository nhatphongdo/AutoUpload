namespace PhotoshopFile.Actions
{
    public class FilePathDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }
        
        public string FilePath { get; private set; }
        
        public FilePathDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;

            var length = reader.ReadInt32();
            var signature = reader.ReadAsciiChars(4);
            var anotherLength = reader.ReadInt32();
            FilePath = reader.ReadUnicodeString();
        }
    }
}
