using System;
using System.Globalization;
using System.Windows.Data;

namespace ASP.NetMonitoringWPF.Converters;

public class StringToReadableNameConverter : IValueConverter {

    private const int MaxLength = 15;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        var input = value.ToString();
        if (input == null)
            return value;
        return input.Length > MaxLength ? input[..MaxLength] + "..." : input;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return value;
    }

}