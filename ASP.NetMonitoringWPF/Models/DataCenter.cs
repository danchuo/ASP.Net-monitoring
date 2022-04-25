namespace ASP.NetMonitoringWPF.Models;

public class DataCenter {

    private CimConnection _cimConnection;

    public CimConnection CimConnection {
        get => _cimConnection;

        set { _cimConnection = value; }
    }

    public DataCenter(CimConnection cimConnection) {
        _cimConnection = cimConnection;
    }

}