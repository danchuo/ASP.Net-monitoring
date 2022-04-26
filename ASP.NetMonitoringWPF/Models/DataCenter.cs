using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace ASP.NetMonitoringWPF.Models;

public class DataCenter {

    public SeriesCollection SeriesCollection { get; }

    public ReadOnlyObservableCollection<ObservedDataChanges> DataChangesList { get; }

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

    private readonly ObservableCollection<ObservedDataChanges> _data;

    public DataCenter(CimConnection cimConnection) {
        _data = new ObservableCollection<ObservedDataChanges>(new List<ObservedDataChanges>());
        DataChangesList = new ReadOnlyObservableCollection<ObservedDataChanges>(_data);
        _cimConnection = cimConnection;
        SeriesCollection = new SeriesCollection();
        Init();
    }

    private void AddLines() {
        var fillBrush = new SolidColorBrush {Opacity = 0};
        foreach (var value in _data) {
            SeriesCollection.Add(
                new LineSeries {
                    Title = value.PropertyName,
                    Values = value.Data,
                    PointGeometrySize = 0,
                    Fill = fillBrush
                });
        }
    }

    private void Init() {
        AddVariable("Select RequestsPerSec from Win32_PerfFormattedData_ASP_ActiveServerPages");
        AddVariable("Select ErrorsPerSec from Win32_PerfFormattedData_ASP_ActiveServerPages");
        AddVariable("Select RequestsQueued from Win32_PerfFormattedData_ASP_ActiveServerPages");
        AddVariable("Select ApplicationsRunning from Win32_PerfFormattedData_ASPNET_ASPNET");
        AddVariable("Select ManagedMemoryUsedestimated from Win32_PerfRawData_ASPNET4030319_ASPNETAppsv4030319");
        AddVariable(
            "Select PercentManagedProcessorTimeestimated from Win32_PerfFormattedData_ASPNET_ASPNETApplications");
        AddLines();
        ObservedDataChanges.StartUpdating();
    }

    public void AddVariable(string wmiQuery) => _data.Add(new ObservedDataChanges(_cimConnection, wmiQuery));

    public void DeleteVariableByName(string name) {
        if (_data.FirstOrDefault(x => x.PropertyName == name) != null) {
            _data.Remove(_data.First(x => x.PropertyName == name));
        }
    }

}