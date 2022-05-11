using System.ComponentModel;

namespace ASP.NetMonitoringWPF.Models;

public class MonitoringTrigger : INotifyPropertyChanged {

    private const int SecondToWait = 20;

    private readonly int _timeInterval;
    private readonly double _borderValue;
    private readonly char _condition;
    private ObservedDataChanges _valueToMonitoring;

    private int _violationCounter;
    private int _activePeriod = SecondToWait;


    public char Condition => _condition;
    public double BorderValue => _borderValue;
    public int TimeInterval => _timeInterval;
    public int ViolationCounter => _violationCounter;
    public bool IsMonitoring { get; set; }
    public bool IsTriggered { get; set; }
    public string PropertyName { get; }

    public MonitoringTrigger(string propertyName, int timeInterval, double borderValue,
        char condition) {
        IsMonitoring = true;
        PropertyName = propertyName;
        _timeInterval = timeInterval;
        _borderValue = borderValue;
        _condition = condition;
    }


    public void SetValueToMonitoring(ObservedDataChanges observedDataChanges) {
        _valueToMonitoring = observedDataChanges;
        IsMonitoring = true;
        IsTriggered = false;
        _activePeriod = SecondToWait;
        _valueToMonitoring.PropertyChanged += MonitorNewValue;
        OnPropertyChanged(nameof(IsMonitoring));
    }

    public void DeleteValueToMonitoring() {
        _valueToMonitoring.PropertyChanged -= MonitorNewValue;
        _valueToMonitoring = null;
        IsMonitoring = false;
        OnPropertyChanged(nameof(IsMonitoring));
    }


    private void MonitorNewValue(object? sender, PropertyChangedEventArgs e) {
        if (e.PropertyName != "LastValue") return;
        
        if (IsTriggered) {
            _activePeriod -= 1;
        }

        if (_activePeriod == 0) {
            _activePeriod = SecondToWait;
            IsTriggered = false;
            OnPropertyChanged(nameof(IsTriggered));
            _violationCounter = 0;
        }
        else {
            if (_activePeriod != SecondToWait) {
                return;
            }
        }


        if (!IsMonitoring || e.PropertyName != nameof(_valueToMonitoring.LastValue)) return;
        var newValue = _valueToMonitoring.LastValue;

        if (_condition == '>') {
            if (newValue > _borderValue) {
                ++_violationCounter;
            }
            else {
                _violationCounter = 0;
            }
        }
        else {
            if (newValue < _borderValue) {
                ++_violationCounter;
            }
            else {
                _violationCounter = 0;
            }
        }

        if (_violationCounter >= _timeInterval) {
            IsTriggered = true;
            OnPropertyChanged(nameof(IsTriggered));
        }

        OnPropertyChanged(nameof(ViolationCounter));
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}