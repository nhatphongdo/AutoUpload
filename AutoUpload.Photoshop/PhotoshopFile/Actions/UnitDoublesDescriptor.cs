using System;
using System.Collections.Generic;

namespace PhotoshopFile.Actions
{
    public class UnitDoublesDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public string UnitType { get; private set; }

        public List<double> Values { get; private set; }

        public UnitDoublesDescriptor(PsdBinaryReader reader, string itemKey, string itemType, int numberOfObjects = 0)
        {
            ItemKey = itemKey;
            ItemType = itemType;
            UnitType = reader.ReadAsciiChars(4);
            var count = reader.ReadInt32();
            if (numberOfObjects > 0 && count != numberOfObjects)
            {
                throw new Exception("Inconsistent object count!");
            }

            Values = new List<double>();
            for (var i = 0; i < count; i++)
            {
                Values.Add(reader.ReadDouble());
            }
        }
    }
}
