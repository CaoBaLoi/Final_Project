using System.Net;
using System.Net.Mail;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace Grocery.Services{
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
        // Đọc tệp JSON
            string jsonString = File.ReadAllText("D:\\Grocery\\mail-setting.json");

            // Định nghĩa lớp EmailSettings để ánh xạ với JSON
            var config = JsonSerializer.Deserialize<Config>(jsonString);
            if (config?.EmailSettings == null)
            {
                throw new ArgumentException("Email settings not found in configuration file.");
            }
            // Gán giá trị cho các biến
            var smtpServer = config.EmailSettings.SmtpServer;
            var portString = config.EmailSettings.Port;
            var username = config.EmailSettings.Username;
            var password = config.EmailSettings.Password;

        if (int.TryParse(portString, out int port))
        {
            if(smtpServer != null && username != null){
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
            }
            else{
                throw new ArgumentException("SMTP server or username is null.");
            }
        }
        else
        {
            // Xử lý trường hợp port không hợp lệ
            throw new ArgumentException("Invalid port number in configuration.");
        }
    }
}
}