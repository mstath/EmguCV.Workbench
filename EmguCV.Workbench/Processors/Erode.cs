using System.ComponentModel;
using System.ComponentModel.Composition;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Erodes image using a 3x3 rectangular structuring element for a
    /// given number of iterations.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    [Export(typeof(IImageProcessor))]
    public class Erode : ImageProcessor
    {
        private int _iterations;
        [Category("Erode")]
        [PropertyOrder(0)]
        [DisplayName(@"Iterations")]
        [Description(@"The number of erode iterations.")]
        public int Iterations
        {
            get { return _iterations; }
            set { Set(ref _iterations, value.Clamp(0, int.MaxValue)); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.Erode(_iterations);
        }
    }
}
