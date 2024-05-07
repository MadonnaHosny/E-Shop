using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class OrderItemRepo : IOrderItemRepo
    {
        private readonly ShoppingContext _shoppingContext;

        public OrderItemRepo(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }

        public List<OrderItem> GetAll(int orderId)
        {
            return _shoppingContext.OrderItems.Where(o => o.OrderId == orderId).ToList();
        }

        public OrderItem GetLast()
        {
            return _shoppingContext.OrderItems.OrderBy(o => o.Id).LastOrDefault();
        }

        public void Insert(OrderItem orderItem)
        {
            if(orderItem != null)
            {
                _shoppingContext.Add(orderItem);
                _shoppingContext.SaveChanges();
               
            }
        }
    }
}
