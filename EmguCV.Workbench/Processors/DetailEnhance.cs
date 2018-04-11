using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// This filter enhances the details of a particular image.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    public class DetailEnhance : ImageProcessor
    {
        private float _sigmaS = 10f;
        [Category("DetailEnhance")]
        [PropertyOrder(0)]
        [DisplayName(@"Sigma S")]
        public float SigmaS
        {
            get { return _sigmaS; }
            set { Set(ref _sigmaS, value.Clamp(0, 200)); }
        }

        private float _sigmaR = 0.15f;
        [Category("DetailEnhance")]
        [PropertyOrder(1)]
        [DisplayName(@"Sigma R")]
        public float SigmaR
        {
            get { return _sigmaR; }
            set { Set(ref _sigmaR, value.Clamp(0, 1)); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            CvInvoke.DetailEnhance(image, image, _sigmaS, _sigmaR);
        }
    }
}
