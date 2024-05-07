using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class CartRepo : ICartRepo
    {
        private readonly List<CartItemViewModel> _cartItems;

        public CartRepo()
        {
            _cartItems = new List<CartItemViewModel>();
        }
        public void Create(CartItemViewModel cartItemViewModel)
        {
            // Generate a unique id for the new item
            cartItemViewModel.Id = _cartItems.Count + 1;

            _cartItems.Add(cartItemViewModel);
        }

        public void Delete(int id)
        {
            var itemToRemove = _cartItems.FirstOrDefault(item => item.Id == id);
            if (itemToRemove != null)
            {
                _cartItems.Remove(itemToRemove);
            }
        }

        public List<CartItemViewModel> GetAll()
        {
            return _cartItems.ToList();
        }

        public CartItemViewModel GetById(int id)
        {
            return _cartItems.Find(item => item.Id == id);
        }

        public void Update(int id, CartItemViewModel cartItemViewModel)
        {
            var existingItem = _cartItems.Find(item => item.Id == id);
            if (existingItem != null)
            {
                existingItem.ProductName = cartItemViewModel.ProductName;
                existingItem.PictureUrl = cartItemViewModel.PictureUrl;
                existingItem.Price = cartItemViewModel.Price;
                existingItem.Quantity = cartItemViewModel.Quantity;
                existingItem.Brand = cartItemViewModel.Brand;
                existingItem.Category = cartItemViewModel.Category;
            }
        }
    }
}
