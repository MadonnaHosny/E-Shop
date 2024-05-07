using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.ViewModels
{
	public class ProductViewModel
	{
		public List<Product> Products { get; set; } = new List<Product>();
		public List<Category> Categories { get; set; } = new List<Category>();
		public List<Brand> Brands { get; set; } = new List<Brand>();
		public List<string> ImageUrls { get; set; } = new List<string>();
		public List<Comment> Comments { get; set; }= new List<Comment>();
		public List<Product> BestSellerProduct { get; set; }= new List<Product>();

		//public List<Rate> userRatings { get; set; } = new();
	}
}
