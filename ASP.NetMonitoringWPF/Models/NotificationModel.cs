using System.ComponentModel;

namespace ASP.NetMonitoringWPF.Models;

public class NotificationModel {

    private DataCenter _dataCenter;
    
    public bool IsNotificationEnable { get; set; } = false;

    public NotificationModel(DataCenter dataCenter) {
        _dataCenter = dataCenter;
    }

}