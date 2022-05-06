using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ASP.NetMonitoringWPF.Commands;
using ASP.NetMonitoringWPF.Models;

namespace ASP.NetMonitoringWPF.ViewModels;

public class AddTriggerViewModel : ViewModelBase {

    private const string CreateNewGroupName = "Создать новую группу";

    private const int MinimalBorderValue = 4;

    private readonly DataCenter _dataCenter;
    private readonly NotificationModel _notificationModel;

    public bool HasData => DataChangesList.Count > 0;

    public bool CanCreateTrigger =>
        (BorderValue > MinimalBorderValue && GroupNameSelectedIndex == 0 && NewGroupName != string.Empty) ||
        (GroupNameSelectedIndex != 0 && BorderValue > MinimalBorderValue);

    private int _groupNameSelectedIndex;

    public int GroupNameSelectedIndex {
        get => _groupNameSelectedIndex;
        set {
            _groupNameSelectedIndex = value;
            OnPropertyChanged(nameof(GroupNameSelectedIndex));
            OnPropertyChanged(nameof(CanCreateTrigger));
        }
    }

    private ObservedDataChanges _propertyNameSelectedItem;

    public ObservedDataChanges PropertyNameSelectedItem {
        get => _propertyNameSelectedItem;
        set {
            _propertyNameSelectedItem = value;
            OnPropertyChanged(nameof(PropertyNameSelectedItem));
        }
    }


    private int _sliderValue;

    public int SliderValue {
        get => _sliderValue;
        set {
            _sliderValue = value;
            OnPropertyChanged(nameof(SliderValue));
        }
    }

    private string _newGroupName = string.Empty;

    public string NewGroupName {
        get => _newGroupName;
        set {
            _newGroupName = value;
            OnPropertyChanged(nameof(NewGroupName));
            OnPropertyChanged(nameof(CanCreateTrigger));
        }
    }

    private bool _isMoreThanValue;

    public bool IsMoreThanValue {
        get => _isMoreThanValue;
        set {
            _isMoreThanValue = value;
            OnPropertyChanged(nameof(IsMoreThanValue));
        }
    }


    private uint _borderValue;

    public uint BorderValue {
        get => _borderValue;
        set {
            _borderValue = value;
            OnPropertyChanged(nameof(BorderValue));
            OnPropertyChanged(nameof(CanCreateTrigger));
        }
    }
    
    public ReadOnlyObservableCollection<ObservedDataChanges> DataChangesList => _dataCenter.DataChangesList;

    public ReadOnlyObservableCollection<string> TriggerGroupList { get; set; }

    public ICommand SwitchMoreOrLessCommand { get; }
    public ICommand AddTriggerCommand { get; }

    public AddTriggerViewModel(DataCenter dataCenter, NotificationModel notificationModel) {
        _dataCenter = dataCenter;
        _notificationModel = notificationModel;
        SwitchMoreOrLessCommand = new SwitchMoreOrLessCommand(this);
        AddTriggerCommand = new AddTriggerCommand(notificationModel, this);
        InitValues();
    }

    public void InitValues() {
        var list = new List<string> {CreateNewGroupName};
        list.AddRange(_notificationModel.TriggerGroupList.Where(x => x.CanAdd).Select(x => x.GroupName));
        TriggerGroupList = new ReadOnlyObservableCollection<string>(new ObservableCollection<string>(list));
        NewGroupName = string.Empty;
        GroupNameSelectedIndex = 0;
        if (HasData) {
            PropertyNameSelectedItem = DataChangesList.First();
        }

        IsMoreThanValue = true;
        BorderValue = 10;
        SliderValue = 5;
        OnPropertyChanged(nameof(CanCreateTrigger));
        OnPropertyChanged(nameof(TriggerGroupList));
    }

}