using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class BrandRepo : IBrandRepo
    {
        public ShoppingContext Context { get; }
        public BrandRepo(ShoppingContext _Context)
        {
            Context = _Context;
        }
        public List<Brand> GetAll()
        {
           
            return Context.Brands.ToList();

        }

        public void Insert(Brand brand)
        {
            Context.Brands.Add(brand);
            Context.SaveChanges();
        }
    }
}
