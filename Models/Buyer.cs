using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.Models
{
    public class Buyer : AppUser
    {
        [DataType(DataType.Date)]
        public DateOnly? DOB { get; set; }
		public List<Rate> Rates { get; set; }
		public ICollection<Address> Addresses { get; set; }
        public ICollection<Order> Orders { get; set; }


    }
}
