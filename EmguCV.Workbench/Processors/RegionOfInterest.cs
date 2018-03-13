using System.ComponentModel;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class RegionOfInterest : ImageProcessor
    {
        private Rectangle _roi;

        public RegionOfInterest()
        {
            Width = 640;
            Height = 480;
            X = 0;
            Y = 0;
        }

        [Category("ROI")]
        [PropertyOrder(0)]
        [DisplayName(@"X")]
        [Description(@"The X coordinate of the upper left corner.")]
        [DefaultValue(0)]
        public int X
        {
            get { return _roi.X; }
            set
            {
                _roi.X = value.Clamp(0, int.MaxValue);
                RaisePropertyChanged();
            }
        }

        [Category("ROI")]
        [PropertyOrder(1)]
        [DisplayName(@"Y")]
        [Description(@"The Y coordinate of the upper left corner.")]
        [DefaultValue(0)]
        public int Y
        {
            get { return _roi.Y; }
            set
            {
                _roi.Y = value.Clamp(0, int.MaxValue);
                RaisePropertyChanged();
            }
        }

        [Category("ROI")]
        [PropertyOrder(2)]
        [DisplayName(@"Width")]
        [Description(@"The width of the ROI.")]
        [DefaultValue(640)]
        public int Width
        {
            get { return _roi.Width; }
            set
            {
                _roi.Width = value.Clamp(0, int.MaxValue);
                RaisePropertyChanged();
            }
        }

        [Category("ROI")]
        [PropertyOrder(3)]
        [DisplayName(@"Height")]
        [Description(@"The height of the ROI.")]
        [DefaultValue(640)]
        public int Height
        {
            get { return _roi.Height; }
            set
            {
                _roi.Height = value.Clamp(0, int.MaxValue);
                RaisePropertyChanged();
            }
        }

        private bool _preserveScale;
        [Category("ROI")]
        [PropertyOrder(4)]
        [DisplayName(@"Preserve Scale")]
        [Description(@"Preserves scale of image.")]
        [DefaultValue(false)]
        public bool PreserveScale
        {
            get { return _preserveScale; }
            set { Set(ref _preserveScale, value); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            if (!_preserveScale)
                image.ROI = _roi;
            else
            {
                var bmp = new Bitmap(image.Width, image.Height);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(image.Copy(_roi).Bitmap, _roi.X, _roi.Y, _roi.Width, _roi.Height);
                    image = new Image<Gray, byte>(bmp);
                }
            }
        }
    }
}
