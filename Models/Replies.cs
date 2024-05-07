using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShoppingApp.Models
{
    public class Replies
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        [ForeignKey("AppUser")]

        public int AppUserId { get; set; }

        public AppUser? AppUser { get; set; }

        [ForeignKey("comment")]
        public int CommentId { get; set; }

        public Comment? comment { get; set; }
    }
}
