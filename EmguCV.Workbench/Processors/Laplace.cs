using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Laplace : ImageProcessor
    {
        private int _apertureSize = 1;
        [Category("Laplace")]
        [PropertyOrder(0)]
        [DisplayName(@"Aperture Size")]
        [Description(@"Aperture size.")]
        public int ApertureSize
        {
            get { return _apertureSize; }
            set { Set(ref _apertureSize, value.ClampOdd(_apertureSize, 1, 31)); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.Convert<Bgr, float>().Laplace(_apertureSize).Convert<Bgr, byte>();
        }
    }
}
