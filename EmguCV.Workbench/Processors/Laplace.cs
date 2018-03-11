using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Laplace : ImageProcessor
    {
        public Laplace()
        {
            ApertureSize = 1;
        }

        private int _apertureSize;
        [Category("Laplace")]
        [PropertyOrder(0)]
        [DisplayName(@"Aperture Size")]
        [Description(@"Aperture size.")]
        [DefaultValue(1)]
        public int ApertureSize
        {
            get { return _apertureSize; }
            set { Set(ref _apertureSize, value.ClampOdd(_apertureSize, 1, 31)); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = image.Convert<Gray, float>().Laplace(_apertureSize).Convert<Gray, byte>();
        }
    }
}
