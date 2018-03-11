using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Canny : ImageProcessor
    {
        public Canny()
        {
            Thresh = 255;
            ThreshLinking = 255;
        }

        private byte _thresh;
        [Category("Canny")]
        [PropertyOrder(0)]
        [DisplayName(@"Threshold")]
        [Description(@"The threshold value.")]
        [DefaultValue(255)]
        public byte Thresh
        {
            get { return _thresh; }
            set { Set(ref _thresh, value); }
        }

        private byte _threshLinking;
        [Category("Canny")]
        [PropertyOrder(1)]
        [DisplayName(@"Threshold Linking")]
        [Description(@"The threshold used for edge linking.")]
        [DefaultValue(255)]
        public byte ThreshLinking
        {
            get { return _threshLinking; }
            set { Set(ref _threshLinking, value); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = image.Canny(_thresh, _threshLinking);
        }
    }
}
