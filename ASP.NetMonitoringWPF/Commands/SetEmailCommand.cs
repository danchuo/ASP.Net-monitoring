using System;
using System.Windows.Input;
using ASP.NetMonitoringWPF.Converters;
using ASP.NetMonitoringWPF.Models;
using ASP.NetMonitoringWPF.ViewModels;

namespace ASP.NetMonitoringWPF.Commands;

public class SetEmailCommand : ICommand {

    private readonly NotificationViewModel _notificationViewModel;
    private readonly INotificationService _notificationService;

    public SetEmailCommand(NotificationViewModel notificationViewModel, INotificationService iNotificationService) {
        _notificationViewModel = notificationViewModel;
        _notificationService = iNotificationService;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) {
        if (parameter is not string newEmail) return;

        var isCorrect = _notificationService.SetAddress(newEmail);
        newEmail = (string) new StringToReadableNameConverter().Convert(newEmail, null, null, null);
        if (isCorrect) {
            _notificationViewModel.ErrorMessage = $"Почта {newEmail} успешно подключёна.";
        }
        else {
            _notificationViewModel.ErrorMessage = "Некорректный формат почты.";
        }
    }

    public event EventHandler? CanExecuteChanged;

}