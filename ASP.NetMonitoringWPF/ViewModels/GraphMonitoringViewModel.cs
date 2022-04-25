using System;
using System.Collections.ObjectModel;
using System.Globalization;
using LiveCharts;
using LiveCharts.Wpf;
using MonitoringModel = ASP.NetMonitoringWPF.Models.MonitoringModel;
using ObservedDataChanges = ASP.NetMonitoringWPF.Models.ObservedDataChanges;

namespace ASP.NetMonitoringWPF.ViewModels;

public class GraphMonitoringViewModel : ViewModelBase {

    public SeriesCollection SeriesCollection => _monitoringModel.SeriesCollection;
    private readonly MonitoringModel _monitoringModel;

    public GraphMonitoringViewModel(MonitoringModel monitoringModel) {
        _monitoringModel = monitoringModel;
        Labels = ObservedDataChanges.Times;
        YFormatter = value => value.ToString(CultureInfo.InvariantCulture);
        //AddLines();
    }

    public ReadOnlyObservableCollection<string> Labels { get; }

    public Func<double, string> YFormatter { get; set; }

}