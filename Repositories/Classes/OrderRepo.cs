using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ShoppingContext _shoppingContext;

        public OrderRepo(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }
        public void CreateOrder(Order order)
        {
            if (order != null)
            {
                _shoppingContext.Orders.Add(order);
                _shoppingContext.SaveChanges();
            }
        }

        public Order GetLastOrder()
        {
            return _shoppingContext.Orders.OrderBy(o => o.Id).LastOrDefault();
        }

        public Order GetOrderById(int id)
        {
            return _shoppingContext.Orders.Find(id);
        }

        public Order GetWithPaymentIntent(string paymentIntentId)
        {
            return _shoppingContext.Orders.FirstOrDefault(o => o.PaymentIntentId == paymentIntentId);
        }

        public void UpdateOrder(int id, Order order)
        {
            var existingOrder = _shoppingContext.Orders.Find(id);
            if (existingOrder != null)
            {
                existingOrder.ClientSecret = order.ClientSecret;
                existingOrder.PaymentIntentId = order.PaymentIntentId;
                existingOrder.Status = order.Status;
                _shoppingContext.SaveChanges();
            }
        }
    }
}
