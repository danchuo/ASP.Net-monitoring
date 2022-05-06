using System;
using System.Windows.Input;
using ASP.NetMonitoringWPF.ViewModels;

namespace ASP.NetMonitoringWPF.Commands;

public class SwitchMoreOrLessCommand : ICommand {

    private readonly AddTriggerViewModel _addTriggerViewModel;

    public SwitchMoreOrLessCommand(AddTriggerViewModel addTriggerViewModel) {
        _addTriggerViewModel = addTriggerViewModel;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) {
        _addTriggerViewModel.IsMoreThanValue = !_addTriggerViewModel.IsMoreThanValue;
    }

    public event EventHandler? CanExecuteChanged;

}