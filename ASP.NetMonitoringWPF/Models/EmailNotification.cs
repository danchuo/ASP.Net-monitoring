using System;
using System.Net;
using System.Net.Mail;

namespace ASP.NetMonitoringWPF.Models;

public class EmailNotification : INotificationService {

    private string _email = string.Empty;

    private readonly string _username;
    private readonly string _password;

    public EmailNotification(string username, string password) {
        _username = username;
        _password = password;
    }
    
    public bool IsAddressSet { get; set; }
    public bool IsNotificationEnable { get; set; } = false;
    public string CurrentAddress => _email;

    public bool SetAddress(string address) {
        try {
            var m = new MailAddress(address);
            _email = address;
            IsAddressSet = true;
            return true;
        }
        catch (Exception) {
            return false;
        }
    }

    public void SendMessage(string report) {
        if (!IsAddressSet || !IsNotificationEnable) return;
       
        var client = new SmtpClient {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential {
                UserName = _username,
                Password = _password
            }
        };

        var from = new MailAddress(_username, "ASP.Net Monitoring");
        var to = new MailAddress(_email, "Пользователь");
        var message = new MailMessage {
            From = from,
            Body = report,
            Subject = "Критическая ситуация в работе приложения",
        };

        message.To.Add(to);

        client.SendMailAsync(message);
    }


}