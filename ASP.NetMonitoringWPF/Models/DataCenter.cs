using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Geared;

namespace ASP.NetMonitoringWPF.Models;

public class DataCenter : INotifyPropertyChanged {

    public SeriesCollection SeriesCollection { get; }

    public ReadOnlyObservableCollection<ObservedDataChanges> DataChangesList { get; }

    private CimConnection _cimConnection;

    public CimConnection CimConnection {
        get => _cimConnection;

        set {
            _cimConnection = value;
            Restart();
        }
    }

    private readonly ObservableCollection<ObservedDataChanges> _data;

    public DataCenter(CimConnection cimConnection) {
        _data = new ObservableCollection<ObservedDataChanges>(new List<ObservedDataChanges>());
        DataChangesList = new ReadOnlyObservableCollection<ObservedDataChanges>(_data);
        _cimConnection = cimConnection;
        SeriesCollection = new SeriesCollection();
        ObservedDataChanges.StartUpdating();
    }

    private void Restart() {
        ObservedDataChanges.StopUpdating();

        while (_data.Count > 0) {
            DeleteVariableByName(_data[0].PropertyName);
        }

        ObservedDataChanges.StartUpdating();
        OnPropertyChanged(nameof(CimConnection));
    }

    private void AddLine(ObservedDataChanges dataChanges) {
        SeriesCollection.Add(
            new GLineSeries {
                Title = dataChanges.PropertyName,
                Values = dataChanges.Data,
                PointGeometrySize = double.NaN,
                Fill = new SolidColorBrush {Opacity = 0},
                StrokeThickness = 1.5
            });
    }


    public void AddVariable(string wmiQuery) {
        var newData = new ObservedDataChanges(_cimConnection, wmiQuery);
        _data.Add(newData);
        AddLine(newData);
    }

    public void DeleteVariableByName(string name) {
        if (_data.FirstOrDefault(x => x.PropertyName == name) == null) return;
        {
            SeriesCollection.Remove(SeriesCollection.FirstOrDefault(x => x.Title == name));
            var toDelete = _data.First(x => x.PropertyName == name);
            _data.Remove(toDelete);
            ObservedDataChanges.DeleteFromTimerList(toDelete);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}