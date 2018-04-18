using System.ComponentModel;
using System.ComponentModel.Composition;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Performs upsampling and downsampling of Gaussian pyramid decomposition
    /// for given number of steps.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    [Export(typeof(IImageProcessor))]
    public class GaussianPyramid : ImageProcessor
    {
        private int _maxLevel;
        [Category("Image Pyramid")]
        [PropertyOrder(0)]
        [DisplayName(@"Max Level")]
        [Description(@"The number of levels for the pyramid.")]
        public int MaxLevel
        {
            get { return _maxLevel; }
            set { Set(ref _maxLevel, value.Clamp(0, int.MaxValue)); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            for (var i = 0; i < _maxLevel; i++)
                image = image.PyrUp().PyrDown();
        }
    }
}
