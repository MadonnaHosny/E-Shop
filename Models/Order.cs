using OnlineShoppingApp.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShoppingApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Buyer))]
        public int BuyerId { get; set; }
        public Buyer? Buyer { get; set; }
        public DateTimeOffset OderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        [NotMapped]
        public decimal Total { get { return SubTotal + DeliveryMethod.DeliveryCost; } }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
