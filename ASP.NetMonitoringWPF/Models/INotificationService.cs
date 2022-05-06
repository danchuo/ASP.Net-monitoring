namespace ASP.NetMonitoringWPF.Models;

public interface INotificationService {

    bool IsAddressSet { get; set; }

    bool IsNotificationEnable { get; set; }

    string CurrentAddress { get; }

    bool SetAddress(string address);
    void SendMessage(string message);

}