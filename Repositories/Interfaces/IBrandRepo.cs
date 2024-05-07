using Microsoft.EntityFrameworkCore.Migrations.Operations;
using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface IBrandRepo
    {
        public List<Brand> GetAll();
        //public Brand GetById(int id);
        public void Insert(Brand brand);
    }
}
