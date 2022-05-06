using System;
using System.Globalization;
using System.Windows.Data;

namespace ASP.NetMonitoringWPF.Converters;

public class BoolToStringConverter : IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        var flag = (bool) value;

        return flag ? "Да" : "Нет";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        var flag = value.ToString();

        return flag == "Да";
    }

}