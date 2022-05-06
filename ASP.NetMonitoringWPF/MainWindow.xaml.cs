using System.Windows.Input;

namespace ASP.NetMonitoringWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow {

    public MainWindow() {
        InitializeComponent();
    }

    private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e) {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

}