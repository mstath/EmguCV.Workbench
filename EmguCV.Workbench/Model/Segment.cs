using Emgu.CV.Structure;

namespace EmguCV.Workbench.Model
{
    public class Segment
    {
        private readonly LineSegment2D _pixel;

        public Segment(LineSegment2D pixel)
        {
            _pixel = pixel;
        }

        public float P1X => _pixel.P1.X;
        public float P1Y => _pixel.P1.Y;
        public float P2X => _pixel.P2.X;
        public float P2Y => _pixel.P2.Y;
        public double Length => _pixel.Length;
        public float DirX => _pixel.Direction.X;
        public float DirY => _pixel.Direction.Y;

        public LineSegment2D GetSegment()
        {
            return _pixel;
        }
    }
}
