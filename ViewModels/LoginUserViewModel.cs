using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.ViewModels
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Email field is required")]

		[RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email is not valid.")]
		public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;  
    }
}
