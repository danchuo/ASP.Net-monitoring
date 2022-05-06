using System;
using System.Windows.Input;
using ASP.NetMonitoringWPF.Models;
using ASP.NetMonitoringWPF.ViewModels;

namespace ASP.NetMonitoringWPF.Commands;

public class ChangeNotificationStatusCommand : ICommand {

    private readonly NotificationViewModel _notificationViewModel;
    private readonly NotificationModel _notificationModel;

    public ChangeNotificationStatusCommand(NotificationViewModel notificationViewModel,
        NotificationModel notificationModel) {
        _notificationViewModel = notificationViewModel;
        _notificationModel = notificationModel;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) {
        if (_notificationViewModel.NotificationCheckBox) {
            _notificationViewModel.NotificationCheckBox = false;
            _notificationModel.NotificationService.IsNotificationEnable = false;
        }
        else {
            if (_notificationModel.NotificationService.IsAddressSet) {
                _notificationViewModel.NotificationCheckBox = true;
                _notificationModel.NotificationService.IsNotificationEnable = true;
            }
            else {
                _notificationViewModel.ErrorMessage = "Необходимо привязать почту.";
            }
        }
    }

    public event EventHandler? CanExecuteChanged;

}