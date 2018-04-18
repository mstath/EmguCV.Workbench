using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace EmguCV.Workbench.Util
{
    public class SplitNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? Regex.Replace(value.ToString(), @"(\B[A-Z])", " $1") : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}