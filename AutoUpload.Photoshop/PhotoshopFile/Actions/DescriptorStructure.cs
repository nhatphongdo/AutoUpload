using System;
using System.Collections.Generic;

namespace PhotoshopFile.Actions
{
    public class DescriptorStructure : IDescriptorStructure
    {
        public string ClassIdName { get; private set; }

        public string ClassIdKey { get; private set; }

        public List<IDescriptorStructure> Structures { get; private set; }

        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public DescriptorStructure(PsdBinaryReader reader, string key = "", string type = "")
        {
            ItemKey = key;
            ItemType = type;

            ClassIdName = reader.ReadUnicodeString();
            var classIdLength = reader.ReadInt32();
            ClassIdKey = classIdLength == 0 ? reader.ReadAsciiChars(4) : reader.ReadAsciiChars(classIdLength);

            Structures = new List<IDescriptorStructure>();
            var numberOfItems = reader.ReadInt32();
            for (var i = 0; i < numberOfItems; i++)
            {
                Structures.Add(ReadDescriptor(reader));
            }
        }

        public static IDescriptorStructure ReadDescriptor(PsdBinaryReader reader, bool ignoreKey = false)
        {
            var itemKey = "";
            if (!ignoreKey)
            {
                var itemKeyLength = reader.ReadInt32();
                itemKey = itemKeyLength == 0 ? reader.ReadAsciiChars(4) : reader.ReadAsciiChars(itemKeyLength);
            }

            var itemType = reader.ReadAsciiChars(4);
            switch (itemType)
            {
                case "TEXT":
                    return new StringDescriptor(reader, itemKey, itemType);
                case "long":
                    return new LongDescriptor(reader, itemKey, itemType);
                case "obj ":
                    return new ReferenceDescriptor(reader, itemKey, itemType);
                case "Objc":
                case "GlbO":
                    return new DescriptorStructure(reader, itemKey, itemType);
                case "VlLs":
                    return new ListDescriptor(reader, itemKey, itemType);
                case "doub":
                    return new DoubleDescriptor(reader, itemKey, itemType);
                case "UntF":
                    return new UnitDoubleDescriptor(reader, itemKey, itemType);
                case "UnFl":
                    return new UnitFloatDescriptor(reader, itemKey, itemType);
                case "enum":
                    return new EnumDescriptor(reader, itemKey, itemType);
                case "comp":
                    return new LargeNumberDescriptor(reader, itemKey, itemType);
                case "bool":
                    return new BooleanDescriptor(reader, itemKey, itemType);
                case "type":
                case "GlbC":
                    return new ClassDescriptor(reader, itemKey, itemType);
                case "alis":
                    return new AliasDescriptor(reader, itemKey, itemType);
                case "Pth ":
                    //return new FilePathDescriptor(reader, itemKey, itemType);
                    return new RawDescriptor(reader, itemKey, itemType);
                case "ObAr":
                    return new ObjectArrayDescriptor(reader, itemKey, itemType);
                case "tdta":
                    return new RawDescriptor(reader, itemKey, itemType);
                default:
                    throw new Exception("Unrecognized item type!");
            }
        }
    }
}
