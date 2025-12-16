using System.Collections.Generic;

namespace PhotoshopFile.Actions
{
    public class ListDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public List<IDescriptorStructure> Items { get; private set; }

        public ListDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;

            Items = new List<IDescriptorStructure>();
            var numberOfItems = reader.ReadInt32();
            for (var i = 0; i < numberOfItems; i++)
            {
                Items.Add(DescriptorStructure.ReadDescriptor(reader, true));
            }
        }
    }
}
