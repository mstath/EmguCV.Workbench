using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class BinaryThreshold : ImageProcessor
    {
        public BinaryThreshold()
        {
            Threshold = 125;
            MaxValue = 255;
            Invert = false;
        }

        private byte _threshold;
        [Category("Binary Threshold")]
        [PropertyOrder(0)]
        [DisplayName(@"Threshold")]
        [Description(@"The threshold value.")]
        [DefaultValue(125)]
        public byte Threshold
        {
            get { return _threshold; }
            set { Set(ref _threshold, value); }
        }

        private byte _maxValue;
        [Category("Binary Threshold")]
        [PropertyOrder(1)]
        [DisplayName(@"Max Value")]
        [Description(@"The maximum value at threshold.")]
        [DefaultValue(255)]
        public byte MaxValue
        {
            get { return _maxValue; }
            set { Set(ref _maxValue, value); }
        }

        private bool _invert;
        [Category("Binary Threshold")]
        [PropertyOrder(2)]
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
            image = _invert
                ? image.ThresholdBinary(new Gray(_threshold), new Gray(_maxValue))
                : image.ThresholdBinaryInv(new Gray(_threshold), new Gray(_maxValue));
        }
    }
}
