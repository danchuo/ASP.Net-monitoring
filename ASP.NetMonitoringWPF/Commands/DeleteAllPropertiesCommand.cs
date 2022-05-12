using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ASP.NetMonitoringWPF.Models;

namespace ASP.NetMonitoringWPF.Commands;

public class DeleteAllPropertiesCommand : ICommand {

    private readonly DataCenter _dataCenter;

    public DeleteAllPropertiesCommand(DataCenter dataCenter) {
        _dataCenter = dataCenter;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) {
        if (parameter is not ReadOnlyObservableCollection<WmiProperty> allParametersList) return;

        foreach (var property in allParametersList) {
            if (!property.IsUnable) continue;
            _dataCenter.DeleteVariableByName(property.Query.Split(ParameterList.Delimiter)[1]);
            property.IsUnable = !property.IsUnable;
        }
    }

    public event EventHandler? CanExecuteChanged;

}