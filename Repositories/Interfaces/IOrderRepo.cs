using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface IOrderRepo
    {
        public void CreateOrder(Order order);
        public Order GetLastOrder();
        public Order GetOrderById(int id);
        public void UpdateOrder(int id,Order order);

        public Order GetWithPaymentIntent(string paymentIntentId);
    }
}
