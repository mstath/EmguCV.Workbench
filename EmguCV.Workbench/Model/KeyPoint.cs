using Emgu.CV.Structure;

namespace EmguCV.Workbench.Model
{
    public class KeyPoint
    {
        private readonly MKeyPoint _pixel;

        public KeyPoint(MKeyPoint pixel)
        {
            _pixel = pixel;
        }

        public float X => _pixel.Point.X;
        public float Y => _pixel.Point.Y;
        public float Size => _pixel.Size;
        public float Angle => _pixel.Angle;
        public float Response => _pixel.Response;
        public int Octave => _pixel.Octave;
        public int ClassId => _pixel.ClassId;

        public MKeyPoint GetKeyPoint()
        {
            return _pixel;
        }
    }
}
