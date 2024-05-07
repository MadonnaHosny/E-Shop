using Microsoft.AspNetCore.Identity;

namespace OnlineShoppingApp.Models
{
    public class Admin : AppUser
    {
        public string WorkNumber { get; set; }
    }
}
