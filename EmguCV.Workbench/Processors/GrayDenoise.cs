using System.ComponentModel;
using System.ComponentModel.Composition;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Perform image denoising using Non-local Means Denoising algorithm to Gray Scale image.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    [Export(typeof(IImageProcessor))]
    public class GrayDenoise : ImageProcessor
    {
        private float _h = 3;
        [Category("Denoise")]
        [PropertyOrder(0)]
        [DisplayName(@"Filter Strength")]
        [Description(@"Parameter regulating filter strength.")]
        public float H
        {
            get { return _h; }
            set { Set(ref _h, value); }
        }

        private int _templateWindowSize = 7;
        [Category("Denoise")]
        [PropertyOrder(2)]
        [DisplayName(@"Template Window Size")]
        [Description(@"Size in pixels of the template patch that is used to compute weights.")]
        public int TemplateWindowSize
        {
            get { return _templateWindowSize; }
            set { Set(ref _templateWindowSize, value.ClampOdd(_templateWindowSize, 1, int.MaxValue)); }
        }

        private int _searchWindowSize = 21;
        [Category("Denoise")]
        [PropertyOrder(3)]
        [DisplayName(@"Search Window Size")]
        [Description(@"Size in pixels of the window that is used to compute weighted average for given pixel.")]
        public int SearchWindowSize
        {
            get { return _searchWindowSize; }
            set { Set(ref _searchWindowSize, value.ClampOdd(_searchWindowSize, 1, int.MaxValue)); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            var dst = new Image<Gray, byte>(image.Width, image.Height);
            CvInvoke.FastNlMeansDenoising(image.Convert<Gray, byte>(), dst, _h, _templateWindowSize, _searchWindowSize);
            image = dst.Convert<Bgr, byte>();
        }
    }
}
