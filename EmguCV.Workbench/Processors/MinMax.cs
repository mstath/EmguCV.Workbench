using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class MinMax : ImageProcessor
    {
        public MinMax()
        {
            Value = 127;
        }

        private byte _value;
        [Category("Min Max")]
        [PropertyOrder(0)]
        [DisplayName(@"Value")]
        [Description(@"The value to compare with.")]
        [DefaultValue(127)]
        public byte Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }

        private bool _max;
        [Category("Min Max")]
        [PropertyOrder(1)]
        [DisplayName(@"Max")]
        [Description(@"Unchecked: minimum of image and value.  Checked: maximum of image and value.")]
        [DefaultValue(false)]
        public bool Max
        {
            get { return _max; }
            set { Set(ref _max, value); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = !_max ? image.Min(_value) : image.Max(_value);
        }
    }
}
