using NamesExporterCSnA.Services.UpdateLog;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NamesExporterCSnA.View
{
    public class LoggerStatusToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((LoggerStatus)value == LoggerStatus.NoFails)
                return Visibility.Hidden;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((Visibility)value != Visibility.Visible)
                return LoggerStatus.HasExceptionFails;
            else 
                return LoggerStatus.NoFails;
        }
    }
}
