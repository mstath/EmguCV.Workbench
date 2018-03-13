using System.Drawing;

namespace EmguCV.Workbench.Model
{
    public class Box
    {
        private readonly Rectangle _pixel;

        public Box(Rectangle pixel)
        {
            _pixel = pixel;
        }

        public int Width => _pixel.Width;
        public int Height => _pixel.Height;
        public int X => _pixel.X;
        public int Y => _pixel.Y;
        public int Top => _pixel.Top;
        public int Right => _pixel.Right;
        public int Bottom => _pixel.Bottom;
        public int Left => _pixel.Left;

        public Rectangle GetBox()
        {
            return _pixel;
        }
    }
}
