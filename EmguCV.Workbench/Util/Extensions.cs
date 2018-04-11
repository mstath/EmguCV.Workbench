using System;

namespace EmguCV.Workbench.Util
{
    public static class Extensions
    {
        /// <summary>
        /// Extension method that clamps value between specified minimum and maximum.
        /// </summary>
        /// <typeparam name="T">Numeric type.</typeparam>
        /// <param name="val">The value being clamped.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>The clamped value.</returns>
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            if (val.CompareTo(max) > 0) return max;
            return val;
        }

        /// <summary>
        /// Extension method that clamps int value to odd value
        /// between specified minimum and maximum.
        /// </summary>
        /// <param name="val">The int value being clamped.</param>
        /// <param name="lastVal">The last int value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>The clamped odd int value.</returns>
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

        /// <summary>
        /// Converts a System.Windows.Media.Color to a System.Drawing.Color.
        /// </summary>
        /// <param name="color">The System.Windows.Media.Color.</param>
        /// <returns>The System.Drawing.Color.</returns>
        public static System.Drawing.Color Color(this System.Windows.Media.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
