using System.ComponentModel;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Or : ImageProcessor
    {
        private Color _color = Colors.Black;
        [Category("Or")]
        [PropertyOrder(0)]
        [DisplayName(@"Color")]
        [Description(@"The value for the OR operation.")]
        public Color Color
        {
            get { return _color; }
            set { Set(ref _color, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.Or(new Bgr(_color.Color()));
        }
    }
}
