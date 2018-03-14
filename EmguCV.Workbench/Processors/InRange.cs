using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class InRange : ImageProcessor
    {
        public InRange()
        {
            Lower = 85;
            Higher = 170;
        }

        private byte _lower;
        [Category("In Range")]
        [PropertyOrder(0)]
        [DisplayName(@"Lower")]
        [Description(@"The inclusive lower limit of color value.")]
        [DefaultValue(85)]
        public byte Lower
        {
            get { return _lower; }
            set { Set(ref _lower, value); }
        }

        private byte _higher;
        [Category("In Range")]
        [PropertyOrder(1)]
        [DisplayName(@"Higher")]
        [Description(@"The inclusive upper limit of color value.")]
        [DefaultValue(170)]
        public byte Higher
        {
            get { return _higher; }
            set { Set(ref _higher, value); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = image.InRange(new Gray(_lower), new Gray(_higher));
        }
    }
}
