namespace OnlineShoppingApp.ViewModels
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}
