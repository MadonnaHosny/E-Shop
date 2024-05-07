using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface IBuyerCartRepo
    {
        //public List<BuyerCartViewModel> GetAll();
        //public void Create(BuyerCartViewModel buyerCartViewModel);
        public BuyerCartViewModel GetById(int id);
        public void Update(int id, BuyerCartViewModel buyerCartViewModel);
        public void Delete(int id);
    }
}
