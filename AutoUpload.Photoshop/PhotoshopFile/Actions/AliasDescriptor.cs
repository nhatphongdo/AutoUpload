namespace PhotoshopFile.Actions
{
    public class AliasDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public int Length { get; private set; }

        public byte[] Data { get; private set; }

        public AliasDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;
            Length = reader.ReadInt32();
            Data = reader.ReadBytes(Length);
        }
    }
}
