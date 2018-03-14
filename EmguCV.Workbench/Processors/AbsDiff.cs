using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class AbsDiff : ImageProcessor
    {
        private byte _color;
        [Category("Abs Diff")]
        [PropertyOrder(0)]
        [DisplayName(@"Color")]
        [Description(@"The color to compute absolute different with.")]
        [DefaultValue(0)]
        public byte Color
        {
            get { return _color; }
            set { Set(ref _color, value); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = image.AbsDiff(new Gray(_color));
        }
    }
}
