using System.Collections.ObjectModel;
using LiveCharts;
using LiveCharts.Wpf;

namespace ConsoleApp2.Models;

public class MonitoringModel {

    public ReadOnlyObservableCollection<ObservedDataChanges> DataChangesList { get; private set; }

    private CimConnection _cimConnection;

    public CimConnection CimConnection {
        get => _cimConnection;

        set {
            _cimConnection = value;
            ObservedDataChanges.StopUpdating();
            _data.Clear();
            Init();
        }
    }

    private ObservableCollection<ObservedDataChanges> _data;

    public MonitoringModel(CimConnection cimConnection) {
        _cimConnection = cimConnection;
        Init();
    }

    public void Init() {
        _data = new ObservableCollection<ObservedDataChanges>(new List<ObservedDataChanges>());
        DataChangesList = new ReadOnlyObservableCollection<ObservedDataChanges>(_data);
        AddVariable("Select RequestsPerSec from Win32_PerfFormattedData_ASP_ActiveServerPages");
        AddVariable("Select RequestsTotal from Win32_PerfFormattedData_ASP_ActiveServerPages");
        AddVariable("Select ErrorsPerSec from Win32_PerfFormattedData_ASP_ActiveServerPages");
        AddVariable("Select RequestsQueued from Win32_PerfFormattedData_ASP_ActiveServerPages");
        AddVariable(
            "Select PercentManagedProcessorTimeestimated from Win32_PerfFormattedData_ASPNET_ASPNETApplications");
        ObservedDataChanges.StartUpdating();
    }

    public void AddVariable(string wmiQuery) => _data.Add(new ObservedDataChanges(_cimConnection, wmiQuery));

}