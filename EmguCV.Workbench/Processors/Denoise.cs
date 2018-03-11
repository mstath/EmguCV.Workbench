using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Denoise : ImageProcessor
    {
        public Denoise()
        {
            H = 3;
            TemplateWindowSize = 7;
            SearchWindowSize = 21;
        }

        private float _h;
        [Category("Denoise")]
        [PropertyOrder(0)]
        [DisplayName(@"Filter Strength")]
        [Description(@"Parameter regulating filter strength.")]
        [DefaultValue(3)]
        public float H
        {
            get { return _h; }
            set { Set(ref _h, value); }
        }

        private int _templateWindowSize;
        [Category("Denoise")]
        [PropertyOrder(1)]
        [DisplayName(@"Template Window Size")]
        [Description(@"Size in pixels of the template patch that is used to compute weights.")]
        [DefaultValue(7)]
        public int TemplateWindowSize
        {
            get { return _templateWindowSize; }
            set { Set(ref _templateWindowSize, value.ClampOdd(_templateWindowSize, 1, int.MaxValue)); }
        }

        private int _searchWindowSize;
        [Category("Denoise")]
        [PropertyOrder(2)]
        [DisplayName(@"Search Window Size")]
        [Description(@"Size in pixels of the window that is used to compute weighted average for given pixel.")]
        [DefaultValue(21)]
        public int SearchWindowSize
        {
            get { return _searchWindowSize; }
            set { Set(ref _searchWindowSize, value.ClampOdd(_searchWindowSize, 1, int.MaxValue)); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            var dns = new Image<Gray, byte>(image.Width, image.Height);
            CvInvoke.FastNlMeansDenoising(image, dns, _h, _templateWindowSize, _searchWindowSize);
            image = dns;
        }
    }
}
