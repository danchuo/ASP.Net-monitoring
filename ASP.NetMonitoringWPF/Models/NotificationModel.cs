namespace ASP.NetMonitoringWPF.Models;

public class NotificationModel {

    private CimConnection _cimConnection;

    public CimConnection CimConnection {
        get => _cimConnection;

        set { _cimConnection = value; }
    }


    public NotificationModel(CimConnection cimConnection) {
        _cimConnection = cimConnection;
    }

}