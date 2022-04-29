using System.ComponentModel;

namespace ASP.NetMonitoringWPF.Models;

public class WmiProperty : INotifyPropertyChanged {

    private string _query;

    private bool _isUnable;

    public bool IsUnable {
        get => _isUnable;
        set {
            _isUnable = value;
            OnPropertyChanged(nameof(IsUnable));
        }
    }


    public string Query {
        get => _query;
        set {
            _query = value;
            OnPropertyChanged(nameof(Query));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}