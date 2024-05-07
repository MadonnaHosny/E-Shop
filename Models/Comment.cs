using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.Models
{
	public class Comment
	{
		public int Id { get; set; }
		[Required]
		public string Text { get; set; }
		public DateTime Date { get; set; }

		public int AppUserId { get; set; }
		public AppUser AppUser { get; set; }

		public int ProductId { get; set; }

		public Product Product { get; set; }

		public ICollection<Replies>? Replies { get; set;}
	}
}
