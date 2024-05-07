using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.ViewModels
{
    public class UpdateProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumberDb { get; set; }
        [RegularExpression(@"^\d{11}$", ErrorMessage = "The phone number must be 11 digits.")]

        public string PhoneNumber { get; set; }

        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }

        public DateOnly? DOB { get; set; }

    }
}
