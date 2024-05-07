using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShoppingApp.Models
{
    public class Product
    {
        
        public Product() { ImageUrl = new List<IFormFile>(); }    


        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }

        // Foreign key property
        [ForeignKey(nameof(Category))]
        public int categoryId { get; set; }

        public virtual Category Category { get; set; }

        // Foreign key property
        [ForeignKey(nameof(Brand))]
        public int brandId { get; set; }
        public virtual Brand Brand { get; set; }

        public int Quantity { get; set; }
        public virtual ICollection<Images>? Images { get; set; } = new List<Images>();

        [NotMapped]
        // public List<string> ImageUrl { get; set; }
        public List<IFormFile> ImageUrl { get; set; }
        public List<Rate> Rates { get; set; }
		public List<Comment> Comments { get; set; }
        public List<ProductSeller> ProductSellers { get; set; }




    }
}
