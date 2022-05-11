using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ASP.NetMonitoringWPF.Models;

public class TriggerGroup {

    private const int MaxAmountOfTriggers = 3;

    private readonly ObservableCollection<MonitoringTrigger> _triggerList;

    public ReadOnlyObservableCollection<MonitoringTrigger> TriggerList { get; }

    public bool CanAdd => TriggerList.Count < MaxAmountOfTriggers;

    public string GroupName { get; }

    public event EventHandler? GroupTriggered;


    public TriggerGroup(string groupName) {
        GroupName = groupName;
        _triggerList = new ObservableCollection<MonitoringTrigger>(new List<MonitoringTrigger>());
        TriggerList = new ReadOnlyObservableCollection<MonitoringTrigger>(_triggerList);
    }

    public void AddTrigger(MonitoringTrigger monitoringTrigger) {
        if (!CanAdd) return;
        _triggerList.Add(monitoringTrigger);
        monitoringTrigger.PropertyChanged += CheckTriggers;
    }

    private void CheckTriggers(object? sender, PropertyChangedEventArgs e) {
        if (e.PropertyName == "IsTriggered" && _triggerList.All(x => x.IsTriggered)) {
            OnGroupTriggered();
        }
    }


    public void DeleteTrigger(MonitoringTrigger monitoringTrigger) {
        _triggerList.Remove(monitoringTrigger);
        monitoringTrigger.PropertyChanged -= CheckTriggers;
    }


    public void DeleteAllTriggers() {
        while (_triggerList.Count > 0) {
            var trigger = _triggerList[0];
            trigger.PropertyChanged -= CheckTriggers;
            _triggerList.Remove(trigger);
        }
    }


    private void OnGroupTriggered() {
        GroupTriggered?.Invoke(this, EventArgs.Empty);
    }
}