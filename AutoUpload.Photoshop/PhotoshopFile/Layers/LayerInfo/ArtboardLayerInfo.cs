using PhotoshopFile.Actions;

namespace PhotoshopFile
{
    public class ArtboardLayerInfo : LayerInfo
    {
        public override string Signature
        {
            get { return "8BIM"; }
        }

        public override string Key
        {
            get { return "artb"; }
        }

        public DescriptorStructure Descriptor { get; private set; }

        public ArtboardLayerInfo(PsdBinaryReader reader)
        {
            var version = reader.ReadInt32();
            Descriptor = new DescriptorStructure(reader);
        }

        protected override void WriteData(PsdBinaryWriter writer)
        {
            //writer.Write(16);
        }
    }
}
