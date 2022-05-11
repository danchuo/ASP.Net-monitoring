using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ASP.NetMonitoringWPF.Converters;

namespace ASP.NetMonitoringWPF.Models;

public class NotificationModel {

    private readonly DataCenter _dataCenter;

    public bool IsNotificationEnable => _notificationService.IsNotificationEnable;

    private readonly INotificationService _notificationService;
    public INotificationService NotificationService => _notificationService;

    public ReadOnlyObservableCollection<TriggerGroup> TriggerGroupList { get; }
    private readonly ObservableCollection<TriggerGroup> _triggerGroupList;


    public NotificationModel(DataCenter dataCenter, INotificationService notificationService) {
        _dataCenter = dataCenter;
        _notificationService = notificationService;
        _triggerGroupList = new ObservableCollection<TriggerGroup>(new List<TriggerGroup>());
        TriggerGroupList = new ReadOnlyObservableCollection<TriggerGroup>(_triggerGroupList);
        ((INotifyCollectionChanged) dataCenter.DataChangesList).CollectionChanged += UpdateAvailableProperties;
    }

    public void AddTriggerGroup(TriggerGroup triggerGroup) {
        _triggerGroupList.Add(triggerGroup);
        triggerGroup.GroupTriggered += SendNotification;
    }

    public void DeleteTriggerGroup(TriggerGroup triggerGroup) {
        _triggerGroupList.Remove(triggerGroup);
        triggerGroup.DeleteAllTriggers();
        triggerGroup.GroupTriggered -= SendNotification;
    }


    private void SendNotification(object? sender, EventArgs e) {
        var groupName = (sender as TriggerGroup)?.GroupName;
        if (groupName != null)
            _notificationService.SendMessage(GenerateReport(sender as TriggerGroup));
    }

    private void UpdateAvailableProperties(object? sender, NotifyCollectionChangedEventArgs e) {
        if (e.NewItems != null) {
            foreach (ObservedDataChanges item in e.NewItems) {
                _triggerGroupList.SelectMany(x => x.TriggerList)
                    .Where(x => x.PropertyName == item.PropertyName).All(y => {
                        y.SetValueToMonitoring(item);
                        return true;
                    });
            }
        }

        if (e.OldItems == null) return;
        {
            foreach (ObservedDataChanges item in e.OldItems) {
                _triggerGroupList.SelectMany(x => x.TriggerList)
                    .Where(x => x.PropertyName == item.PropertyName).All(y => {
                        y.DeleteValueToMonitoring();
                        return true;
                    });
            }
        }
    }


    private string GenerateReport(TriggerGroup triggerGroup) {
        var report = new StringBuilder();
        report.Append($"Компьютер {_dataCenter.CimConnection.ComputerName}\n" +
                      $"Сработала группа триггеров \"{triggerGroup.GroupName}\" в {DateTime.Now}.\n" +
                      $"Все наблюдаемые свойства вышли из допустимого диапазона.\n\n");

        foreach (var trigger in triggerGroup.TriggerList) {
            report.Append(
                $"Своство {trigger.PropertyName} было {(trigger.Condition == '<' ? "ниже" : "выше")} " +
                $"допустимого значения {trigger.BorderValue} в течение {new SecondsToMinutesConverter().Convert(trigger.TimeInterval, null, null, null)}.\n");
        }

        return report.ToString();
    }
}