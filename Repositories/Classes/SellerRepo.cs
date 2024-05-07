using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class SellerRepo : ISellerRepo
    {

        private ShoppingContext _context;
        private IWebHostEnvironment _hostingEnvironment;

        public SellerRepo(ShoppingContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public GetSellerProfileViewModel GetProfileDataAsViewer(int sellerId)
        {
            Seller seller = _context.Sellers.Where(b => b.Id == sellerId).FirstOrDefault();
            GetSellerProfileViewModel sellerProfile = null;
            if (seller != null)
            {
                sellerProfile = new()
                {
                    PhoneNumber = seller.PhoneNumber,
                    Description = seller.Description,
                    BusinessName = seller.BusinessName,
                    VAT = seller.VAT,
                    Image = seller.Image,
                    AvgRating = GetAvgRateForSeller(sellerId),
                    IsVerified = seller.IsVerified,
                    Paper = seller.Paper,
                };

            }
            return sellerProfile;
        }
        public UpdateSellerProfileViewModel GetProfileData(int sellerId)
        {
            Seller seller = _context.Sellers.Where(b => b.Id == sellerId).FirstOrDefault();
            UpdateSellerProfileViewModel sellerProfile = null;
            if (seller != null)
            {
                sellerProfile = new()
                {
                    PhoneNumber = seller.PhoneNumber,
                    Paper = seller.Paper,
                    Description = seller.Description,
                    BusinessName = seller.BusinessName,
                    VAT = seller.VAT,
                    Image = seller.Image,
                };

            }
            return sellerProfile;
        }

        public bool UpdateProfile(UpdateSellerProfileViewModel oldData, int sellerId)
        {
            Seller seller = _context.Sellers.Where(b => b.Id == sellerId).FirstOrDefault();

            if (seller != null)
            {

                seller.PhoneNumber = oldData.PhoneNumberDb;
                seller.Paper = oldData.Paper;
                seller.Description = oldData.Description;
                seller.BusinessName = oldData.BusinessName;
                seller.VAT = oldData.VAT;
                seller.Image = seller.Image;

                if (oldData.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "UserImage");
                    string fileNameWithExtention = $"{sellerId}{Path.GetExtension(oldData.ImageFile.FileName)}";

                    string filePath = Path.Combine(uploadsFolder, fileNameWithExtention);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        oldData.ImageFile.CopyTo(fileStream);
                    }

                    seller.Image = fileNameWithExtention;
                }

                if (oldData.PaperFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "PaperImage");
                    string fileNameWithExtention = $"{sellerId}{Path.GetExtension(oldData.PaperFile.FileName)}";

                    string filePath = Path.Combine(uploadsFolder, fileNameWithExtention);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        oldData.PaperFile.CopyTo(fileStream);
                    }

                    seller.Paper = fileNameWithExtention;
                }

            }
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public List<Seller> GetNotVerifiedSeller()
        {
            return _context.Sellers.OrderByDescending(s => s.Id).Where(s => !s.IsVerified).ToList();
        }
        private int GetAvgRateForSeller(int sellerId)
        {
            var rates = _context.ProductSellers
                        .Where(ps => ps.SellerId == sellerId)
                        .Join(
                            _context.Rates,
                            ps => ps.ProductId,
                            r => r.ProductId,
                            (ps, r) => r.NumOfStars
                        );

            if (rates.Any())
            {
                double averageRating = rates.Average();
                return (int)Math.Round(averageRating);
            }
            else
            {
                return 0;
            }

        }

        public bool VerifySeller(int sellerId)
        {
            Seller seller = _context.Sellers.Where(s => s.Id == sellerId).FirstOrDefault();
            try
            {
                seller.IsVerified = true;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;

            }
        }
        public bool CheckIfSeller(int id)
        {
            var Seller = _context.Sellers.FirstOrDefault(s => s.Id == id);
            if (Seller == null)
            {
                return false;
            }

            return true;

        }

    }
}
