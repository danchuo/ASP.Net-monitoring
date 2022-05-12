using System;
using System.Globalization;
using System.Windows.Data;
using ASP.NetMonitoringWPF.Models;

namespace ASP.NetMonitoringWPF.Converters;

public class QueryToPropertyNameConverter : IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return ((string) value).Split(ParameterList.Delimiter)[1];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return value;
    }

}