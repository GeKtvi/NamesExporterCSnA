using NamesExporterCSnA.Data.UpdateLog;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace NamesExporterCSnA.View.Converters
{
    public class LoggerStatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((LoggerStatus)value)
            {
                case LoggerStatus.NoFails:
                    return new SolidColorBrush(Colors.Transparent);
                case LoggerStatus.HasExceptionFails:
                    return new SolidColorBrush(Colors.Orange);
                case LoggerStatus.HasErrorFails:
                    return new SolidColorBrush(Colors.Red);
                default:
                    throw new ArgumentException("Возможно использование только с LoggerStatus", nameof(value));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
