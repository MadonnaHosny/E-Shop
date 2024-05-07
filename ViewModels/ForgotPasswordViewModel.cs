using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string Email { get; set; }   
    }
}
