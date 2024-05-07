using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface IOrderItemRepo
    {
        public void Insert(OrderItem orderItem);
        public OrderItem GetLast();

        public List<OrderItem> GetAll(int orderId);
    }
}
