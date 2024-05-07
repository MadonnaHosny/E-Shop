using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.Models
{
    public class Seller : AppUser
    {
        public string? Description { get; set; }
        public string? BusinessName { get; set; }
        [MaxLength(9)]
        public string? VAT { get; set; }
        public string? Paper { get; set; }

        public bool IsVerified { get; set; } = false;

        // navigation properties
        public List<ProductSeller> ProductSellers { get; set; }

    }
}
