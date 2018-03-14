using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class SmoothBilatral : ImageProcessor
    {
        private int _kernelSize;
        [Category("Smooth Bilatral")]
        [PropertyOrder(0)]
        [DisplayName(@"Kernel Size")]
        [Description(@"The size of the bilatral kernel.")]
        public int KernelSize
        {
            get { return _kernelSize; }
            set { Set(ref _kernelSize, value.Clamp(0, int.MaxValue)); }
        }

        private int _colorSigma;
        [Category("Smooth Bilatral")]
        [PropertyOrder(1)]
        [DisplayName(@"Color Sigma")]
        public int ColorSigma
        {
            get { return _colorSigma; }
            set { Set(ref _colorSigma, value.Clamp(0, int.MaxValue)); }
        }

        private int _spaceSigma;
        [Category("Smooth Bilatral")]
        [PropertyOrder(2)]
        [DisplayName(@"Space Sigma")]
        public int SpaceSigma
        {
            get { return _spaceSigma; }
            set { Set(ref _spaceSigma, value.Clamp(0, int.MaxValue)); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = image.SmoothBilatral(
                _kernelSize,
                _colorSigma,
                _spaceSigma);
        }
    }
}
