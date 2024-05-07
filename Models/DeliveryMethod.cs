namespace OnlineShoppingApp.Models
{
    public class DeliveryMethod
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public decimal DeliveryCost { get; set; }
        public string DeliveryTime { get; set; }
        public string? Description { get; set; }
    }
}
