using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace ASP.NetMonitoringWPF.Converters;

public class StringToSplitStringConverter : IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return string.Join(" ", Regex.Split((string) value, "(?=\\p{Lu}\\p{Lu}\\p{Lu}\\p{Lu}|\\p{Lu}\\p{Ll})"))
            .Replace('_', '\0');
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return string.Join("", ((string) value).Split(' '));
    }

}