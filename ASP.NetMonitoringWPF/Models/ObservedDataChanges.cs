using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Timers;
using LiveCharts;
using Timer = System.Timers.Timer;

namespace ASP.NetMonitoringWPF.Models;

public class ObservedDataChanges : INotifyPropertyChanged {

    private const int Period = 120;

    private static readonly ObservableCollection<string> _times;
    private static readonly Timer _timer;
    private static int _currentPosition = -1;

    private readonly CimConnection _cimConnection;
    private readonly string _wmiQuery;
    public string PropertyName { get; }
    public ChartValues<double> Data { get; }
    public double AverageValue { get; set; }
    public double MinimalValue { get; set; }
    public double MaximalValue { get; set; }

    public static ReadOnlyObservableCollection<string> Times { get; }

    public ObservedDataChanges(CimConnection cimConnection, string wmiQuery) {
        _cimConnection = cimConnection;
        PropertyName = wmiQuery.Split()[1];
        _wmiQuery = wmiQuery;

        var array = new double[Period];
        for (var i = 0; i < Period; i++) {
            array[i] = double.NaN;
        }

        Data = new ChartValues<double>(array);
        _timer.Elapsed += UpdateData;
    }

    static ObservedDataChanges() {
        _timer = new Timer(1000);
        _timer.Elapsed += AddSecond;

        _times = new ObservableCollection<string>(new string[Period]);
        Times = new ReadOnlyObservableCollection<string>(_times);
    }

    public static void StopUpdating() {
        _timer.Enabled = false;
        _currentPosition = -1;
    }

    public static void StartUpdating() {
        Thread.Sleep(1000 - DateTime.Now.Millisecond);
        _timer.Enabled = true;
    }


    public static void DeleteFromTimerList(ObservedDataChanges value) {
        _timer.Elapsed -= value.UpdateData;
    }

    private static void AddSecond(object? source, ElapsedEventArgs e) {
        _currentPosition = (_currentPosition + 1) % Period;
        _times[_currentPosition] = DateTime.Now.ToString("hh:mm:ss");
    }

    private void UpdateData(object? source, ElapsedEventArgs e) {
        var value = _cimConnection.GetPropertyValue(PropertyName, _wmiQuery);
        Data[(_currentPosition + 1) % Period] = double.NaN;
        Data[(_currentPosition + 2) % Period] = double.NaN;
        Data[(_currentPosition + 3) % Period] = double.NaN;
        Data[_currentPosition] = value;

        MaximalValue = Data.Where(x => !double.IsNaN(x)).Max();
        MinimalValue = Data.Where(x => !double.IsNaN(x)).Min();
        AverageValue = Data.Where(x => !double.IsNaN(x)).Average();

        OnPropertyChanged(nameof(MaximalValue));
        OnPropertyChanged(nameof(MinimalValue));
        OnPropertyChanged(nameof(AverageValue));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}