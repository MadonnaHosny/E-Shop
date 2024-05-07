using Microsoft.AspNetCore.Identity;

namespace OnlineShoppingApp.Models
{
    public class AppRole : IdentityRole<int>
    {
        public List<AppUserRole> UserRoles { get; set; }
    }
}
