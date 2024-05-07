using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShoppingApp.Models
{
    public class Images
    {
        public int Id { get; set; }
        public string Source { get; set; } 

        public int IsMain { get; set; }

        // Foreign key property
        [ForeignKey(nameof(Product))] 
        public int ProductId { get; set; } 

        public virtual Product Product { get; set; }
    }
}
