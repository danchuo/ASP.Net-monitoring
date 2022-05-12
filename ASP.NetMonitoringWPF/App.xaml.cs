using System;
using System.Configuration;
using System.Net.Mail;
using System.Windows;
using ASP.NetMonitoringWPF.Models;
using ASP.NetMonitoringWPF.Navigator;
using ASP.NetMonitoringWPF.ViewModels;

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
        INotificationService notificationService;
        
        try {
            notificationService =
                new EmailNotification(EmailNotification.CreateSmtpClient(ConfigurationManager.AppSettings["host"],
                    int.Parse(ConfigurationManager.AppSettings["port"]),
                    bool.Parse(ConfigurationManager.AppSettings["enableSsl"]),
                    ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]));
        }
        catch (Exception) {
            notificationService = new EmailNotification(new SmtpClient());
        }

        navigator.CurrentViewModel = new GraphMonitoringViewModel(dataCenter);

        MainWindow = new MainWindow {
            MinHeight = 720,
            MinWidth = 720,
            WindowState = WindowState.Maximized,
            DataContext = new MainViewModel(navigator, dataCenter, notificationService)
        };

        MainWindow.Show();
        base.OnStartup(e);
    }

}