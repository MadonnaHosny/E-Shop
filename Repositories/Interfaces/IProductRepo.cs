using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface IProductRepo
    {
        public List<Product> GetAll();

        public Product GetById(int id);

        public List<Product> GetByName(string Name);
        public List<Product> GetProductsStartingWith(string search);

		public void Edit(int id,Product newProduct,int userId, List<IFormFile> ImageUrl);
        public void insertImage(List<IFormFile> ImageUrl, int productID);
        public void DeleteImage(Images image, string path);
        public void Insert(Product product,int userId, List<IFormFile> ImageUrl);
        public void Delete(Product product, int userId);

        public List<Product> GetProductsPerSeller(int sellerId);

        public List<Product> GetBestSellingProducts();
        public AppUser GetProductSellerId(int prodID);
        public void UpdateProductQuantity(int prodID, Product product);
    }
}
