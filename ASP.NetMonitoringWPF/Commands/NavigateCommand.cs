using System;
using System.Windows.Input;
using ASP.NetMonitoringWPF.Navigator;
using ASP.NetMonitoringWPF.ViewModels;

namespace ASP.NetMonitoringWPF.Commands;

public class NavigateCommand<TViewModel> : ICommand
    where TViewModel : ViewModelBase {

    private readonly INavigator _navigator;

    private readonly Func<TViewModel> _createViewModel;

    public NavigateCommand(INavigator navigator, Func<TViewModel> createViewModel) {
        _navigator = navigator;
        _createViewModel = createViewModel;
    }

    public void Execute(object? parameter) {
        _navigator.CurrentViewModel = _createViewModel();
    }

    public bool CanExecute(object? parameter) => true;

    public event EventHandler? CanExecuteChanged;

}