using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Extentions;
using OnlineShoppingApp.Repositories.Classes;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
	public class RatesController : Controller
	{
        IProductRepo _productRepo;

        IRateRepo _rateRepo { get; }
		public RatesController(IRateRepo rateRepo, IProductRepo productRepo)
		{
			_rateRepo = rateRepo;
			_productRepo = productRepo;

		}
		public IActionResult Index()   
		{
			return View();
		}

		[HttpPost]
		public IActionResult RateProduct(int Id, int NumOfStars)
		{

			if (!_rateRepo.ProductExist(Id)) return View("NotFound");
			_rateRepo.Rate(Id, User.GetUserId(), NumOfStars);
		//	return View("GetPtoduct");
            return View("GetProduct",_productRepo.GetById(Id));
        }
	}
}
