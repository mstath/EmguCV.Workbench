using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Sobel : ImageProcessor
    {
        private int _xorder = 1;
        [Category("Sobel")]
        [PropertyOrder(0)]
        [DisplayName(@"X Order")]
        [Description(@"Order of the derivative x.")]
        public int XOrder
        {
            get { return _xorder; }
            set { Set(ref _xorder, value.Clamp(0, 6)); }
        }

        private int _yorder;
        [Category("Sobel")]
        [PropertyOrder(1)]
        [DisplayName(@"Y Order")]
        [Description(@"Order of the derivative y.")]
        public int YOrder
        {
            get { return _yorder; }
            set { Set(ref _yorder, value.Clamp(0, 6)); }
        }

        private int _apertureSize = 7;
        [Category("Sobel")]
        [PropertyOrder(2)]
        [DisplayName(@"Aperture Size")]
        [Description(@"Size of the extended Sobel kernel.")]
        public int ApertureSize
        {
            get { return _apertureSize; }
            set { Set(ref _apertureSize, value.ClampOdd(_apertureSize, 1, 7)); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.Convert<Bgr, float>().Sobel(_xorder, _yorder, _apertureSize).Convert<Bgr, byte>();
        }
    }
}
