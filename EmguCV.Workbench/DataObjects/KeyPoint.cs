using Emgu.CV.Structure;

namespace EmguCV.Workbench.DataObjects
{
    public class KeyPoint
    {
        private MKeyPoint _kp;

        public KeyPoint(MKeyPoint kp)
        {
            _kp = kp;
        }

        public float X => _kp.Point.X;
        public float Y => _kp.Point.Y;
        public float Size => _kp.Size;
        public float Angle => _kp.Angle;
        public float Response => _kp.Response;
        public int Octave => _kp.Octave;
        public int ClassId => _kp.ClassId;

        public MKeyPoint GetMKeyPoint()
        {
            return _kp;
        }
    }
}
