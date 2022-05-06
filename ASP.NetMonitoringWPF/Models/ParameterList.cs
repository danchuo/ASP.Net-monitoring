using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.Management.Infrastructure;

namespace ASP.NetMonitoringWPF.Models;

public class ParameterList {

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
        AddProperties("Select * from Win32_PerfFormattedData_ASPNET4030319_ASPNETAppsv4030319");
        AddProperties("Select * from Win32_PerfFormattedData_ASPNET_ASPNET");
        AddProperties("Select * from Win32_PerfFormattedData_ASP_ActiveServerPages");
        AddProperties("Select * from Win32_PerfFormattedData_ASPNET_ASPNETApplications");

        // AddProperties("Select * From Win32_PerfFormattedData_PerfOS_Processor Where Name = \"_Total\"");
    }

    private void AddProperties(string query) {
        var enumerable = DataCenter.CimConnection.MakeQuery(query);

        foreach (var cimInstance in enumerable) {
            foreach (var cimInstanceProperty in cimInstance.CimInstanceProperties) {
                if (cimInstanceProperty.Value == null || (cimInstanceProperty.CimType != CimType.UInt32 &&
                                                          cimInstanceProperty.CimType != CimType.UInt64)) continue;

                if (_uniqeParameters.Contains(cimInstanceProperty.Name)) continue;
                _uniqeParameters.Add(cimInstanceProperty.Name);
                var isUnable = DataCenter.DataChangesList.Any(x => x.PropertyName == cimInstanceProperty.Name);
                _allParametersList.Add(new WmiProperty
                    {IsUnable = isUnable, Query = query.Replace("*", cimInstanceProperty.Name)});
            }
        }
    }

}