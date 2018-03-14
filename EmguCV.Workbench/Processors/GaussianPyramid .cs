using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class GaussianPyramid : ImageProcessor
    {
        private int _maxLevel;
        [Category("Image Pyramid")]
        [PropertyOrder(0)]
        [DisplayName(@"Max Level")]
        [Description(@"The number of levels for the pyramid.")]
        [DefaultValue(0)]
        public int MaxLevel
        {
            get { return _maxLevel; }
            set { Set(ref _maxLevel, value.Clamp(0, int.MaxValue)); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            for (var i = 0; i < _maxLevel; i++)
                image = image.PyrUp().PyrDown();
        }
    }
}
