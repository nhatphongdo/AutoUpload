using System.Collections.Generic;

namespace PhotoshopFile.Actions
{
    public class ReferenceDescriptor : IDescriptorStructure
    {
        public string ItemKey { get; set; }

        public string ItemType { get; set; }

        public List<IDescriptorStructure> References { get; private set; }

        public ReferenceDescriptor(PsdBinaryReader reader, string itemKey, string itemType)
        {
            ItemKey = itemKey;
            ItemType = itemType;

            References = new List<IDescriptorStructure>();
            var numberOfItems = reader.ReadInt32();
            for (var i = 0; i < numberOfItems; i++)
            {
                var type = reader.ReadAsciiChars(4);
                switch (type)
                {
                    case "prop":
                        References.Add(new PropertyDescriptor(reader, "", ""));
                        break;
                    case "Clss":
                        References.Add(new ClassDescriptor(reader, "", ""));
                        break;
                    case "Enmr":
                        References.Add(new EnumReferenceDescriptor(reader, "", ""));
                        break;
                    case "rele":
                        References.Add(new OffsetDescriptor(reader, "", ""));
                        break;
                    case "Idnt":
                        References.Add(new UnsignedIntegerDescriptor(reader, "", ""));
                        break;
                    case "indx":
                        References.Add(new UnsignedIntegerDescriptor(reader, "", ""));
                        break;
                    case "name":
                        References.Add(new StringDescriptor(reader, "", ""));
                        break;
                }
            }
        }
    }
}
