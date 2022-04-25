using System.Windows;
using System.Windows.Controls;

namespace ASP.NetMonitoringWPF.Controls;

public partial class MaxMinCrossStackPanel : UserControl {

    private Window mainWindow;

    public MaxMinCrossStackPanel() {
        InitializeComponent();
        mainWindow = Application.Current.MainWindow;
    }

    private void CrossButton_Click(object sender, RoutedEventArgs e) {
        mainWindow.Close();
    }

    private void MaximizeButton_Click(object sender, RoutedEventArgs e) {
        mainWindow.WindowState = mainWindow.WindowState != WindowState.Maximized
            ? WindowState.Maximized
            : WindowState.Normal;
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e) {
        mainWindow.WindowState = WindowState.Minimized;
    }

}