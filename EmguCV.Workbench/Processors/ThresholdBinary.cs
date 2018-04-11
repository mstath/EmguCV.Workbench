using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Non-inverted: threshold the image such that dst(x,y) = max_value if src(x,y) > threshold, otherwise 0.
    /// Inverted: threshold the image such that dst(x,y) = 0 if src(x,y) > threshold, otherwise max_value.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    public class ThresholdBinary : ImageProcessor
    {
        private byte _threshold = 127;
        [Category("Threshold Binary")]
        [PropertyOrder(0)]
        [DisplayName(@"Threshold")]
        [Description(@"The threshold value.")]
        public byte Threshold
        {
            get { return _threshold; }
            set { Set(ref _threshold, value); }
        }

        private byte _maxValue = 255;
        [Category("Threshold Binary")]
        [PropertyOrder(1)]
        [DisplayName(@"Max Value")]
        [Description(@"The maximum value at threshold.")]
        public byte MaxValue
        {
            get { return _maxValue; }
            set { Set(ref _maxValue, value); }
        }

        private bool _invert;
        [Category("Threshold Binary")]
        [PropertyOrder(2)]
        [DisplayName(@"Invert")]
        [Description(@"Select to invert threshold.")]
        public bool Invert
        {
            get { return _invert; }
            set { Set(ref _invert, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = !_invert
                ? image.Convert<Gray, byte>()
                    .ThresholdBinary(new Gray(_threshold), new Gray(_maxValue))
                    .Convert<Bgr, byte>()
                : image.Convert<Gray, byte>()
                    .ThresholdBinaryInv(new Gray(_threshold), new Gray(_maxValue))
                    .Convert<Bgr, byte>();
        }
    }
}
