using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class CategoryRepo : ICategoriesRepo
    {
        public ShoppingContext Context { get; }
        public CategoryRepo(ShoppingContext _Context)
        {
            Context = _Context;
        }
        public List<Category> GetAll()
        {
           return Context.Categories.ToList();
        }
        public void Insert(Category category)
        {
            Context.Categories.Add(category);
            Context.SaveChanges();
        }

    }
}
