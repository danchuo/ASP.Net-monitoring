using System;
using System.Globalization;
using System.Windows.Data;

namespace ASP.NetMonitoringWPF.Converters;

public class SecondsToMinutesConverter : IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        var time = TimeSpan.FromSeconds(int.Parse(value.ToString() ?? string.Empty));

        return time.ToString(@"mm\:ss");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return string.Join("", ((string) value).Split(' '));
    }

}