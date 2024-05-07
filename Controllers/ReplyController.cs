using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Extentions;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Classes;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Controllers
{
    public class ReplyController : Controller
    {
        IReplyRepo replyRepo;
        IProductRepo productRepo;
        ICommentsRepo commentRepo;
        public ReplyController(IReplyRepo _replyRepo, IProductRepo _productRepo, ICommentsRepo _commentRepo)
        {
            replyRepo = _replyRepo; 
            productRepo = _productRepo;
            commentRepo = _commentRepo; 

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult InsertReply(string enteredText, int commentId) 
        {
            Replies reply;
           if (enteredText != null)
           {
                reply = new Replies

                {
                  Text = enteredText, Date = DateTime.Now , AppUserId = User.GetUserId() , CommentId=commentId    
                };
                replyRepo.AddReply(reply);
           }
            
            var comment = commentRepo.GetComment(commentId);
            var product = productRepo.GetById(comment.ProductId);
            ViewBag.Seller = productRepo.GetProductSellerId(product.Id);

            return RedirectToAction("GetProduct", "Product", new { id = product.Id });
        }
    }
}
