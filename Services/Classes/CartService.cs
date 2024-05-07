using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OnlineShoppingApp.Helpers;
using OnlineShoppingApp.Services.Interfaces;
using OnlineShoppingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OnlineShoppingApp.Services
{
    public class CartService : ICartService
    {
        private string CartKey = $"UserCart{UserHelper.LoggedinUserId}";
        // int id= int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddToCart(CartItemViewModel newItem)
        {
            // Retrieve existing cart items from cookies
            var existingCart = GetCartItems();

            // Check if the item already exists in the cart
            var existingItem = existingCart.FirstOrDefault(item => item.Id == newItem.Id);

            //// Update properties of the existing item
            //existingItem.Quantity = newItem.Quantity; // For example, update the quantity
            //existingItem.Price = newItem.Price; // Update other properties as needed

            if (existingItem == null)
            {
                // Add the new item to the cart
                existingCart.Add(newItem);

                // Save the updated cart items to cookies
                SaveCartItems(existingCart);
            }

        }



        public void RemoveFromCart(int itemId)
        {
            // Retrieve existing cart items from cookies
            var existingCart = GetCartItems();

            // Find the item with the specified ID and remove it from the cart
            var itemToRemove = existingCart.FirstOrDefault(item => item.Id == itemId);
            if (itemToRemove != null)
            {
                existingCart.Remove(itemToRemove);
            }

            // Save the updated cart items to cookies
            SaveCartItems(existingCart);
        }

        public List<CartItemViewModel> GetCartItems()
        {

            if (UserHelper.LoggedinUserId != 0)
            {
                var context = _httpContextAccessor.HttpContext;
                var cartJson = context.Request.Cookies[CartKey];
                return cartJson != null ? JsonConvert.DeserializeObject<List<CartItemViewModel>>(cartJson) : new List<CartItemViewModel>();
            }
            return new List<CartItemViewModel> { };
        }

        public void SaveCartItems(List<CartItemViewModel> cartItems)
        {
            var context = _httpContextAccessor.HttpContext;
            var cartJson = JsonConvert.SerializeObject(cartItems);
            context.Response.Cookies.Append(CartKey, cartJson, new CookieOptions
            {
                Expires = DateTime.Now.AddMonths(1), // Cookie expiration time
                HttpOnly = true // Make the cookie accessible only through HTTP requests
            });
        }

        public void UpdateCart(int id, int Quantity)
        {
            // Retrieve existing cart items from cookies
            var existingCart = GetCartItems();

            // Check if the item already exists in the cart
            var existingItem = existingCart.FirstOrDefault(item => item.Id == id);

            // Update properties of the existing item
            existingItem.Quantity = Quantity; // For example, update the quantity

            // Save the updated cart items to cookies
            SaveCartItems(existingCart);

        }

        public CartItemViewModel GetCartItem(int id)
        {
            var existingCart = GetCartItems();

            // Check if the item already exists in the cart
            var existingItem = existingCart.FirstOrDefault(item => item.Id == id);

            return existingItem;
        }

        public int GetTotal()
        {
            var total = 0;
            var existingCart = GetCartItems();

            foreach (var item in existingCart)
            {
                total += (int)(item.Quantity * item.Price);
            }
            return total;
        }

        public int GetTotalPerProduct(int productId)
        {
            var existingCart = GetCartItems();

            // Check if the item already exists in the cart
            var existingItem = existingCart.FirstOrDefault(item => item.Id == productId);
            var productTotal = (int)(existingItem.Quantity * existingItem.Price);
            return productTotal;
        }

        public void DeleteCart()
        {
            var context = _httpContextAccessor.HttpContext;
            context.Response.Cookies.Delete(CartKey);
        }
    }
}
