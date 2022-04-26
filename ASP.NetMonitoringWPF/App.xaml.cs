using System.Windows;
using ASP.NetMonitoringWPF.Models;
using ASP.NetMonitoringWPF.Navigator;
using ASP.NetMonitoringWPF.ViewModels;
using CimConnection = ASP.NetMonitoringWPF.Models.CimConnection;

namespace ASP.NetMonitoringWPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App {

    protected override void OnStartup(StartupEventArgs e) {
        // Запуск приложения и инициализация всех сервисов.
        INavigator navigator = new Navigator.Navigator();
        var cimConnection = new CimConnection();
        var dataCenter = new DataCenter(cimConnection);
        navigator.CurrentViewModel = new GraphMonitoringViewModel(dataCenter);

        MainWindow = new MainWindow {
            MinHeight = 720,
            MinWidth = 720,
            WindowState = WindowState.Maximized,
            DataContext = new MainViewModel(navigator, dataCenter)
        };

        MainWindow.Show();
        base.OnStartup(e);
    }

}