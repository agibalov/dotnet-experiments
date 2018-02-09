using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HelloValueConverter
{
    public class BoolToVisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Hidden;
            }

            var isVisible = (bool)value;
            if (isVisible)
            {
                return Visibility.Visible;
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            var visibility = (Visibility)value;
            if (visibility == Visibility.Visible)
            {
                return true;
            }

            return false;
        }
    }
}