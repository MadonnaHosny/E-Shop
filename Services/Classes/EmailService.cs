using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using OnlineShoppingApp.ConfigurationClasses;
using OnlineShoppingApp.Services.Interfaces;
using MailKit.Net.Smtp;

namespace OnlineShoppingApp.Services.Classes
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailMessage emailMessage)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailConfiguration:EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(emailMessage.To));
            email.Subject = emailMessage.Subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Body
            };
            using var smtp = new SmtpClient();
            smtp.Connect(
                _config.GetSection("EmailConfiguration:EmailHost").Value,
                int.Parse(_config.GetSection("EmailConfiguration:Port").Value),
                SecureSocketOptions.StartTls);

            smtp.Authenticate(
                _config.GetSection("EmailConfiguration:EmailUsername").Value,
                _config.GetSection("EmailConfiguration:EmailPassword").Value
            );
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
