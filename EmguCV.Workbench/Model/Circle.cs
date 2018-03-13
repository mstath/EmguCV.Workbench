using Emgu.CV.Structure;

namespace EmguCV.Workbench.Model
{
    public class Circle
    {
        private readonly CircleF _pixel;

        public Circle(CircleF pixel)
        {
            _pixel = pixel;
        }

        public float CenterX => _pixel.Center.X;
        public float CenterY => _pixel.Center.Y;
        public float Radius => _pixel.Radius;
        public double Area => _pixel.Area;

        public CircleF GetCircle()
        {
            return _pixel;
        }
    }
}
