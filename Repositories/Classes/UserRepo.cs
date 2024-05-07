using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class UserRepo : IUserRepo
    {
        ShoppingContext _context { get; }
        public UserRepo(ShoppingContext context)
        {
            _context = context;
        }

        public bool EmailExist(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
        public bool UsernameExist(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }

        public int? GetUserRoleID(int id)
        {
            return _context.UserRoles.FirstOrDefault(u => u.UserId == id)?.RoleId;
        }

        public string GetUserRole(int id)
        {
            return _context.Roles.FirstOrDefault(r => r.Id == GetUserRoleID(id)).Name;
        }

        //public AppUser GetAppUser(int id)
        //{
        //    return _context.Users.FirstOrDefault(u => u.Id == id);
        //}


    }
}
