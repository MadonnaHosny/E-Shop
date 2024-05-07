using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.Models
{
    public class ProductSeller
    {
        public int SellerId { get; set; }
        public  Seller Seller { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}
