using System.Collections.ObjectModel;
using System.Windows.Input;
using ASP.NetMonitoringWPF.Commands;
using ASP.NetMonitoringWPF.Models;

namespace ASP.NetMonitoringWPF.ViewModels;

public class NotificationViewModel : ViewModelBase {

    private readonly NotificationModel _notificationModel;

    public MessageViewModel ErrorMessageViewModel { get; }

    private string _email = string.Empty;
    private string _computerName;

    public bool IsNotificationEnable => _notificationModel.IsNotificationEnable;

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

    private bool _notificationCheckBox;

    public bool NotificationCheckBox {
        get => _notificationCheckBox;
        set {
            _notificationCheckBox = value;
            OnPropertyChanged(nameof(NotificationCheckBox));
            OnPropertyChanged(nameof(IsNotificationEnable));
        }
    }

    public string ComputerName {
        get => _computerName;
        set {
            _computerName = value;
            OnPropertyChanged(nameof(ComputerName));
        }
    }

    public ICommand ChangeConnectionCommand { get; }
    public ICommand DeleteTriggerCommand { get; }
    public ICommand DeleteTriggerGroupCommand { get; }
    public ICommand ChangeNotificationStatusCommand { get; }
    public ICommand SetEmailCommand { get; }

    public bool HasData => TriggerGroups.Count > 0;
    public ReadOnlyObservableCollection<TriggerGroup> TriggerGroups => _notificationModel.TriggerGroupList;

    public NotificationViewModel(DataCenter dataCenter, NotificationModel notificationModel) {
        _notificationModel = notificationModel;
        _computerName = dataCenter.CimConnection.ComputerName;
        Email = notificationModel.NotificationService.CurrentAddress;
        NotificationCheckBox = notificationModel.NotificationService.IsNotificationEnable;
        ErrorMessageViewModel = new MessageViewModel();
        ChangeConnectionCommand = new ChangeConnectionCommand(this, dataCenter);
        DeleteTriggerCommand = new DeleteTriggerCommand(notificationModel);
        DeleteTriggerGroupCommand = new DeleteTriggerGroupCommand(notificationModel);
        ChangeNotificationStatusCommand = new ChangeNotificationStatusCommand(this, notificationModel);
        SetEmailCommand = new SetEmailCommand(this, _notificationModel.NotificationService);
    }

}