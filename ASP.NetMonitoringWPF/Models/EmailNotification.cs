using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Media.TextFormatting;

namespace ASP.NetMonitoringWPF.Models;

public class EmailNotification : INotificationService {

    private string _email = string.Empty;

    private readonly SmtpClient _smtpClient;


    public EmailNotification(SmtpClient smtpClient) {
        _smtpClient = smtpClient;
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

        var from = new MailAddress("asp.net.monitoring@gmail.com", "ASP.Net Monitoring");
        var to = new MailAddress(_email, "Пользователь");
        var message = new MailMessage {
            From = from,
            Body = report,
            Subject = "Критическая ситуация в работе приложения",
        };

        message.To.Add(to);
        try {
            _smtpClient?.SendMailAsync(message);
        }
        catch (Exception) {
            // ignored
        }
    }


    public static SmtpClient CreateSmtpClient(string host, int port, bool enableSsl, string username, string password) {
        var client = new SmtpClient {
            Host = host,
            Port = port,
            EnableSsl = enableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential {
                UserName = username,
                Password = password
            }
        };


        return client;
    }

}