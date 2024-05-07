using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShoppingApp.Common;
using OnlineShoppingApp.Extentions;
using OnlineShoppingApp.Helpers;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Classes;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;
using System.Diagnostics;

namespace OnlineShoppingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IProductRepo ProductRepo;
        ICategoriesRepo categoriesRepo;
        IBrandRepo brandRepo;

        IRateRepo _rateRepo { get; }
        static int ProductIdForJs = 0;
        ICommentsRepo commentsRepo;
        IUserRepo userRepo;

        public HomeController(ILogger<HomeController> logger, IProductRepo _productRepo, ICategoriesRepo _categoriesRepo, IBrandRepo _brandRepo, ICommentsRepo commentsRepo, IRateRepo rateRepo, UserManager<AppUser> userManager, IUserRepo _userRepo)
        {
            _logger = logger;
            ProductRepo = _productRepo;
            categoriesRepo = _categoriesRepo;
            brandRepo = _brandRepo;
            this.commentsRepo = commentsRepo;
            _rateRepo = rateRepo;
            userRepo = _userRepo;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string role = userRepo.GetUserRole(User.GetUserId());

                if (role == UserType.Seller.ToString())
                {
                    var viewModel = new ProductViewModel
                    {
                        Products = ProductRepo.GetProductsPerSeller(User.GetUserId())
                    };
                    ViewBag.ProductCategories = categoriesRepo.GetAll();
                    return View("~/Views/Product/GetAllSellerProducts.cshtml", viewModel);
                }
                else if (role == UserType.Buyer.ToString())
                {
                    var viewModel = new ProductViewModel
                    {
                        Categories = categoriesRepo.GetAll(),
                        Brands = brandRepo.GetAll(),
                        Products = ProductRepo.GetAll(),
                        BestSellerProduct = ProductRepo.GetBestSellingProducts(),

                    };

                    ViewBag.ProductCategories = categoriesRepo.GetAll();
                    return View(viewModel);
                }
                else
                {
                    return RedirectToAction("GetNotVerifiedSellers", "Admins");

                }
            }
            else
            {
                // User is not authenticated, render the view without any account details
                var viewModel = new ProductViewModel
                {
                    Categories = categoriesRepo.GetAll(),
                    Brands = brandRepo.GetAll(),
                    Products = ProductRepo.GetAll(),
                    BestSellerProduct = ProductRepo.GetBestSellingProducts(),

                };

                ViewBag.ProductCategories = categoriesRepo.GetAll();
                return View(viewModel);
            }


            return NoContent();

        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult NotFound()
        {
            return View("NotFound");
        }

    }
}
