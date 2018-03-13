using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCV.Workbench.Model
{
    public class RotatedBox
    {
        private readonly RotatedRect _pixel;
        private readonly PointF[] _vertices;

        public RotatedBox(RotatedRect pixel)
        {
            _pixel = pixel;
            _vertices = CvInvoke.BoxPoints(_pixel);
        }

        public float Width => _pixel.Size.Width;
        public float Height => _pixel.Size.Height;
        public float Angle => _pixel.Angle;
        public float X0 => _vertices[0].X;
        public float Y0 => _vertices[0].Y;
        public float X1 => _vertices[1].X;
        public float Y1 => _vertices[1].Y;
        public float X2 => _vertices[2].X;
        public float Y2 => _vertices[2].Y;
        public float X3 => _vertices[3].X;
        public float Y3 => _vertices[3].Y;

        public RotatedRect GetBox()
        {
            return _pixel;
        }

        public PointF[] GetVertices()
        {
            return _vertices;
        }
    }
}
