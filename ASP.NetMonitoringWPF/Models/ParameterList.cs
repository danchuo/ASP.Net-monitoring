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

    public ParameterList(DataCenter dataCenter) {
        DataCenter = dataCenter;
        FillParametersList(null, null);
        DataCenter.PropertyChanged += FillParametersList;
    }

    private void FillParametersList(object? sender, PropertyChangedEventArgs e) {
        _allParametersList = new ObservableCollection<WmiProperty>(new List<WmiProperty>());
        AllParametersList = new ReadOnlyObservableCollection<WmiProperty>(_allParametersList);
        AddProperties("Select * from Win32_PerfRawData_ASPNET4030319_ASPNETAppsv4030319");
        AddProperties("Select * from Win32_PerfFormattedData_ASPNET_ASPNET");
        AddProperties("Select * from Win32_PerfFormattedData_ASP_ActiveServerPages");
        AddProperties("Select * from Win32_PerfFormattedData_ASPNET_ASPNETApplications");
        //    AddProperties("Select * From Win32_PerfFormattedData_PerfOS_Processor Where Name = \"_Total\"");
    }

    private void AddProperties(string query) {
        var enumerable = DataCenter.CimConnection.MakeQuery(query);

        foreach (var cimInstance in enumerable) {
            foreach (var cimInstanceProperty in cimInstance.CimInstanceProperties) {
                if (cimInstanceProperty.Value == null || (cimInstanceProperty.CimType != CimType.UInt32 &&
                                                          cimInstanceProperty.CimType != CimType.UInt64)) continue;
                var isParse = double.TryParse(cimInstanceProperty.Value.ToString() ?? string.Empty, out var testValue);

                if (!isParse || testValue > int.MaxValue) continue;
                var isUnable = DataCenter.DataChangesList.Any(x => x.PropertyName == cimInstanceProperty.Name);
                _allParametersList.Add(new WmiProperty
                    {IsUnable = isUnable, Query = query.Replace("*", cimInstanceProperty.Name)});
            }
        }
    }

}