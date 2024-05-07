using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.ViewModels
{
    public class UpdateSellerProfileViewModel
    {
        public string? PhoneNumberDb { get; set; }//
        [RegularExpression(@"^\d{11}$", ErrorMessage = "The phone number must be 11 digits.")]

        public string PhoneNumber { get; set; }//

        public string? Paper { get; set; }
        public IFormFile? PaperFile { get; set; }


        public string? Image { get; set; }//
        public IFormFile? ImageFile { get; set; }//

        public string? Description { get; set; }//
        public string? BusinessName { get; set; }//

        [RegularExpression(@"^\d{9}$", ErrorMessage = "The VAT must be 9 digits.")]

        public string? VAT { get; set; }



    }
}
