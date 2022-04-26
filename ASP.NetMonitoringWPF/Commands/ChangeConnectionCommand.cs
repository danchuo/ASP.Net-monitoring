using System;
using System.Linq;
using System.Windows.Input;
using ASP.NetMonitoringWPF.Models;
using ASP.NetMonitoringWPF.ViewModels;
using Microsoft.Management.Infrastructure;

namespace ASP.NetMonitoringWPF.Commands;

public class ChangeConnectionCommand : ICommand {

    private readonly NotificationViewModel _notificationViewModel;
    private readonly DataCenter _dataCenter;

    public ChangeConnectionCommand(NotificationViewModel notificationViewModel, DataCenter dataCenter) {
        _notificationViewModel = notificationViewModel;
        _dataCenter = dataCenter;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) {
        if (parameter is not string computerName) return;

        try {
            var cimConnection = new CimConnection(computerName);
            var ans = cimConnection.MakeQuery(
                "Select RequestsPerSec from Win32_PerfFormattedData_ASP_ActiveServerPages").FirstOrDefault();
            _dataCenter.CimConnection = cimConnection;
            computerName = computerName.Length > 15 ? computerName[..15] + "..." : computerName;
            _notificationViewModel.ErrorMessage = $"Компьютер {computerName} успешно подключён.";
        }
        catch (Exception) {
            _notificationViewModel.ErrorMessage = "Компьютер с таким именем не найден.";
        }
    }

    public event EventHandler? CanExecuteChanged;

}