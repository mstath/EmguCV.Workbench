using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Perform Gaussian Smoothing for kernelSize x kernelSize neighborhood.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    public class SmoothGaussian : ImageProcessor
    {
        private int _kernelSize = 1;
        [Category("Smooth Gaussian")]
        [PropertyOrder(0)]
        [DisplayName(@"Kernel Size")]
        [Description(@"The size of the Gaussian kernel.")]
        public int KernelSize
        {
            get { return _kernelSize; }
            set { Set(ref _kernelSize, value.ClampOdd(_kernelSize, 1, 639)); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.SmoothGaussian(_kernelSize);
        }
    }
}
