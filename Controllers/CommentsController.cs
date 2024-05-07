using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Extentions;
using OnlineShoppingApp.Helpers;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Classes;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentsRepo commRepo;
        private readonly IProductRepo productRepo;

        public CommentsController(ICommentsRepo _commRepo, IProductRepo productRepo)
        {
            commRepo = _commRepo;
            this.productRepo = productRepo;
        }

      
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertComment(string review, int prodID)
        {

            var comment = new Comment
            {
                Text = review,
                Date = DateTime.Now,
                AppUserId = User.GetUserId(),
                Product = productRepo.GetById(prodID)
            };

            commRepo.AddComment(comment);
            var product = productRepo.GetById(prodID);
            if (product != null)
            {
                product.Comments = commRepo.GetAllComments(prodID);
            }
            return View("~/Views/Product/GetProduct.cshtml", product);

        }
    }
}
