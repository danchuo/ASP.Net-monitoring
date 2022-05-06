using System.Collections.ObjectModel;
using ASP.NetMonitoringWPF.Models;

namespace ASP.NetMonitoringWPF.ViewModels;

public class MetersDataViewModel : ViewModelBase {

    private readonly DataCenter _dataCenter;

    public bool HasData => DataChangesList.Count > 0;

    public ReadOnlyObservableCollection<ObservedDataChanges> DataChangesList => _dataCenter.DataChangesList;

    public MetersDataViewModel(DataCenter dataCenter) {
        _dataCenter = dataCenter;
    }

}