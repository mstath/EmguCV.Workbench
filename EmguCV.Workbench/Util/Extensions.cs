using System;

namespace EmguCV.Workbench.Util
{
    public static class Extensions
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            if (val.CompareTo(max) > 0) return max;
            return val;
        }

        public static int ClampOdd(this int val, int lastVal, int min, int max)
        {
            int newVal;
            if (val % 2 == 0)
            {
                if (val > lastVal)
                    newVal = val + 1;
                else
                    newVal = val - 1;
            }
            else
                newVal = val;

            if (newVal.CompareTo(min) < 0) return min;
            if (newVal.CompareTo(max) > 0) return max;
            return newVal;
        }

        public static System.Drawing.Color Color(this System.Windows.Media.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
