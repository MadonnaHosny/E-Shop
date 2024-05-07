using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShoppingApp.Models;


namespace OnlineShoppingApp.ViewModels
{
    public class DataForOrder
    {
        public int? BuyerId { get; set; }
        public AppUser User { get; set; }
        public List<CartItemViewModel>? Items { get; set; }
        public int DeliveryMethodId { get; set; }
        public SelectList? AddressList { get; set; } 
        public int? AddressId { get; set; }
        public Address OrderAddress { get; set; }
        public decimal SubTotal { get; set; }
		public decimal ShippingPrice { get; set; }
        public decimal? total {  get; set; }
		

	}
}
