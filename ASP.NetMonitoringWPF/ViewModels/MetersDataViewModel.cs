using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ASP.NetMonitoringWPF.Commands;
using ASP.NetMonitoringWPF.Models;

namespace ASP.NetMonitoringWPF.ViewModels;

public class MetersDataViewModel : ViewModelBase {

    private readonly DataCenter _dataCenter;

    public ReadOnlyObservableCollection<ObservedDataChanges> DataChangesList => _dataCenter.DataChangesList;
    
    public MetersDataViewModel(DataCenter dataCenter) {
        _dataCenter = dataCenter;
    }

}