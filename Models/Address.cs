using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShoppingApp.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int BuildingNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int IsMain { get; set; }
        [ForeignKey(nameof(Buyer))]
        public int BuyerId { get; set; }
        public Buyer? Buyer { get; set; }
    }
}
