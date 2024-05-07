using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface ICartRepo
    {
        public List<CartItemViewModel> GetAll();
        public CartItemViewModel GetById(int id);
        public void Create(CartItemViewModel cartItemViewModel);
        public void Update(int id,CartItemViewModel cartItemViewModel);
        public void Delete(int id);

    }
}
