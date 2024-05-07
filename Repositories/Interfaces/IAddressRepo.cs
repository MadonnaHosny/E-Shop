using OnlineShoppingApp.Models;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Repositories.Interfaces
{
	public interface IAddressRepo
	{
		public List<Address> GetAddresses();
		public Address GetAddressById(int id);
		public void Insert(Address address);
		public void Update(int  id, Address address);
		public void Delete(int id);
		public AppUser GetUser(int id);
		public List<Address> GetAddressByBuyerId(int id);
		public bool CheckIfExist(AddAddressViewModel address);

    }
}
