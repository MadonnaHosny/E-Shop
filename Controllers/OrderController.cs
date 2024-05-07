using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineShoppingApp.Common;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.Services.Classes;
using OnlineShoppingApp.Services.Interfaces;
using OnlineShoppingApp.ViewModels;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;
using System.Net.WebSockets;
using static System.Net.WebRequestMethods;
using Order = OnlineShoppingApp.Models.Order;

namespace OnlineShoppingApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ICartService _cartService;
        private readonly IProductRepo _productRepo;
        private readonly IDeliveryMethodsRepo _deliveryMethodsRepo;
        private readonly IAddressRepo _addressRepo;
        private readonly IOrderItemRepo _orderItemRepo;
        public OrderController(IOrderRepo orderRepo,
            ICartService cartService,
            IProductRepo productRepo,
            IDeliveryMethodsRepo deliveryMethodsRepo,
            IAddressRepo addressRepo,
            IOrderItemRepo orderItemRepo,
            IPaymentService paymentService)
        {
            _orderRepo = orderRepo;
            _cartService = cartService;
            _productRepo = productRepo;
            _deliveryMethodsRepo = deliveryMethodsRepo;
            _addressRepo = addressRepo;
            _orderItemRepo = orderItemRepo;
        }

        public IActionResult Create(string news, DataForOrder orderData)
        {
            Order order = new Order();
            if (news == "Strip")
            {
                //BuyerCartViewModel buyerCartViewModel = new BuyerCartViewModel();
                order.BuyerId = (int)orderData.BuyerId;
                order.DeliveryMethod = _deliveryMethodsRepo.GetbyId(orderData.DeliveryMethodId);
                order.ShippingAddress = _addressRepo.GetAddressById((int)orderData.AddressId);
                order.SubTotal = orderData.SubTotal;
                //order.PaymentIntentId
                _orderRepo.CreateOrder(order);
                var orderId = _orderRepo.GetLastOrder().Id;
                var CartItems = _cartService.GetCartItems();
                var domain = "https://localhost:7289/";
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"Order/Confirmation",
                    CancelUrl = domain + $"Order/Cancel",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };
                foreach (var cartItem in CartItems)
                {
                    var sessionListItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(cartItem.Price * 100) ,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = cartItem.ProductName.ToString(),
                            }
                        },
                        Quantity = cartItem.Quantity,
                    };

                    options.LineItems.Add(sessionListItem);

                    OrderItem orderItem = new OrderItem()
                    {
                        ProductId = cartItem.Id,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Price,
                        Product = _productRepo.GetById(cartItem.Id),
                        OrderId = orderId
                    };
                    _orderItemRepo.Insert(orderItem);
                    var gettedOrderItem = _orderItemRepo.GetLast();
                    order.OrderItems.Add(gettedOrderItem);

                }
                var shippingCostLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(orderData.ShippingPrice * 100), // Convert shipping cost to cents
                        Currency = "usd",ProductData = new SessionLineItemPriceDataProductDataOptions                 
                        {                     
                            Name = "Shipping Cost",                 
                        }             
                    },             
                    Quantity = 1, // Assuming shipping cost is a one-time charge
                 };         
                options.LineItems.Add(shippingCostLineItem);
                var service = new SessionService();
                Session session = service.Create(options);
                Response.Headers.Add("Location", session.Url);

                //OrderConfirmationDataViewModel orderConfirmationData = new OrderConfirmationDataViewModel
                //{
                //    OrderId = order.Id,
                //    SessionId = session.Id

                //};

                TempData["SessionId"] = session.Id;
                TempData["OrderId"] = order.Id;

                //  _paymentService.CreateOrUpdatePaymentIntent(orderId);
                //buyerCartViewModel.Items = order.OrderItems;



            }
            else if (news == "Cash")
            {
                order.BuyerId = (int)orderData.BuyerId;
                order.DeliveryMethod = _deliveryMethodsRepo.GetbyId(orderData.DeliveryMethodId);
                order.ShippingAddress = _addressRepo.GetAddressById((int)orderData.AddressId);
                order.SubTotal = orderData.SubTotal;
                _orderRepo.CreateOrder(order);
                var orderId = _orderRepo.GetLastOrder().Id;
                var CartItems = _cartService.GetCartItems();
                foreach (var cartItem in CartItems)
                {
                    OrderItem orderItem = new OrderItem()
                    {
                        ProductId = cartItem.Id,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Price,
                        Product = _productRepo.GetById(cartItem.Id),
                        OrderId = orderId
                    };
                    _orderItemRepo.Insert(orderItem);
                    var gettedOrderItem = _orderItemRepo.GetLast();
                    order.OrderItems.Add(gettedOrderItem);
                }
                var orderitems = _orderItemRepo.GetAll(orderId);
                foreach (var item in orderitems)
                {
                    var prod = _productRepo.GetById(item.ProductId);
                    prod.Quantity -= item.Quantity;
                    _productRepo.UpdateProductQuantity(item.ProductId, prod);
                }
                _cartService.DeleteCart();
                return View("PaymentPending");
            }
            else
            {
                Console.WriteLine("Alert News is Null");
            }
            return new StatusCodeResult(303);
        }


        public IActionResult Confirmation()
        {
            if (!TempData.ContainsKey("SessionId") || !TempData.ContainsKey("OrderId"))
            {
                // Handle case where TempData keys are missing
                return BadRequest("Invalid request");
            }

            var sessionId = TempData.Peek("SessionId").ToString();
            var orderId = Convert.ToInt32(TempData.Peek("OrderId"));

            var service = new SessionService();
            Session session = service.Get(sessionId);

            if (session.PaymentStatus == "paid")
            {
                var order = _orderRepo.GetOrderById(orderId);
                if (order != null)
                {
                    order.Status = OrderStatus.PaymentReceived;
                    order.PaymentIntentId = session.PaymentIntentId;
                    _orderRepo.UpdateOrder(orderId, order);
                    _cartService.DeleteCart();
                    var orderitems = _orderItemRepo.GetAll(orderId);
                    foreach(var item in orderitems)
                    {
                        var prod = _productRepo.GetById(item.ProductId);
                        prod.Quantity -= item.Quantity;
                        _productRepo.UpdateProductQuantity(item.ProductId, prod);
                    }
                    return View("PaymentSucceeded");

                }
                else
                {
                    // Handle case where order is not found
                    return NotFound();
                }
            }
            else
            {
                var order = _orderRepo.GetOrderById(orderId);
                order.Status = OrderStatus.PaymentFailed;
                _orderRepo.UpdateOrder(orderId, order);

                return View("PaymentFailed");
            }
        }
    }
}
