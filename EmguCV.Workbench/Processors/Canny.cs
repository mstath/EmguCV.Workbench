using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Converts image to image of edges.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    public class Canny : ImageProcessor
    {
        private byte _thresh = 255;
        [Category("Canny")]
        [PropertyOrder(0)]
        [DisplayName(@"Threshold")]
        [Description(@"The threshold value.")]
        public byte Thresh
        {
            get { return _thresh; }
            set { Set(ref _thresh, value); }
        }

        private byte _threshLinking = 255;
        [Category("Canny")]
        [PropertyOrder(1)]
        [DisplayName(@"Threshold Linking")]
        [Description(@"The threshold used for edge linking.")]
        public byte ThreshLinking
        {
            get { return _threshLinking; }
            set { Set(ref _threshLinking, value); }
        }

        private int _apertureSize = 3;
        [Category("Canny")]
        [PropertyOrder(2)]
        [DisplayName(@"Aperture Size")]
        public int ApertureSize
        {
            get { return _apertureSize; }
            set { Set(ref _apertureSize, value.ClampOdd(_apertureSize, 3, 7)); }
        }

        private bool _l2Gradient;
        [Category("Canny")]
        [PropertyOrder(3)]
        [DisplayName(@"L2 Gradient")]
        [Description(@"Use a more accurate norm.")]
        public bool L2Gradient
        {
            get { return _l2Gradient; }
            set { Set(ref _l2Gradient, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image =
                image
                    .Convert<Gray, byte>()
                    .Canny(_thresh, _threshLinking, _apertureSize, _l2Gradient)
                    .Convert<Bgr, byte>();
        }
    }
}
