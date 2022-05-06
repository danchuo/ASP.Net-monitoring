using System;
using System.Windows.Input;
using ASP.NetMonitoringWPF.Models;

namespace ASP.NetMonitoringWPF.Commands;

public class DeleteTriggerCommand : ICommand {
    
    private readonly NotificationModel _notificationModel;

    public DeleteTriggerCommand(NotificationModel notificationModel) {
        _notificationModel = notificationModel;
    }

    public bool CanExecute(object? parameter) => true;


    public void Execute(object? parameter) {
        if (parameter is not ValueTuple<MonitoringTrigger, TriggerGroup> trigger) return;
        trigger.Item2.DeleteTrigger(trigger.Item1);
        if (trigger.Item2.TriggerList.Count == 0) {
            _notificationModel.DeleteTriggerGroup(trigger.Item2);
        }
    }

    public event EventHandler? CanExecuteChanged;

}