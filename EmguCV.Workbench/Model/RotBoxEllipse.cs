using Emgu.CV.Structure;

namespace EmguCV.Workbench.Model
{
    public class RotBoxEllipse
    {
        private readonly RotatedRect _pixel;

        public RotBoxEllipse(RotatedRect pixel)
        {
            _pixel = pixel;
        }

        public float CenterX => _pixel.Center.X;
        public float CenterY => _pixel.Center.Y;
        public float Width => _pixel.Size.Width;
        public float Height => _pixel.Size.Height;
        public float Angle => _pixel.Angle;

        public Ellipse GetEllipse()
        {
            return new Ellipse(_pixel);
        }
    }
}
