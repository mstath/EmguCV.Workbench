using System.ComponentModel;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Find the elementwise minimum/maximum image.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    public class MinMax : ImageProcessor
    {
        private Color _value = Colors.Black;
        [Category("Min Max")]
        [PropertyOrder(0)]
        [DisplayName(@"Value")]
        [Description(@"The value to compare with.")]
        public Color Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }

        private bool _max;
        [Category("Min Max")]
        [PropertyOrder(1)]
        [DisplayName(@"Max")]
        [Description(@"Unchecked: minimum of image and value.  Checked: maximum of image and value.")]
        public bool Max
        {
            get { return _max; }
            set { Set(ref _max, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            using (var color = new Image<Bgr, byte>(image.Width, image.Height, new Bgr(_value.Color())))
            {
                image = !_max ? image.Min(color) : image.Max(color);
            }
        }
    }
}
