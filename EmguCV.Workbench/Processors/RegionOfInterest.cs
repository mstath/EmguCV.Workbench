using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Set the ROI for the image with option to preserve scale.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    [Export(typeof(IImageProcessor))]
    public class RegionOfInterest : ImageProcessor
    {
        private Rectangle _roi = new Rectangle(0, 0, 640, 480);

        [Category("ROI")]
        [PropertyOrder(0)]
        [DisplayName(@"X")]
        [Description(@"The X coordinate of the upper left corner.")]
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
        public bool PreserveScale
        {
            get { return _preserveScale; }
            set { Set(ref _preserveScale, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            // if not preserving scale, just set the ROI property
            // otherwise create image and superimpose ROI image
            if (!_preserveScale)
                image.ROI = _roi;
            else
            {
                var bmp = new Bitmap(image.Width, image.Height);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(image.Copy(_roi).Bitmap, _roi.X, _roi.Y, _roi.Width, _roi.Height);
                    image = new Image<Bgr, byte>(bmp);
                }
            }
        }
    }
}
