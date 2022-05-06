using System;
using System.Globalization;
using System.Windows.Data;
using ASP.NetMonitoringWPF.Models;

namespace ASP.NetMonitoringWPF.Converters;

public class TriggerWithGroupConverter : IMultiValueConverter {

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
        var trigger = (MonitoringTrigger) values[0];
        var itemsControl = (System.Windows.Controls.ItemsControl) values[1];
        var group = (TriggerGroup) (itemsControl.Items.CurrentItem ?? itemsControl.Items.GetItemAt(0));
        
        return (trigger, group);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
        return null;
    }

}