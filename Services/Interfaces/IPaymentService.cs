using OnlineShoppingApp.Models;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Services.Interfaces
{
    public interface IPaymentService
    {
        public Order CreateOrUpdatePaymentIntent(int orderId);
        public Order UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded);
    }
}
