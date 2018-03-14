using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class ThresholdAdaptive : ImageProcessor
    {
        public ThresholdAdaptive()
        {
            MaxValue = 255;
            BlockSize = 35;
            AdaptiveType = AdaptiveThresholdType.MeanC;
            ThresholdType = ThreshType.Binary;
            Param1 = 0;
        }

        private byte _maxValue;
        [Category("Threshold Adaptive")]
        [PropertyOrder(0)]
        [DisplayName(@"Max Value")]
        [Description(@"Maximum value to use with binary and binary inverse thresholding types.")]
        [DefaultValue(255)]
        public byte MaxValue
        {
            get { return _maxValue; }
            set { Set(ref _maxValue, value); }
        }

        private AdaptiveThresholdType _adaptiveType;
        [Category("Threshold Adaptive")]
        [PropertyOrder(1)]
        [DisplayName(@"Adaptive Type")]
        [Description(@"Adaptive method.")]
        [DefaultValue(AdaptiveThresholdType.MeanC)]
        public AdaptiveThresholdType AdaptiveType
        {
            get { return _adaptiveType; }
            set { Set(ref _adaptiveType, value); }
        }

        private ThreshType _thresholdType;
        [Category("Threshold Adaptive")]
        [PropertyOrder(2)]
        [DisplayName(@"Threshold Type")]
        [Description(@"Binary or binary inverse type.")]
        [DefaultValue(ThreshType.Binary)]
        public ThreshType ThresholdType
        {
            get { return _thresholdType; }
            set { Set(ref _thresholdType, value); }
        }

        private int _blockSize;
        [Category("Threshold Adaptive")]
        [PropertyOrder(3)]
        [DisplayName(@"Block Size")]
        [Description(@"The size of a pixel neighborhood that is used to calculate a threshold value for the pixel.")]
        [DefaultValue(35)]
        public int BlockSize
        {
            get { return _blockSize; }
            set { Set(ref _blockSize, value.ClampOdd(_blockSize, 3, int.MaxValue)); }
        }

        private int _param1;
        [Category("Threshold Adaptive")]
        [PropertyOrder(4)]
        [DisplayName(@"Param1")]
        [Description(@"Constant subtracted from mean or weighted mean. It may be negative.")]
        [DefaultValue(0)]
        public int Param1
        {
            get { return _param1; }
            set { Set(ref _param1, value.Clamp(-255, 255)); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = image.ThresholdAdaptive(
                new Gray(_maxValue),
                _adaptiveType,
                (ThresholdType) _thresholdType,
                _blockSize,
                new Gray(_param1));
        }
    }
}
