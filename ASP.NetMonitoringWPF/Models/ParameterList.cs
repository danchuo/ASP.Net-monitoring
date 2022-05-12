using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace ASP.NetMonitoringWPF.Models;

public class ParameterList {

    public const char Delimiter = '*';
    public DataCenter DataCenter { get; }
    public ReadOnlyObservableCollection<WmiProperty> AllParametersList { get; private set; }

    private ObservableCollection<WmiProperty> _allParametersList;

    private HashSet<string> _uniqeParameters;

    public ParameterList(DataCenter dataCenter) {
        DataCenter = dataCenter;
        InitParametersList(null, null);
        DataCenter.PropertyChanged += InitParametersList;
    }

    private void InitParametersList(object? sender, PropertyChangedEventArgs e) {
        _uniqeParameters = new HashSet<string>();
        _allParametersList = new ObservableCollection<WmiProperty>(new List<WmiProperty>());
        AllParametersList = new ReadOnlyObservableCollection<WmiProperty>(_allParametersList);

        AddProperties("ASP.NET Apps v4.0.30319");
    }

    private void AddProperties(string wmiClass) {
        var connection = DataCenter.CimConnection.ComputerName == "localhost"
            ? PerformanceCounterCategory.GetCategories()
            : PerformanceCounterCategory.GetCategories(DataCenter.CimConnection.ComputerName);
        var counters = connection.FirstOrDefault(x => x.CategoryName.Contains(wmiClass))
            ?.GetCounters("__Total__");

        foreach (var performanceCounter in counters) {
            if (_uniqeParameters.Contains(performanceCounter.CounterName)) continue;
            var isUnable = DataCenter.DataChangesList.Any(x => x.PropertyName == performanceCounter.CounterName);
            _allParametersList.Add(new WmiProperty
                {IsUnable = isUnable, Query = wmiClass + Delimiter + performanceCounter.CounterName});
        }
    }

}