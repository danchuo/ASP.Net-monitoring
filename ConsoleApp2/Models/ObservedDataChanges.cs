using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using LiveCharts;
using Timer = System.Timers.Timer;

namespace ConsoleApp2.Models;

public class ObservedDataChanges : INotifyPropertyChanged {

    private const int Period = 60;

    private static readonly ObservableCollection<string> _times;
    private static readonly Timer _timer;
    private static int _currentPosition = -1;

    public string PropertyName { get; }
    private readonly CimConnection _cimConnection;
    private readonly string _wmiQuery;
    public ChartValues<int> Data { get; }
    public double AverageValue { get; set; }
    public int MinimalValue { get; set; }
    public int MaximalValue { get; set; }

    public static ReadOnlyObservableCollection<string> Times { get; }

    public ObservedDataChanges(CimConnection cimConnection, string wmiQuery) {
        _cimConnection = cimConnection;
        PropertyName = wmiQuery.Split()[1];
        _wmiQuery = wmiQuery;
        Data = new ChartValues<int>(new int[Period]);
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

    private static void AddSecond(object source, ElapsedEventArgs e) {
        _currentPosition = (_currentPosition + 1) % Period;
        _times[_currentPosition] = DateTime.Now.ToString("hh:mm:ss");
    }


    private void UpdateData(object source, ElapsedEventArgs e) {
        var value = _cimConnection.GetPropertyValue(PropertyName, _wmiQuery);
        Data[_currentPosition] = value;

        MaximalValue = Data.Max();
        MinimalValue = Data.Min();
        AverageValue = Data.Average();

        OnPropertyChanged(nameof(MaximalValue));
        OnPropertyChanged(nameof(MinimalValue));
        OnPropertyChanged(nameof(AverageValue));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}