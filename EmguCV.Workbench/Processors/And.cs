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
    /// Performs binary AND operation with some color.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    [Export(typeof(IImageProcessor))]
    public class And : ImageProcessor
    {
        private Color _color = Colors.White;
        [Category("And")]
        [PropertyOrder(0)]
        [DisplayName(@"Color")]
        [Description(@"The color for the AND operation.")]
        public Color Color
        {
            get { return _color; }
            set { Set(ref _color, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.And(new Bgr(_color.Color()));
        }
    }
}
