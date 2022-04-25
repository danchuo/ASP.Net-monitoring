using System.Windows;
using ASP.NetMonitoringWPF.Navigator;
using ASP.NetMonitoringWPF.ViewModels;
using CimConnection = ASP.NetMonitoringWPF.Models.CimConnection;
using MonitoringModel = ASP.NetMonitoringWPF.Models.MonitoringModel;

namespace ASP.NetMonitoringWPF; 

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {

    protected override void OnStartup(StartupEventArgs e) {
        // Запуск приложения и инициализация всех сервисов.
        INavigator navigator = new Navigator.Navigator();
        var cimConnection = new CimConnection();
        var monitoringModel = new MonitoringModel(cimConnection);
        navigator.CurrentViewModel = new GraphMonitoringViewModel(monitoringModel);


        MainWindow = new MainWindow {
            MinHeight = 720,
            MinWidth = 720,
            WindowState = WindowState.Maximized,
            DataContext = new MainViewModel(navigator, monitoringModel)
        };
        
        MainWindow.Show();
        base.OnStartup(e);
    }

}