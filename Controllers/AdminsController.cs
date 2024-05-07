using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminsController : Controller
    {
        private ISellerRepo _sellerRepo;

        public AdminsController(ISellerRepo sellerRepo)
        {
            _sellerRepo = sellerRepo;
        }
        public IActionResult GetNotVerifiedSellers()
        {
            return View(_sellerRepo.GetNotVerifiedSeller());
        }

        public IActionResult GetSellerToVerify(int Id)
        {
            return View(_sellerRepo.GetProfileDataAsViewer(Id));
        }

        [HttpPost]
        public IActionResult VerifySeller(int Id)
        {
            bool verify = _sellerRepo.VerifySeller(Id);
            return Json(verify);
        }
    }
}
