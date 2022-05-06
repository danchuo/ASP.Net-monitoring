using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ASP.NetMonitoringWPF.Models;
using ASP.NetMonitoringWPF.ViewModels;

namespace ASP.NetMonitoringWPF.Commands;

public class AddTriggerCommand : ICommand {
    
    private readonly NotificationModel _notificationModel;
    private readonly AddTriggerViewModel _addTriggerViewModel;

    public AddTriggerCommand(NotificationModel notificationModel,
        AddTriggerViewModel addTriggerViewModel) {
         _notificationModel = notificationModel;
        _addTriggerViewModel = addTriggerViewModel;
        addTriggerViewModel.PropertyChanged += AddTriggerViewModel_PropertyChanged;
    }

    public bool CanExecute(object? parameter) => _addTriggerViewModel.CanCreateTrigger;

    private void AddTriggerViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
        if (e.PropertyName == nameof(AddTriggerViewModel.CanCreateTrigger)) {
            CanExecuteChanged?.Invoke(sender, e);
        }
    }


    public void Execute(object? parameter) {
        var triggerGroup = _addTriggerViewModel.GroupNameSelectedIndex == 0
            ? new TriggerGroup(_addTriggerViewModel.NewGroupName)
            : _notificationModel.TriggerGroupList.First(x =>
                x.GroupName ==
                _addTriggerViewModel.TriggerGroupList.ElementAt(_addTriggerViewModel.GroupNameSelectedIndex));

        var trigger = new MonitoringTrigger(_addTriggerViewModel.PropertyNameSelectedItem.PropertyName,
            _addTriggerViewModel.SliderValue,
            _addTriggerViewModel.BorderValue, _addTriggerViewModel.IsMoreThanValue ? '>' : '<');
        trigger.SetValueToMonitoring(_addTriggerViewModel.PropertyNameSelectedItem);
        triggerGroup.AddTrigger(trigger);

        if (_addTriggerViewModel.GroupNameSelectedIndex == 0) {
            _notificationModel.AddTriggerGroup(triggerGroup);
        }

        _addTriggerViewModel.InitValues();
    }

    public event EventHandler? CanExecuteChanged;

}