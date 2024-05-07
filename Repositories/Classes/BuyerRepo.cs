using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class BuyerRepo : IBuyerRepo
    {

        private ShoppingContext _context;
        private IWebHostEnvironment _hostingEnvironment;

        public BuyerRepo(ShoppingContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public UpdateProfileViewModel GetProfileData(int buyerId)
        {
            Buyer buyer = _context.Buyers.Where(b => b.Id == buyerId).FirstOrDefault();
            UpdateProfileViewModel buyerProfile = null;
            if (buyer != null)
            {
                buyerProfile = new()
                {
                    FirstName = buyer.FirstName,
                    LastName = buyer.LastName,
                    DOB = buyer.DOB,
                    Image = buyer.Image,
                    PhoneNumber = buyer.PhoneNumber
                };

            }
            return buyerProfile;
        }

        public  bool UpdateProfile(UpdateProfileViewModel oldData, int buyerId)
        {
            Buyer buyer = _context.Buyers.Where(b => b.Id == buyerId).FirstOrDefault();

            if (buyer != null)
            {
                buyer.FirstName = oldData.FirstName;
                buyer.LastName = oldData.LastName;
                buyer.PhoneNumber = oldData.PhoneNumberDb;
                buyer.DOB = oldData.DOB;
                //buyer.Image = oldData.Image;

                if (oldData.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "UserImage");
                    string fileNameWithExtention = $"{buyerId}{Path.GetExtension(oldData.ImageFile.FileName)}";

                    string filePath = Path.Combine(uploadsFolder, fileNameWithExtention);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                         oldData.ImageFile.CopyTo(fileStream);
                    }

                    buyer.Image = fileNameWithExtention; 
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
    }
}
