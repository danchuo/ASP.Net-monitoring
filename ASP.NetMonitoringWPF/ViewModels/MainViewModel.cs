using System.Windows.Input;
using ASP.NetMonitoringWPF.Commands;
using ASP.NetMonitoringWPF.Models;
using ASP.NetMonitoringWPF.Navigator;

namespace ASP.NetMonitoringWPF.ViewModels;

public class MainViewModel : ViewModelBase {

    private readonly INavigator _navigator;

    public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;

    public ICommand ViewGraphMonitoringCommand { get; }
    public ICommand ViewNotificationCommand { get; }
    public ICommand ViewMetersDataCommand { get; }
    public ICommand ViewParameterListCommand { get; }
    public ICommand ViewAddTriggersCommand { get; }

    public MainViewModel(INavigator navigator, DataCenter dataCenter,INotificationService notificationService) {
        _navigator = navigator;
        navigator.CurrentViewModelChanged += OnCurrentViewModelChanged;
        var parameterList = new ParameterList(dataCenter);
        var notificationModel = new NotificationModel(dataCenter, notificationService);

        ViewGraphMonitoringCommand =
            new NavigateCommand<GraphMonitoringViewModel>(navigator,
                () => new GraphMonitoringViewModel(dataCenter));
        ViewParameterListCommand =
            new NavigateCommand<ParameterListModelView>(navigator,
                () => new ParameterListModelView(parameterList));
        ViewNotificationCommand =
            new NavigateCommand<NotificationViewModel>(navigator,
                () => new NotificationViewModel(dataCenter,notificationModel));
        ViewMetersDataCommand =
            new NavigateCommand<MetersDataViewModel>(navigator,
                () => new MetersDataViewModel(dataCenter));
        ViewAddTriggersCommand =
            new NavigateCommand<AddTriggerViewModel>(navigator,
                () => new AddTriggerViewModel(dataCenter, notificationModel));
    }

    private void OnCurrentViewModelChanged() {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

}