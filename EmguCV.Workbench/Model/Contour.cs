using System;
using System.Drawing;
using System.Linq;

namespace EmguCV.Workbench.Model
{
    public class Contour
    {
        private readonly PointF[] _pixel;

        public Contour(PointF[] pixel)
        {
            _pixel = pixel;
        }

        public Contour(Point[] pixel)
        {
            _pixel = pixel.Select(p => new PointF(p.X, p.Y)).ToArray();
        }

        public int Count => _pixel.Length;
        public float StartX => _pixel.Length > 0 ? _pixel[0].X : float.NaN;
        public float StartY => _pixel.Length > 0 ? _pixel[0].Y : float.NaN;
        public float EndX => _pixel.Length > 0 ? _pixel[_pixel.Length - 1].X : float.NaN;
        public float EndY => _pixel.Length > 0 ? _pixel[_pixel.Length - 1].Y : float.NaN;
        public double Length => GetLength();

        /// <summary>
        /// Computes and sums the lengths of all sub-segments within a segment.
        /// </summary>
        /// <returns>The total length of the segment.</returns>
        private double GetLength()
        {
            var length = 0.0;

            if (_pixel.Length < 2)
                return length;

            for (var i = 1; i < _pixel.Length; i++)
                length +=
                    Math.Sqrt(Math.Pow(_pixel[i].X - _pixel[i - 1].X, 2) +
                              Math.Pow(_pixel[i].Y - _pixel[i - 1].Y, 2));

            return length;
        }

        public PointF[] GetContourF()
        {
            return _pixel;
        }

        public Point[] GetContour()
        {
            return _pixel.Select(Point.Round).ToArray();
        }
    }
}
