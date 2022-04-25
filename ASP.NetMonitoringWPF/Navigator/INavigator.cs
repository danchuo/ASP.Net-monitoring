using System;
using ASP.NetMonitoringWPF.ViewModels;

namespace ASP.NetMonitoringWPF.Navigator; 

public interface INavigator
{
    ViewModelBase? CurrentViewModel { get; set; }

    event Action? CurrentViewModelChanged;
}