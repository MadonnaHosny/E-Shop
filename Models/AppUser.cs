using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.Models
{
    public class AppUser : IdentityUser<int>
    {

        [MaxLength(30)]
        [Required]
        public string FirstName { get; set; }
       
        [MaxLength(30)]
        [Required]

        public string LastName { get; set; }

        public string? Image { get; set; }



        // navigation props
        public List<AppUserRole> UserRoles { get; set; }

        public List<Comment> Comments { get; set; }
	}
}
