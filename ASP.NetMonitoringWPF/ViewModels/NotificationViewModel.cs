using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ASP.NetMonitoringWPF.Commands;
using MonitoringModel = ASP.NetMonitoringWPF.Models.MonitoringModel;
using ObservedDataChanges = ASP.NetMonitoringWPF.Models.ObservedDataChanges;

namespace ASP.NetMonitoringWPF.ViewModels;

public class NotificationViewModel : ViewModelBase {

    private readonly MonitoringModel _monitoringModel;

    public MessageViewModel ErrorMessageViewModel { get; }

    public ReadOnlyObservableCollection<ObservedDataChanges> DataChangesList => _monitoringModel.DataChangesList;

    private string _email = String.Empty;
    private string _computerName;

    public bool IsNotificationEnable { get; set; } = false;
    
    public string ErrorMessage {
        set => ErrorMessageViewModel.Message = value;
    }

    public string Email {
        get => _email;
        set {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }


    public string ComputerName {
        get => _computerName;
        set {
            _computerName = value;
            OnPropertyChanged(nameof(ComputerName));
        }
    }


    public ICommand ChangeConnectionCommand { get; set; }

    public NotificationViewModel(MonitoringModel monitoringModel) {
        _monitoringModel = monitoringModel;
        _computerName = monitoringModel.CimConnection.ComputerName;
        ErrorMessageViewModel = new MessageViewModel();
        ChangeConnectionCommand = new ChangeConnectionCommand(this, monitoringModel);
    }

}