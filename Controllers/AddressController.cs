using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Helpers;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
	public class AddressController : Controller
	{
		private readonly IAddressRepo _addressRepo;
        private DataForOrder _dataForOrder =new DataForOrder();
		public AddressController(IAddressRepo addressRepo)
        {
			_addressRepo = addressRepo;
			
		}
        // GET: AddressController
        public ActionResult Index(DataForOrder buyerCartData)
		{
			var buyerAddress = _addressRepo.GetAddressByBuyerId(UserHelper.LoggedinUserId);
			var addressList = buyerAddress.Select(Ad => new SelectListItem
			{
				Value = Ad.Id.ToString(),
				Text = $"{Ad.BuildingNumber} - {Ad.Street} - {Ad.City} - {Ad.Country}"
			}).ToList();
            buyerCartData.AddressList = new SelectList( addressList, "Value", "Text");
            buyerCartData.User = _addressRepo.GetUser(UserHelper.LoggedinUserId);

            _dataForOrder = buyerCartData;
            //{ 
            //    BuyerId=buyerCartData.BuyerId,
            //    User=buyerCartData.User,
            //    Items=buyerCartData.Items,
            //    DeliveryMethodId=buyerCartData.DeliveryMethodId,
            //    AddressList=buyerCartData.AddressList,
            //    AddressId=buyerCartData.AddressId,

            //};
		   // ViewBag.buyerCart = buyerCart;
			return View(buyerCartData);
		}


		//public ActionResult AddNewAddress([Bind("BuildingNumber, Street, City, Country, IsMain, BuyerId")] Address address)
		//{
		//	//Address address = new Address(){ BuildingNumber = BuildingNumber, Street = Street, Country = Country, City = City };
		//	if(address != null)
		//	{
		//		_addressRepo.Insert(address);
		//	}
		//	return RedirectToAction(nameof(Index));
		//}
        //public ActionResult AddNewAddress(Address address)
        //{
        //    //Address address = new Address(){ BuildingNumber = BuildingNumber, Street = Street, Country = Country, City = City };
        //    if (address != null)
        //    {
        //        _addressRepo.Insert(address);
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

        public ActionResult AddNewAddress(AddAddressViewModel viewModel, decimal ShippingPrice, decimal SubTotal, int DeliveryMethodId)
        {
            bool flag = false;
            if (viewModel != null)
            {
                if (_addressRepo.CheckIfExist(viewModel))
                {
                    // Create a new Address object and map properties from the view model
                    Address address = new Address
                    {
                        BuildingNumber = viewModel.BuildingNumber,
                        Street = viewModel.Street,
                        City = viewModel.City,
                        Country = viewModel.Country,
                        IsMain = viewModel.IsMain,
                        BuyerId = viewModel.BuyerId
                    };
                    _addressRepo.Insert(address);
                    flag = true;
                    ViewBag.Flag = flag;

                }
                else
                {
                    flag = false;
                    ViewBag.Flag = flag;
                    Console.WriteLine("Address Exists");
                    return RedirectToAction("Index");
                }
                
            }
            Address lastAdrs = _addressRepo.GetAddresses().Last();
            _dataForOrder.OrderAddress = lastAdrs;
            _dataForOrder.User = _addressRepo.GetUser(UserHelper.LoggedinUserId);
            _dataForOrder.ShippingPrice = ShippingPrice;
            _dataForOrder.SubTotal = SubTotal;
            _dataForOrder.DeliveryMethodId = DeliveryMethodId;
            _dataForOrder.total = _dataForOrder.SubTotal + _dataForOrder.ShippingPrice;
            //{
            //	BuyerId = lastAdrs.BuyerId,
            //	BuildingNumber = lastAdrs.BuildingNumber,
            //	Street = lastAdrs.Street,
            //	City = lastAdrs.City,
            //	Country = lastAdrs.Country,
            //             Id= lastAdrs.Id,
            //             IsMain= lastAdrs.IsMain,
            //};


            return View("CheckoutStaticData", _dataForOrder);
        }

        public ActionResult GetExistingAddressId(int AddressId,decimal ShippingPrice,decimal SubTotal,int DeliveryMethodId)
        {
            Address extAdrs = _addressRepo.GetAddressById(AddressId);
            _dataForOrder.OrderAddress = extAdrs;
            _dataForOrder.User = _addressRepo.GetUser(UserHelper.LoggedinUserId);
            _dataForOrder.ShippingPrice = ShippingPrice;
            _dataForOrder.SubTotal = SubTotal;
            _dataForOrder.DeliveryMethodId = DeliveryMethodId;
            _dataForOrder.total = _dataForOrder.SubTotal + _dataForOrder.ShippingPrice;
            //{
            //    Id = extAdrs.Id,
            //    BuyerId = extAdrs.BuyerId,
            //    BuildingNumber = extAdrs.BuildingNumber,
            //    Street = extAdrs.Street,
            //    City = extAdrs.City,
            //    Country = extAdrs.Country,
            //    IsMain = extAdrs.IsMain,
            //};

            //return RedirectToAction(nameof(CheckoutStaticData),_dataForOrder);
            return View("CheckoutStaticData", _dataForOrder);

        }
        public ActionResult CheckoutStaticData(DataForOrder DataForOrder)
        {
            //ViewBag.Buyer = _addressRepo.GetUser(UserHelper.LoggedinUserId);
            Console.WriteLine(DataForOrder);
			return View(DataForOrder);
        }
    }
}
