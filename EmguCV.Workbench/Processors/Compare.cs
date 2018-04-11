using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Compare the image with value and return the comparison mask.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    public class Compare : ImageProcessor
    {
        private byte _value;
        [Category("Compare")]
        [PropertyOrder(0)]
        [DisplayName(@"Value")]
        [Description(@"The value to compare with.")]
        public byte Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }

        private CmpType _comparisonType = CmpType.Equal;
        [Category("Compare")]
        [PropertyOrder(1)]
        [DisplayName(@"Comparison Type")]
        public CmpType ComparisonType
        {
            get { return _comparisonType; }
            set { Set(ref _comparisonType, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.Cmp(_value, _comparisonType);
        }
    }
}
