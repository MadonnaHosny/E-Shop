using OnlineShoppingApp.Common;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.Services.Interfaces;
using OnlineShoppingApp.ViewModels;
using Stripe;

namespace OnlineShoppingApp.Services.Classes
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderRepo _orderRepo;
        private readonly IDeliveryMethodsRepo _deliveryMethodsRepo;

        public PaymentService(IConfiguration configuration, IOrderRepo orderRepo, IDeliveryMethodsRepo deliveryMethodsRepo)
        {
            _configuration = configuration;
            _orderRepo = orderRepo;
            _deliveryMethodsRepo = deliveryMethodsRepo;
        }

        public Order CreateOrUpdatePaymentIntent(int orderId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var order = _orderRepo.GetOrderById(orderId);
            if (order != null)
            {
                var shippingPrice = _deliveryMethodsRepo.GetbyId(order.DeliveryMethod.Id);
                var total = order.Total;
                PaymentIntentService paymentIntentService = new PaymentIntentService();
                PaymentIntent paymentIntent;
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)order.OrderItems.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice.DeliveryCost * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = paymentIntentService.Create(options);
                order.PaymentIntentId = paymentIntent.Id;
                order.ClientSecret = paymentIntent.ClientSecret;
                _orderRepo.UpdateOrder(orderId, order);
            }
            return order;
        }

        public Order UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId,bool isSucceeded)
        {
            
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var order = _orderRepo.GetWithPaymentIntent(paymentIntentId);

                //var paymentstatus = paymentIntent.Status;
                if (isSucceeded)
                    order.Status = OrderStatus.PaymentReceived;
                else
                    order.Status = OrderStatus.PaymentFailed;

                _orderRepo.UpdateOrder(order.Id, order);
            return order;
        }

        //public string GetPaymentIntentStatus(string paymentIntentId)
        //{
        //    StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

        //    var paymentIntentService = new PaymentIntentService();
        //    var paymentIntent = paymentIntentService.Get(paymentIntentId);

        //    if (paymentIntent != null)
        //    {
        //        return paymentIntent.Status;
        //    }

        //    return null; // Or handle this case according to your application logic
        //}
    }
}


//public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
//{
//    StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
//    var basket = await _basketRepo.GetBasketAsync(basketId);
//    if (basket is null) return null;
//    var shippingPrice = 0M;
//    if (basket.DeliveryMethodId.HasValue)
//    {
//        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
//        basket.ShippingPrice = deliveryMethod.Cost;
//        shippingPrice = deliveryMethod.Cost;
//    }
//    if (basket?.Items.Count > 0)
//    {
//        foreach (var item in basket.Items)
//        {
//            var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
//            if (item.Price != product.Price)
//                item.Price = product.Price;
//        }
//    }
//    PaymentIntentService paymentIntentService = new PaymentIntentService();
//    PaymentIntent paymentIntent;
//    if (string.IsNullOrEmpty(basket.PaymentIntentId))
//    {
//        var options = new PaymentIntentCreateOptions()
//        {
//            Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice * 100,
//            Currency = "usd",
//            PaymentMethodTypes = new List<string>() { "card" }
//        };
//        paymentIntent = await paymentIntentService.CreateAsync(options);
//        basket.PaymentIntentId = paymentIntent.Id;
//        basket.ClientSecret = paymentIntent.ClientSecret;
//    }
//    else
//    {
//        var options = new PaymentIntentUpdateOptions()
//        {
//            Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice * 100,

//        };
//        await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);

//    }
//    await _basketRepo.UpdateBasketAsync(basket);
//    return basket;
//}

//public async Task<Order> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded)
//{
//    var spec = new OrderWithPaymentIntentSpecifications(paymentIntentId);
//    var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);

//    if (isSucceeded)
//        order.Status = OrderStatus.PaymentReceived;
//    else
//        order.Status = OrderStatus.PaymentFailed;

//    _unitOfWork.Repository<Order>().Update(order);

//    await _unitOfWork.CompleteAsync();

//    return order;
//}