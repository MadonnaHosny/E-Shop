using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Extentions;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
    public class BuyersController : Controller
    {
        private  IBuyerRepo _buyerRepo;

        public BuyersController(IBuyerRepo buyerRepo)
        {
           _buyerRepo = buyerRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View(_buyerRepo.GetProfileData(User.GetUserId()));
        }

        [HttpPost]
        public IActionResult Profile(UpdateProfileViewModel updateProfile)
        {
            if (ModelState.IsValid)
            {
               bool res =  _buyerRepo.UpdateProfile(updateProfile, User.GetUserId());
                if (res)
                {
                    ViewBag.MessageSuccess = "Profile updated successfully.";
                    return RedirectToAction("Profile", "Buyers");
                }

            }

            ViewBag.MessageFail = "Failed to update profile.";

            return View(updateProfile);
        }

        //[HttpPost]

        //public async Task<IActionResult> Edit(UpdateProfileViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.GetUserAsync(User);
        //        if (user == null)
        //        {
        //            return NotFound();
        //        }

        //        user.FirstName = model.FirstName;
        //        user.LastName = model.LastName;
        //        user.PhoneNumber = model.PhoneNumber;
        //        // Update other properties as needed

        //        //if (model.ImageFile != null)
        //        //{
        //        //    // Handle image upload
        //        //    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
        //        //    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
        //        //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        //    {
        //        //        await model.ImageFile.CopyToAsync(fileStream);
        //        //    }

        //        //    user.Image = uniqueFileName; // Update user's Image property
        //        //}

        //        user.DOB = model.DOB; // Assuming DOB is nullable DateTime in your user model

        //        var result = await _userManager.UpdateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Index", "Home"); // or wherever you want to redirect after successful update
        //        }
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //    }
        //    return View(model);
        //}
    }
}
