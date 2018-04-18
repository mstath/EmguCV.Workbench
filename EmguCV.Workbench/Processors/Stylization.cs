using System.ComponentModel;
using System.ComponentModel.Composition;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Stylization aims to produce digital imagery with a wide variety of effects not focused on photorealism. 
    /// Edge-aware filters are ideal for stylization, as they can abstract regions of low contrast while preserving, 
    /// or enhancing, high-contrast features.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    [Export(typeof(IImageProcessor))]
    public class Stylization : ImageProcessor
    {
        private float _sigmaS = 60f;
        [Category("Stylization")]
        [PropertyOrder(0)]
        [DisplayName(@"Sigma S")]
        public float SigmaS
        {
            get { return _sigmaS; }
            set { Set(ref _sigmaS, value.Clamp(0, 200)); }
        }

        private float _sigmaR = 0.45f;
        [Category("Stylization")]
        [PropertyOrder(1)]
        [DisplayName(@"Sigma R")]
        public float SigmaR
        {
            get { return _sigmaR; }
            set { Set(ref _sigmaR, value.Clamp(0, 1)); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            CvInvoke.Stylization(image, image, _sigmaS, _sigmaR);
        }
    }
}
