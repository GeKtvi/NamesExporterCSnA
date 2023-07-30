using NamesExporterCSnA.Services.UpdateLog;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace NamesExporterCSnA.View.Converters
{
    public class LoggerStatusToAppearanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            switch ((LoggerStatus)value)
            {
                case LoggerStatus.NoFails:
                    return Wpf.Ui.Common.ControlAppearance.Primary;
                case LoggerStatus.HasExceptionFails:
                    return Wpf.Ui.Common.ControlAppearance.Caution;
                case LoggerStatus.HasErrorFails:
                    return Wpf.Ui.Common.ControlAppearance.Danger;
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
