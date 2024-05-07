using Microsoft.CodeAnalysis;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class RateRepo : IRateRepo
    {
        ShoppingContext _context { get; }
        public RateRepo(ShoppingContext context)
        {
            _context = context;
        }

        public bool ProductExist(int productId)
        {
            return _context.Products.Any(p => p.Id == productId);
        }
        public void Rate(int ProductId, int UserId, int NumOfStars)
        {
            var rate = _context.Rates.Find(ProductId, UserId);
            if (rate != null)
            {
                if (rate.NumOfStars == NumOfStars)
                {
                    _context.Rates.Remove(rate);
                    _context.SaveChanges();
                    return;
                }
                _context.Rates.Remove(rate);
            }

            if (NumOfStars >= 1 && NumOfStars <= 5)
            {

                rate = new Rate
                {
                    BuyerId = UserId,
                    ProductId = ProductId,
                    NumOfStars = NumOfStars
                };

                _context.Rates.Add(rate);
                _context.SaveChanges();

            }
        }

        public int GetRateForUser(int productId, int userId)
        {
            return _context.Rates.Where(r => r.ProductId == productId && r.BuyerId == userId).Select(r => r.NumOfStars).FirstOrDefault();
        }

        public int GetAvgRateForProduct(int productId)
        {
            var ratings = _context.Rates
                        .Where(r => r.ProductId == productId)
                        .Select(r => r.NumOfStars)
                        .ToList();

            if (ratings.Any())
            {
                double averageRating = ratings.Average();
                return (int)Math.Round(averageRating);
            }
            else
            {
                return 0;
            }
        }

        public List<Rate> GetAllRatesForProduct(int productId)
        {
            return _context.Rates.Where(r => r.ProductId == productId).ToList();
        }
    }
}
