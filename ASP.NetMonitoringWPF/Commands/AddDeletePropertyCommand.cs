using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows.Input;
using ASP.NetMonitoringWPF.Models;
using ASP.NetMonitoringWPF.ViewModels;

namespace ASP.NetMonitoringWPF.Commands;

public class AddDeletePropertyCommand : ICommand {

    private readonly DataCenter _dataCenter;

    public AddDeletePropertyCommand(DataCenter dataCenter) {
        _dataCenter = dataCenter;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) {
        if (parameter is not WmiProperty property) return;

        if (property.IsUnable) {
            _dataCenter.DeleteVariableByName(property.Query.Split(ParameterList.Delimiter)[1]);
        }
        else {
            _dataCenter.AddVariable(property.Query);
        }

        property.IsUnable = !property.IsUnable;
    }

    public event EventHandler? CanExecuteChanged;

}