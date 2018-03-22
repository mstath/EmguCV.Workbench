using System.ComponentModel;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class ThresholdAdaptive : ImageProcessor
    {
        private Color _maxValue = Colors.White;
        [Category("Threshold Adaptive")]
        [PropertyOrder(0)]
        [DisplayName(@"Max Value")]
        [Description(@"Maximum value to use with binary and binary inverse thresholding types.")]
        public Color MaxValue
        {
            get { return _maxValue; }
            set { Set(ref _maxValue, value); }
        }

        private AdaptiveThresholdType _adaptiveType = AdaptiveThresholdType.MeanC;
        [Category("Threshold Adaptive")]
        [PropertyOrder(1)]
        [DisplayName(@"Adaptive Type")]
        [Description(@"Adaptive method.")]
        public AdaptiveThresholdType AdaptiveType
        {
            get { return _adaptiveType; }
            set { Set(ref _adaptiveType, value); }
        }

        private ThreshType _thresholdType = ThreshType.Binary;
        [Category("Threshold Adaptive")]
        [PropertyOrder(2)]
        [DisplayName(@"Threshold Type")]
        [Description(@"Binary or binary inverse type.")]
        public ThreshType ThresholdType
        {
            get { return _thresholdType; }
            set { Set(ref _thresholdType, value); }
        }

        private int _blockSize = 35;
        [Category("Threshold Adaptive")]
        [PropertyOrder(3)]
        [DisplayName(@"Block Size")]
        [Description(@"The size of a pixel neighborhood that is used to calculate a threshold value for the pixel.")]
        public int BlockSize
        {
            get { return _blockSize; }
            set { Set(ref _blockSize, value.ClampOdd(_blockSize, 3, int.MaxValue)); }
        }

        private Color _param1 = Colors.Black;
        [Category("Threshold Adaptive")]
        [PropertyOrder(4)]
        [DisplayName(@"Param1")]
        [Description(@"Constant subtracted from mean or weighted mean. It may be negative.")]
        public Color Param1
        {
            get { return _param1; }
            set { Set(ref _param1, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image
                .ThresholdAdaptive(
                    new Bgr(_maxValue.Color()),
                    _adaptiveType,
                    (ThresholdType) _thresholdType,
                    _blockSize,
                    new Bgr(_param1.Color()));
        }
    }
}
