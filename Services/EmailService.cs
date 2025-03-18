namespace JwtAuthDemo.Services;

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(string toEmail, string subject, string body)
    {
        var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(smtpSettings.FromName, smtpSettings.FromEmail));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;

        email.Body = new TextPart("plain")
        {
            Text = body
        };

        using var smtp = new SmtpClient();
        smtp.Connect(smtpSettings.Host, smtpSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(smtpSettings.Username, smtpSettings.Password);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
    
    public void SendDailyReport()
    {
        // Simulate sending a daily report email
        Console.WriteLine("Sending daily report email...");
        // Add your email sending logic here
    }
}

public class SmtpSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string FromEmail { get; set; }
    public string FromName { get; set; }
}