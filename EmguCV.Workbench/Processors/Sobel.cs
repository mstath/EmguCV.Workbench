using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Sobel : ImageProcessor
    {
        public Sobel()
        {
            XOrder = 1;
            YOrder = 0;
            ApertureSize = 7;
        }

        private int _xorder;
        [Category("Sobel")]
        [PropertyOrder(0)]
        [DisplayName(@"X Order")]
        [Description(@"Order of the derivative x.")]
        [DefaultValue(1)]
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
        [DefaultValue(0)]
        public int YOrder
        {
            get { return _yorder; }
            set { Set(ref _yorder, value.Clamp(0, 6)); }
        }

        private int _apertureSize;
        [Category("Sobel")]
        [PropertyOrder(2)]
        [DisplayName(@"Aperture Size")]
        [Description(@"Size of the extended Sobel kernel.")]
        [DefaultValue(7)]
        public int ApertureSize
        {
            get { return _apertureSize; }
            set { Set(ref _apertureSize, value.ClampOdd(_apertureSize, 1, 7)); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = image.Convert<Gray, float>().Sobel(_xorder, _yorder, _apertureSize).Convert<Gray, byte>();
        }
    }
}
