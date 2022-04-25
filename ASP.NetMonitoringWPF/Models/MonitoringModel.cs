using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace ASP.NetMonitoringWPF.Models;

public class MonitoringModel {

    public SeriesCollection SeriesCollection { get; }

    public ReadOnlyObservableCollection<ObservedDataChanges> DataChangesList { get;  }

    private CimConnection _cimConnection;

    public CimConnection CimConnection {
        get => _cimConnection;

        set {
            _cimConnection = value;
            ObservedDataChanges.StopUpdating();
            _data.Clear();
            SeriesCollection.Clear();
            Init();
        }
    }

    private ObservableCollection<ObservedDataChanges> _data;

    public MonitoringModel(CimConnection cimConnection) {
        _data = new ObservableCollection<ObservedDataChanges>(new List<ObservedDataChanges>());
        DataChangesList = new ReadOnlyObservableCollection<ObservedDataChanges>(_data);
        _cimConnection = cimConnection;
        SeriesCollection = new SeriesCollection();
        Init();
    }

    private void AddLines() {
        foreach (var value in _data) {
            SeriesCollection.Add(
                new LineSeries {
                    Title = value.PropertyName,
                    Values = value.Data,
                    PointGeometrySize = 0
                });
        }
    }

    private void Init() {
        AddVariable("Select RequestsPerSec from Win32_PerfFormattedData_ASP_ActiveServerPages");
        AddVariable("Select RequestsTotal from Win32_PerfFormattedData_ASP_ActiveServerPages");
        AddVariable("Select ErrorsPerSec from Win32_PerfFormattedData_ASP_ActiveServerPages");
        AddVariable("Select RequestsQueued from Win32_PerfFormattedData_ASP_ActiveServerPages");
        AddVariable(
            "Select PercentManagedProcessorTimeestimated from Win32_PerfFormattedData_ASPNET_ASPNETApplications");
        AddLines();
        ObservedDataChanges.StartUpdating();
    }

    private void AddVariable(string wmiQuery) => _data.Add(new ObservedDataChanges(_cimConnection, wmiQuery));

}