using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Calculates Laplacian of the image by summing second x- and y- derivatives calculated using Sobel operator. 
    /// Specifying aperture_size=1 gives the fastest variant that is equal to convolving the image with the following kernel: |0 1 0| |1 -4 1| |0 1 0|.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    public class Laplace : ImageProcessor
    {
        private int _apertureSize = 1;
        [Category("Laplace")]
        [PropertyOrder(0)]
        [DisplayName(@"Aperture Size")]
        [Description(@"Aperture size.")]
        public int ApertureSize
        {
            get { return _apertureSize; }
            set { Set(ref _apertureSize, value.ClampOdd(_apertureSize, 1, 31)); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.Convert<Bgr, float>().Laplace(_apertureSize).Convert<Bgr, byte>();
        }
    }
}
