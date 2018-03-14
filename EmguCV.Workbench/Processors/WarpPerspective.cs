using System.ComponentModel;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    class WarpPerspective : ImageProcessor
    {
        public WarpPerspective()
        {
            P1X = 0;
            P1Y = 0;
            P2X = 640;
            P2Y = 0;
            P3X = 640;
            P3Y = 480;
            P4X = 0;
            P4Y = 480;
            Width = 640;
            Height = 480;
            InterpolationType = Inter.Nearest;
            WarpType = Warp.Default;
            BorderType = BordType.Transparent;
            BackgroundColor = 0;
        }

        private float _p1X;
        [Category("Warp Affine")]
        [PropertyOrder(0)]
        [DisplayName(@"P1X")]
        [Description(@"The X coordinate of the upper left corner.")]
        [DefaultValue(0f)]
        public float P1X
        {
            get { return _p1X; }
            set { Set(ref _p1X, value); }
        }

        private float _p1Y;
        [Category("Warp Affine")]
        [PropertyOrder(1)]
        [DisplayName(@"P1Y")]
        [Description(@"The Y coordinate of the upper left corner.")]
        [DefaultValue(0f)]
        public float P1Y
        {
            get { return _p1Y; }
            set { Set(ref _p1Y, value); }
        }

        private float _p2X;
        [Category("Warp Affine")]
        [PropertyOrder(2)]
        [DisplayName(@"P2X")]
        [Description(@"The X coordinate of the upper right corner.")]
        [DefaultValue(640f)]
        public float P2X
        {
            get { return _p2X; }
            set { Set(ref _p2X, value); }
        }

        private float _p2Y;
        [Category("Warp Affine")]
        [PropertyOrder(3)]
        [DisplayName(@"P2Y")]
        [Description(@"The Y coordinate of the upper right corner.")]
        [DefaultValue(0f)]
        public float P2Y
        {
            get { return _p2Y; }
            set { Set(ref _p2Y, value); }
        }

        private float _p3X;
        [Category("Warp Affine")]
        [PropertyOrder(4)]
        [DisplayName(@"P3X")]
        [Description(@"The X coordinate of the lower right corner.")]
        [DefaultValue(640f)]
        public float P3X
        {
            get { return _p3X; }
            set { Set(ref _p3X, value); }
        }

        private float _p3Y;
        [Category("Warp Affine")]
        [PropertyOrder(5)]
        [DisplayName(@"P3Y")]
        [Description(@"The Y coordinate of the lower right corner.")]
        [DefaultValue(480f)]
        public float P3Y
        {
            get { return _p3Y; }
            set { Set(ref _p3Y, value); }
        }

        private float _p4X;
        [Category("Warp Affine")]
        [PropertyOrder(6)]
        [DisplayName(@"P4X")]
        [Description(@"The X coordinate of the lower left corner.")]
        [DefaultValue(0f)]
        public float P4X
        {
            get { return _p4X; }
            set { Set(ref _p4X, value); }
        }

        private float _p4Y;
        [Category("Warp Affine")]
        [PropertyOrder(7)]
        [DisplayName(@"P4Y")]
        [Description(@"The Y coordinate of the lower left corner.")]
        [DefaultValue(480f)]
        public float P4Y
        {
            get { return _p4Y; }
            set { Set(ref _p4Y, value); }
        }

        private int _width;
        [Category("Warp Affine")]
        [PropertyOrder(8)]
        [DisplayName(@"Width")]
        [Description(@"The width of the resulting image.")]
        [DefaultValue(640)]
        public int Width
        {
            get { return _width; }
            set { Set(ref _width, value); }
        }

        private int _height;
        [Category("Warp Affine")]
        [PropertyOrder(9)]
        [DisplayName(@"Height")]
        [Description(@"The height of the resulting image.")]
        [DefaultValue(480)]
        public int Height
        {
            get { return _height; }
            set { Set(ref _height, value); }
        }

        private Inter _interpolationType;
        [Category("Warp Affine")]
        [PropertyOrder(10)]
        [DisplayName(@"Interpolation Type")]
        [DefaultValue(Inter.Nearest)]
        public Inter InterpolationType
        {
            get { return _interpolationType; }
            set { Set(ref _interpolationType, value); }
        }

        private Warp _warpType;
        [Category("Warp Affine")]
        [PropertyOrder(11)]
        [DisplayName(@"Warp Type")]
        [DefaultValue(Warp.Default)]
        public Warp WarpType
        {
            get { return _warpType; }
            set { Set(ref _warpType, value); }
        }

        private BordType _borderType;
        [Category("Warp Affine")]
        [PropertyOrder(12)]
        [DisplayName(@"Border Type")]
        [Description(@"Pixel extrapolation method.")]
        [DefaultValue(BordType.Constant)]
        public BordType BorderType
        {
            get { return _borderType; }
            set { Set(ref _borderType, value); }
        }

        private byte _backgroundColor;
        [Category("Warp Affine")]
        [PropertyOrder(13)]
        [DisplayName(@"Background Color")]
        [Description(@"A value used to fill outliers.")]
        public byte BackgroundColor
        {
            get { return _backgroundColor; }
            set { Set(ref _backgroundColor, value); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            var src = new[]
            {
                new PointF(0, 0),
                new PointF(image.Width, 0),
                new PointF(image.Width, image.Height),
                new PointF(0, image.Height)
            };
            var dst = new[]
            {
                new PointF(_p1X, _p1Y),
                new PointF(_p2X, _p2Y),
                new PointF(_p3X, _p3Y),
                new PointF(_p4X, _p4Y)
            };

            using (var mat = CvInvoke.GetPerspectiveTransform(src, dst))
            {
                CvInvoke.WarpPerspective(
                    image,
                    image,
                    mat,
                    new Size(_width, _height),
                    _interpolationType,
                    _warpType,
                    (BorderType) _borderType,
                    new Gray(_backgroundColor).MCvScalar);
            }
        }
    }
}
