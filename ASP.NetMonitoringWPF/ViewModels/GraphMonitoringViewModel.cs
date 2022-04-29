using System;
using System.Collections.ObjectModel;
using System.Globalization;
using ASP.NetMonitoringWPF.Models;
using LiveCharts;
using LiveCharts.Wpf;
using ObservedDataChanges = ASP.NetMonitoringWPF.Models.ObservedDataChanges;

namespace ASP.NetMonitoringWPF.ViewModels;

public class GraphMonitoringViewModel : ViewModelBase {

    public SeriesCollection SeriesCollection => _dataCenter.SeriesCollection;
    private readonly DataCenter _dataCenter;

    public bool HasData => SeriesCollection.Count > 0;

    public GraphMonitoringViewModel(DataCenter dataCenter) {
        _dataCenter = dataCenter;
        Labels = ObservedDataChanges.Times;
        YFormatter = value => value.ToString(CultureInfo.InvariantCulture);
    }

    public ReadOnlyObservableCollection<string> Labels { get; }

    public Func<double, string> YFormatter { get; set; }

}