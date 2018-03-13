using System;
using System.Drawing;

namespace EmguCV.Workbench.Model
{
    public class Contour
    {
        private readonly Point[] _pixel;

        public Contour(Point[] pixel)
        {
            _pixel = pixel;
        }

        public int Count => _pixel.Length;
        public float StartX => _pixel.Length > 0 ? _pixel[0].X : float.NaN;
        public float StartY => _pixel.Length > 0 ? _pixel[0].Y : float.NaN;
        public float EndX => _pixel.Length > 0 ? _pixel[_pixel.Length - 1].X : float.NaN;
        public float EndY => _pixel.Length > 0 ? _pixel[_pixel.Length - 1].Y : float.NaN;
        public double Length => GetLength();

        private double GetLength()
        {
            var length = 0.0;

            if (_pixel.Length > 1)
                for (var i = 1; i < _pixel.Length; i++)
                    length +=
                        Math.Sqrt(Math.Pow(_pixel[i].X - _pixel[i - 1].X, 2) +
                                  Math.Pow(_pixel[i].Y - _pixel[i - 1].Y, 2));

            return length;
        }

        public Point[] GetContour()
        {
            return _pixel;
        }
    }
}
