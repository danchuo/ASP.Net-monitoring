using System;
using System.Globalization;
using System.Windows.Data;

namespace ASP.NetMonitoringWPF.Converters;

public class QueryToPropertyNameConverter : IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return new StringToSplitStringConverter().Convert(((string) value).Split(' ')[1], null!, null!, null!);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return value;
    }

}