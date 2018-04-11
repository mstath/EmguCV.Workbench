using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Applies convolution (Sobel) filters to create derivitive image.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    public class Derivitive : ImageProcessor
    {
        private int _order = 1;
        [Category("Derivitive")]
        [PropertyOrder(0)]
        [DisplayName(@"Order")]
        [Description(@"Order of the derivative.")]
        public int Order
        {
            get { return _order; }
            set { Set(ref _order, value.Clamp(1, _apertureSize - 1)); }
        }

        private int _apertureSize = 3;
        [Category("Derivitive")]
        [PropertyOrder(1)]
        [DisplayName(@"Aperture Size")]
        [Description(@"Size of the extended Sobel kernel.")]
        public int ApertureSize
        {
            get { return _apertureSize; }
            set { Set(ref _apertureSize, value.ClampOdd(_apertureSize, 1, 31)); }
        }

        private bool _invert = true;
        [Category("Derivitive")]
        [PropertyOrder(2)]
        [DisplayName(@"Invert")]
        public bool Invert
        {
            get { return _invert; }
            set { Set(ref _invert, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            // the gradient image in x
            var gradX = image.Sobel(_order, 0, _apertureSize).ConvertScale<byte>(1, 0);
            // the gradient image in y
            var gradY = image.Sobel(0, _order, _apertureSize).ConvertScale<byte>(1, 0);
            var grad = new Image<Bgr, byte>(image.Width, image.Height);
            // blend the gradient images
            CvInvoke.AddWeighted(gradX, 0.5, gradY, 0.5, 1.0, grad);
            // invert the image
            if (_invert)
                CvInvoke.BitwiseNot(grad, grad);
            image = grad.Convert<Bgr, byte>();
        }
    }
}
