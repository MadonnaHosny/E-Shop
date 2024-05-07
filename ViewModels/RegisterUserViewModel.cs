using OnlineShoppingApp.Common;
using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]

        public string LastName { get; set; }

        [Required]
		[RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email is not valid.")]

		public string Email { get; set; }

        [Required]

        public string Username { get; set; }


        [Required]
		[RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{6,}$", ErrorMessage = "The password must be at least 6 characters long and contain at least one digit, one lowercase letter, one uppercase letter, and one special character....")]

		public string Password { get; set; }


        public UserType UserType { get; set; } = UserType.Buyer;
    //// For buyers
    //public DateOnly? DOB { get; set; }

    //// For sellers
    //public string? Description { get; set; }

    //public string? BusinessName { get; set; }
}
}
