using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Services.Interfaces
{
    public interface ICartService
    {
        public void AddToCart(CartItemViewModel newItem);
        public List<CartItemViewModel> GetCartItems();
        public CartItemViewModel GetCartItem(int id);
        public void SaveCartItems(List<CartItemViewModel> cartItems);

        public void RemoveFromCart(int itemId);
        public void UpdateCart(int id,int Quantity);
        public int GetTotal();
        public int GetTotalPerProduct(int productId);

        public void DeleteCart();
    }
}
