using System.ComponentModel;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Elementwise addition/subtraction of a color to the image.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    class AddSubtract : ImageProcessor
    {
        private Color _color = Colors.Black;
        [Category("Add Subtract")]
        [PropertyOrder(0)]
        [DisplayName(@"Color")]
        [Description(@"The color value to be added/subtracted to the image.")]
        public Color Color
        {
            get { return _color; }
            set { Set(ref _color, value); }
        }

        private bool _subtract;
        [Category("Add Subtract")]
        [PropertyOrder(1)]
        [DisplayName(@"Subtract")]
        [Description(@"Unchecked: value added to image.  Checked: value subtracted from image.")]
        public bool Subtract
        {
            get { return _subtract; }
            set { Set(ref _subtract, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = !_subtract ? image.Add(new Bgr(_color.Color())) : image.Sub(new Bgr(_color.Color()));
        }
    }
}
