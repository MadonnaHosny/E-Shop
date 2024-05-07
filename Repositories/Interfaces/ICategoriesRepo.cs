using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface ICategoriesRepo
    {


        public List<Category> GetAll();
        public void Insert(Category category);

    }
}
