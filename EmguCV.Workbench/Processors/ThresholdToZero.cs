using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class ThresholdToZero : ImageProcessor
    {
        public ThresholdToZero()
        {
            Threshold = 127;
            Invert = false;
        }

        private byte _threshold;
        [Category("Threshold To Zero")]
        [PropertyOrder(0)]
        [DisplayName(@"Threshold")]
        [Description(@"The threshold value.")]
        [DefaultValue(125)]
        public byte Threshold
        {
            get { return _threshold; }
            set { Set(ref _threshold, value); }
        }

        private bool _invert;
        [Category("Threshold To Zero")]
        [PropertyOrder(1)]
        [DisplayName(@"Invert")]
        [Description(@"Select to invert threshold.")]
        [DefaultValue(false)]
        public bool Invert
        {
            get { return _invert; }
            set { Set(ref _invert, value); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = !_invert
                ? image.ThresholdToZero(new Gray(_threshold))
                : image.ThresholdToZeroInv(new Gray(_threshold));
        }
    }
}
