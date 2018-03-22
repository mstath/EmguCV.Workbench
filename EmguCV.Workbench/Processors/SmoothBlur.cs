using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class SmoothBlur : ImageProcessor
    {
        private int _width = 1;
        [Category("Smooth Blur")]
        [PropertyOrder(0)]
        [DisplayName(@"Width")]
        [Description(@"The width of the window.")]
        public int Width
        {
            get { return _width; }
            set { Set(ref _width, value.Clamp(1, int.MaxValue)); }
        }

        private int _height = 1;
        [Category("Smooth Blur")]
        [PropertyOrder(1)]
        [DisplayName(@"Height")]
        [Description(@"The height of the window.")]
        public int Height
        {
            get { return _height; }
            set { Set(ref _height, value.Clamp(1, int.MaxValue)); }
        }

        private bool _scale = true;
        [Category("Smooth Blur")]
        [PropertyOrder(2)]
        [DisplayName(@"Scale")]
        [Description(@"If true, the result is subsequent scaled by 1/(width x height).")]
        public bool Scale
        {
            get { return _scale; }
            set { Set(ref _scale, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.SmoothBlur(
                _width,
                _height,
                _scale);
        }
    }
}
