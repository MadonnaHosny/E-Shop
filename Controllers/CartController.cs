using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Extentions;
using OnlineShoppingApp.Helpers;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Classes;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.Services;
using OnlineShoppingApp.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace OnlineShoppingApp.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly IProductRepo _ProductRepo;
        private readonly IDeliveryMethodsRepo _deliveryMethod;

        public CartController(CartService cartService, IProductRepo productRepo, IDeliveryMethodsRepo deliveryMethod)
        {
            _cartService = cartService;
            _ProductRepo = productRepo;
            _deliveryMethod = deliveryMethod;
        }

        public IActionResult Index()
        {
            ViewBag.DeliveryMethods = _deliveryMethod.GetAll();
            ViewBag.Products = _ProductRepo.GetAll();
            return View(_cartService.GetCartItems());
        }
        public IActionResult AddToCart(int id)
        {
            var prod = _ProductRepo.GetById(id);
            if (prod != null)
            {
                CartItemViewModel item = new CartItemViewModel()
                {
                    Id = id,
                    ProductName = prod.Name,
                    PictureUrl = prod.Images.Where(I => I.IsMain == 1).Select(I => I.Source).FirstOrDefault(),
                    Price = prod.Price,
                    Brand = prod.Brand.Name,
                    Category = prod.Category.Name,
                    Description = prod.Description
                };
                _cartService.AddToCart(item);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult UpdateCart(Dictionary<int, int> quant)
        {
            var itemId = 0;
            var quantity = 0;
            foreach (var entry in quant)
            {
                itemId = entry.Key; // This will be "@item.Id"
                quantity = entry.Value; // This will be the corresponding quantity

                // Process the update for the item with itemId and quantity
            }

            _cartService.UpdateCart(itemId, quantity);
            //prod.Quantity -= quantity;
            //_ProductRepo.UpdateProductQuantity(itemId, prod);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var cartItem = _cartService.GetCartItem(id);
            if (cartItem != null)
            {
                _cartService.RemoveFromCart(id);
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteFromIndex(int id)
        {
            var cartItem = _cartService.GetCartItem(id);
            if (cartItem != null)
            {
                _cartService.RemoveFromCart(id);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ProceedToAddress(int DeliveryMethodId)
        {
            DataForOrder buyerCartData = new DataForOrder()
            {
                BuyerId = User.GetUserId(),
                Items = _cartService.GetCartItems(),
                DeliveryMethodId = DeliveryMethodId,
                SubTotal = _cartService.GetTotal(),
                ShippingPrice = _deliveryMethod.GetbyId(DeliveryMethodId).DeliveryCost,
            };
            return RedirectToAction("Index", "Address", buyerCartData);
        }
        [HttpGet]
        public IActionResult GetNumOfItemsInCart()
        {
            return Json(_cartService?.GetCartItems()?.Count);
        }
    }

}
