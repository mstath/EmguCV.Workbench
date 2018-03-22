using System.ComponentModel;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Rotate : ImageProcessor
    {
        private double _angle;
        [Category("Rotate")]
        [PropertyOrder(0)]
        [DisplayName(@"Angle")]
        [Description(@"The angle of rotation in degrees.")]
        public double Angle
        {
            get { return _angle; }
            set { Set(ref _angle, value.Clamp(-360, 360)); }
        }

        private bool _crop;
        [Category("Rotate")]
        [PropertyOrder(1)]
        [DisplayName(@"Crop")]
        [Description(@"If set to true the image is cropped to its original size. If set to false all rotation information is preserved.")]
        public bool Crop
        {
            get { return _crop; }
            set { Set(ref _crop, value); }
        }

        private Color _background = Colors.Black;
        [Category("Rotate")]
        [PropertyOrder(2)]
        [DisplayName(@"Background")]
        [Description(@"The color with wich to fill the background.")]
        public Color Background
        {
            get { return _background; }
            set { Set(ref _background, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.Rotate(_angle, new Bgr(_background.Color()), _crop);
        }
    }
}
