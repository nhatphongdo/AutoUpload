using System;
using System.Collections.Generic;

namespace PhotoshopFile.Actions
{
    public class ObjectArrayDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public string Name { get; private set; }

        public string ClassId { get; private set; }

        public List<IDescriptorStructure> Items { get; private set; }

        public ObjectArrayDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;

            var numberOfObjects = reader.ReadInt32();
            Name = reader.ReadUnicodeString();
            var classLength = reader.ReadInt32();
            ClassId = classLength == 0 ? reader.ReadAsciiChars(4) : reader.ReadAsciiChars(classLength);
            var numberOfItems = reader.ReadInt32();

            Items = new List<IDescriptorStructure>();
            for (var i = 0; i < numberOfItems; i++)
            {
                var objectKeyLength = reader.ReadInt32();
                var objectKey = objectKeyLength == 0 ? reader.ReadAsciiChars(4) : reader.ReadAsciiChars(objectKeyLength);
                var objectType = reader.ReadAsciiChars(4);
                switch (objectType)
                {
                    case "UnFl":
                        Items.Add(new UnitDoublesDescriptor(reader, objectKey, objectType, numberOfObjects));
                        break;
                    default:
                        throw new Exception("Unrecognized item type!");
                }
            }
        }
    }
}
