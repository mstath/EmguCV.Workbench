using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Split current Image into an array of gray scale images where each element in the 
    /// array represent a single color channel of the original image.  Use the layer paremter
    /// to select which image to display.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    public class Split : ImageProcessor
    {
        private Image<Gray, byte>[] _images;

        private int _layer;
        [Category("Split")]
        [PropertyOrder(0)]
        [DisplayName(@"Layer")]
        [Description(@"The layer of image to display.")]
        public int Layer
        {
            get { return _layer; }
            set { Set(ref _layer, value.Clamp(0, 2)); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            _images = image.Split();
            image = _images[_layer].Convert<Bgr, byte>();
        }
    }
}
