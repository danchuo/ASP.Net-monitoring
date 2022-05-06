using System;
using System.Windows.Input;
using ASP.NetMonitoringWPF.Models;

namespace ASP.NetMonitoringWPF.Commands;

public class DeleteTriggerGroupCommand : ICommand {

    private readonly NotificationModel _notificationModel;

    public DeleteTriggerGroupCommand(NotificationModel notificationModel) {
        _notificationModel = notificationModel;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) {
        if (parameter is not TriggerGroup group) return;
        _notificationModel.DeleteTriggerGroup(group);
    }

    public event EventHandler? CanExecuteChanged;

}