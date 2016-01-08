using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFNotification.Converters
{
    public class EmptyStringConverter : BaseConverter, IValueConverter
    {
        public EmptyStringConverter()
        { }
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? parameter : value;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
