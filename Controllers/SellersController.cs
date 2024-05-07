using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Extentions;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
    public class SellersController : Controller
    {
        private ISellerRepo _sellerRepo;
        private IRateRepo _rateRepo;
        private readonly ICategoriesRepo _categoriesRepo;

        public SellersController(ISellerRepo sellerRepo, IRateRepo rateRepo,ICategoriesRepo categoriesRepo)
        {
            _sellerRepo = sellerRepo;
            _rateRepo = rateRepo;
            _categoriesRepo = categoriesRepo;
        }


        [HttpGet]
        public IActionResult GetProfileData(int Id)
        {
            ViewBag.ProductCategories = _categoriesRepo.GetAll();
            return View(_sellerRepo.GetProfileDataAsViewer(Id));
        }



        [HttpGet]
        public IActionResult Profile()
        {
            return View(_sellerRepo.GetProfileData(User.GetUserId()));
        }

        [HttpPost]
        public IActionResult Profile(UpdateSellerProfileViewModel updateProfile)
        {
            if (ModelState.IsValid)
            {
                bool res = _sellerRepo.UpdateProfile(updateProfile, User.GetUserId());
                if (res)
                {
                    ViewBag.MessageSuccess = "Profile updated successfully.";
                    return RedirectToAction("Profile", "Sellers");
                }

            }

            ViewBag.MessageFail = "Failed to update profile.";

            return View(updateProfile);
        }
    }
}
