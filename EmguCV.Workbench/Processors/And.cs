using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class And : ImageProcessor
    {
        public And()
        {
            Color = 255;
        }

        private byte _color;
        [Category("And")]
        [PropertyOrder(0)]
        [DisplayName(@"Color")]
        [Description(@"The color for the AND operation.")]
        [DefaultValue(255)]
        public byte Color
        {
            get { return _color; }
            set { Set(ref _color, value); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = image.And(new Gray(_color));
        }
    }
}
