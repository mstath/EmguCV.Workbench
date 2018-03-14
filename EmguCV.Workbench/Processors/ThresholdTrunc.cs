using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class ThresholdTrunc : ImageProcessor
    {
        public ThresholdTrunc()
        {
            Threshold = 55;
        }

        private byte _threshold;
        [Category("Threshold Trunc")]
        [PropertyOrder(0)]
        [DisplayName(@"Threshold")]
        [Description(@"The threshold value.")]
        [DefaultValue(55)]
        public byte Threshold
        {
            get { return _threshold; }
            set { Set(ref _threshold, value); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = image.ThresholdTrunc(new Gray(_threshold));
        }
    }
}
