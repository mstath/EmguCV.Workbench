using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Sobel : ImageProcessor
    {
        private SobelOrder _order;
        [Category("Sobel")]
        [PropertyOrder(0)]
        [DisplayName(@"Order")]
        [Description(@"Order of the derivative.")]
        public SobelOrder Order
        {
            get { return _order; }
            set { Set(ref _order, value); }
        }

        private int _orderValue = 1;
        [Category("Sobel")]
        [PropertyOrder(1)]
        [DisplayName(@"Order Value")]
        public int YOrder
        {
            get { return _orderValue; }
            set { Set(ref _orderValue, value.Clamp(1, _apertureSize - 1)); }
        }

        private int _apertureSize = 7;
        [Category("Sobel")]
        [PropertyOrder(2)]
        [DisplayName(@"Aperture Size")]
        [Description(@"Size of the extended Sobel kernel.")]
        public int ApertureSize
        {
            get { return _apertureSize; }
            set { Set(ref _apertureSize, value.ClampOdd(_apertureSize, 1, 31)); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image
                .Convert<Bgr, float>()
                .Sobel(
                    _order == SobelOrder.X ? _orderValue : 0,
                    _order == SobelOrder.Y ? _orderValue : 0,
                    _apertureSize)
                .Convert<Bgr, byte>();
        }
    }
}
