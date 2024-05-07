using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Services.Interfaces
{
    public interface IBuyerCartService
    {
        public BuyerCartViewModel GetBuyerCart(int id);
    }
}
