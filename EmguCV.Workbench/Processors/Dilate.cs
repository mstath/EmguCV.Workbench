using System.ComponentModel;
using System.ComponentModel.Composition;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Dilates image using a 3x3 rectangular structuring element for a
    /// given number of iterations.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    [Export(typeof(IImageProcessor))]
    public class Dilate : ImageProcessor
    {
        private int _iterations;
        [Category("Dilate")]
        [PropertyOrder(0)]
        [DisplayName(@"Iterations")]
        [Description(@"The number of dilate iterations.")]
        public int Iterations
        {
            get { return _iterations; }
            set { Set(ref _iterations, value.Clamp(0, int.MaxValue)); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.Dilate(_iterations);
        }
    }
}
