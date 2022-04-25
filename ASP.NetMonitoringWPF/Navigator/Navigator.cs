using System;
using ASP.NetMonitoringWPF.ViewModels;

namespace ASP.NetMonitoringWPF.Navigator;

public class Navigator : INavigator {

    private ViewModelBase? _currentViewModel;

    public ViewModelBase? CurrentViewModel {
        get => _currentViewModel;
        set {
            _currentViewModel?.Dispose();

            _currentViewModel = value;
            CurrentViewModelChanged?.Invoke();
        }
    }

    public event Action? CurrentViewModelChanged;

}