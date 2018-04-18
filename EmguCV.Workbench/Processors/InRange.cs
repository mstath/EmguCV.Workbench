using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Checks that image elements lie between two scalars.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    [Export(typeof(IImageProcessor))]
    public class InRange : ImageProcessor
    {
        private Color _lower = Colors.Black;
        [Category("In Range")]
        [PropertyOrder(0)]
        [DisplayName(@"Lower")]
        [Description(@"The inclusive lower limit of color value.")]
        public Color Lower
        {
            get { return _lower; }
            set { Set(ref _lower, value); }
        }

        private Color _higher = Colors.White;
        [Category("In Range")]
        [PropertyOrder(1)]
        [DisplayName(@"Higher")]
        [Description(@"The inclusive upper limit of color value.")]
        public Color Higher
        {
            get { return _higher; }
            set { Set(ref _higher, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.InRange(new Bgr(_lower.Color()), new Bgr(_higher.Color())).Convert<Bgr, byte>();
        }
    }
}
