using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.ViewModels
{
    public class BuyerCartViewModel
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public Buyer? Buyer { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
