using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface IUserRepo
    {
        public bool EmailExist(string email);
        public bool UsernameExist(string username);
        //public AppUser GetAppUser(int id);
        public int? GetUserRoleID(int id);
        public string GetUserRole(int id);
    }
}
