namespace OnlineShoppingApp.Models
{
	public class Rate
	{
		public Product Product { get; set; }
		public int ProductId { get; set; }
		public Buyer Buyer { get; set; }
		public int BuyerId { get; set; }
		public int NumOfStars { get; set; }
	}
}
