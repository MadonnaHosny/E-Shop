using OnlineShoppingApp.ConfigurationClasses;

namespace OnlineShoppingApp.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailMessage email);

    }
}
