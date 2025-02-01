using HunterPie.UI.Logging.Entity;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Logging.Presentation;

public class LogLevelToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (LogLevel)value switch
        {
            LogLevel.Debug => "[DEBUG]",
            LogLevel.Info => "[INFO]",
            LogLevel.Warn => "[WARN]",
            LogLevel.Error => "[ERROR]",
            LogLevel.Panic => "[PANIC]",
            LogLevel.Benchmark => "[BENCHMARK]",
            _ => string.Empty,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}