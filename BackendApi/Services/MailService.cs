using System.Net;
using System.Net.Mail;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json;
namespace Househole_shop.Services{
public class MailService():IEmailSender{
    public class Config
    {
        public required EmailSettings EmailSettings { get; set; }
    }

    public class EmailSettings
    {
        public required string SmtpServer { get; set; }
        public required string Port { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            using (StreamReader r = new StreamReader("D:\\Househole_shop\\mail-setting.json"))
            {
                string jsonString = r.ReadToEnd();
                Console.WriteLine(jsonString);

                var config = JsonConvert.DeserializeObject<Config>(jsonString);
                if (config?.EmailSettings == null)
                {
                    throw new ArgumentException("Email settings not found in configuration file.");
                }

                var smtpServer = config.EmailSettings.SmtpServer;
                var portString = config.EmailSettings.Port;
                var username = config.EmailSettings.Username;
                var password = config.EmailSettings.Password;

                if (int.TryParse(portString, out int port))
                {
                    if (!string.IsNullOrEmpty(smtpServer) && !string.IsNullOrEmpty(username))
                    {
                        var smtpClient = new SmtpClient
                        {
                            Host = smtpServer,
                            Port = port,
                            EnableSsl = true,
                            Credentials = new NetworkCredential(username, password)
                        };

                        var mailMessage = new MailMessage(username, email)
                        {
                            Subject = subject,
                            Body = message,
                            IsBodyHtml = true
                        };

                        await smtpClient.SendMailAsync(mailMessage);
                        Console.WriteLine("Email sent successfully.");
                    }
                    else
                    {
                        throw new ArgumentException("SMTP server or username is null or empty.");
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid port number in configuration.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }
}
}