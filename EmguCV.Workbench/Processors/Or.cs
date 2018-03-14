using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Or : ImageProcessor
    {
        public Or()
        {
            Color = 127;
        }
        private byte _color;
        [Category("Or")]
        [PropertyOrder(0)]
        [DisplayName(@"Color")]
        [Description(@"The value for the OR operation.")]
        [DefaultValue(127)]
        public byte Color
        {
            get { return _color; }
            set { Set(ref _color, value); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = image.Or(new Gray(_color));
        }
    }
}
