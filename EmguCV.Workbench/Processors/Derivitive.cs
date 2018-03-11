using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Derivitive : ImageProcessor
    {
        public Derivitive()
        {
            Order = 1;
            ApertureSize = 3;
        }

        private int _order;
        [Category("Derivitive")]
        [PropertyOrder(0)]
        [DisplayName(@"Order")]
        [Description(@"Order of the derivative.")]
        [DefaultValue(1)]
        public int Order
        {
            get { return _order; }
            set { Set(ref _order, value.Clamp(1, 6)); }
        }

        private int _apertureSize;
        [Category("Derivitive")]
        [PropertyOrder(1)]
        [DisplayName(@"Aperture Size")]
        [Description(@"Size of the extended Sobel kernel.")]
        [DefaultValue(3)]
        public int ApertureSize
        {
            get { return _apertureSize; }
            set { Set(ref _apertureSize, value.ClampOdd(_apertureSize, 1, 7)); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            var gradX = image.Sobel(_order, 0, _apertureSize).ConvertScale<byte>(1, 0);
            var gradY = image.Sobel(0, _order, _apertureSize).ConvertScale<byte>(1, 0);
            var grad = new Image<Gray, byte>(image.Width, image.Height);
            CvInvoke.AddWeighted(gradX, 0.5, gradY, 0.5, 1.0, grad);
            CvInvoke.BitwiseNot(grad, grad);
            image = grad;
        }
    }
}
