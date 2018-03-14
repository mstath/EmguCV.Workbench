using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Xor : ImageProcessor
    {
        public Xor()
        {
            Color = 127;
        }
        private byte _color;
        [Category("Xor")]
        [PropertyOrder(0)]
        [DisplayName(@"Color")]
        [Description(@"The value for the XOR operation.")]
        [DefaultValue(127)]
        public byte Color
        {
            get { return _color; }
            set { Set(ref _color, value); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = image.Xor(new Gray(_color));
        }
    }
}
