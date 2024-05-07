using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.Repositories.Interfaces
{
	public interface IRateRepo
	{
		public bool ProductExist(int productId);
		public void Rate(int ProductId, int UserId, int NumOfStars);
		public int GetRateForUser(int productId, int userId);
		public int GetAvgRateForProduct(int productId);
		public List<Rate> GetAllRatesForProduct(int productId);

    }
}
