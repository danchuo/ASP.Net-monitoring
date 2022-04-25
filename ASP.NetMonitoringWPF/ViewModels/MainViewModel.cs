using System.Windows.Input;
using ASP.NetMonitoringWPF.Commands;
using ASP.NetMonitoringWPF.Navigator;
using MonitoringModel = ASP.NetMonitoringWPF.Models.MonitoringModel;

namespace ASP.NetMonitoringWPF.ViewModels;

public class MainViewModel : ViewModelBase {

    private readonly INavigator _navigator;

    public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;

    public ICommand ViewGraphMonitoringCommand { get; }
    public ICommand ViewNotificationCommand { get; }


    public MainViewModel(INavigator navigator, MonitoringModel monitoringModel) {
        _navigator = navigator;
        navigator.CurrentViewModelChanged += OnCurrentViewModelChanged;

        ViewGraphMonitoringCommand =
            new NavigateCommand<GraphMonitoringViewModel>(navigator,
                () => new GraphMonitoringViewModel(monitoringModel));
        ViewNotificationCommand =
            new NavigateCommand<NotificationViewModel>(navigator,
                () => new NotificationViewModel(monitoringModel));
    }

    private void OnCurrentViewModelChanged() {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

}