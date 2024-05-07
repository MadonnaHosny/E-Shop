using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Common;
using OnlineShoppingApp.ConfigurationClasses;
using OnlineShoppingApp.Helpers;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.Services.Interfaces;
using OnlineShoppingApp.ViewModels;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace OnlineShoppingApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailService _emailService;

        private IUserRepo _userRepo { get; }
        public AccountController(UserManager<AppUser> userManager,
                                  SignInManager<AppUser> signInManager,
                                  RoleManager<AppRole> roleManager,
                                  IEmailService emailService,
                                  IUserRepo userRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _userRepo = userRepo;
        }

        public IActionResult Test()
        {
            return View("SomethingWentWrong");
        }
        [HttpGet]

        // Redirect user to google, making url ready
        public IActionResult GoogleLogin(string returnUrl = null) // reviewed
        {
            var redirectUrl = Url.Action(nameof(GoogleLoginCallback), "Account", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        [HttpGet]
        public async Task<IActionResult> GoogleLoginCallback(string returnUrl = null, string remoteError = null) // reviewed
        {
            if (remoteError != null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await HttpContext.AuthenticateAsync("Google");
            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var firstName = result.Principal.FindFirstValue(ClaimTypes.GivenName);
            var lastName = result.Principal.FindFirstValue(ClaimTypes.Surname);
            var user = await _userManager.FindByEmailAsync(email);

            // User does not exist, so create a new user
            if (user == null)
            {
                user = new Buyer { UserName = email, Email = email, FirstName = firstName, LastName = lastName };
                var createResult = await _userManager.CreateAsync(user);

                var roleResult = await _userManager.AddToRoleAsync(user, UserType.Buyer.ToString());

                if (!createResult.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }

                ////
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = Url.Action("ConfirmEmail", "Account",

                                new { userId = user.Id, token = token }, Request.Scheme);

                /// sending confirmation mail
                EmailMessage emailMessage = new EmailMessage
                {
                    To = email,
                    Body = $"<p>Thank you for registering! To activate your account, please click the link below:</p>\r\n <a href='{confirmationLink}'>Activate</a>",
                    Subject = "Confirm your email"
                };

                _emailService.SendEmail(emailMessage);
                ///


                // Add the external login information to userlogin tale
                var addLoginResult = await _userManager.AddLoginAsync(user, new UserLoginInfo("Google", result.Principal.FindFirstValue(ClaimTypes.NameIdentifier), "Google"));

                if (!addLoginResult.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
            }

            if (user.EmailConfirmed)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                UserHelper.LoggedinUserId = user.Id;

                return RedirectToAction("Index", "Home");

            }
            else
            {
                return View("RegistrationSuccessful");
            }

        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token) // when user click on the link
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View("EmailConfirmed");

            }

            return View("SomethingWentWrong");
        }

        [HttpGet]

        public IActionResult Login(string returnUrl = null) // Reviewed
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpGet]

        public IActionResult Register() // normal register
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserViewModel model, string returnUrl = null)// Reviewed
        {
            AppUser user = null;
            if (ModelState.IsValid)
            {
                //if (!IsValidEmail(model.Email))
                //{
                //	ModelState.AddModelError("Email", "Please provide a valid email address (e.g., example@example.com)");
                //	return View(model);
                //}

                if (_userRepo.EmailExist(model.Email))
                {
                    ModelState.AddModelError("Email", "Email already registered");
                    return View(model);
                }

                //if (!IsPasswordCompatible(model.Password))
                //{
                //	ModelState.AddModelError("Password", "Passwords must be at least 6 characters, at least one digit, one lowercase letter, one uppercase letter, and one non-alphanumeric character.");
                //	return View(model);
                //}
                if (_userRepo.UsernameExist(model.Username))
                {
                    ModelState.AddModelError("Username", "Username is taken");
                    return View(model);
                }

                if (model.UserType == UserType.Buyer)
                {
                    user = new Buyer
                    {
                        UserName = model.Username,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = "+20 0"
                    };
                }
                else
                {
                    user = new Seller
                    {
                        UserName = model.Username,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = "+20 0"
                    };
                }

                await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, model.UserType == UserType.Buyer ? "Buyer" : "Seller");


                ///
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = Url.Action("ConfirmEmail", "Account",

                                new { userId = user.Id, token = token }, Request.Scheme);

                /// sending confirmation mail
                EmailMessage emailMessage = new EmailMessage
                {
                    To = user.Email,
                    Body = $"<p>Thank you for registering! To activate your account, please click the link below:</p>\r\n <a href='{confirmationLink}'>Activate</a>",
                    Subject = "Confirm your email"
                };

                _emailService.SendEmail(emailMessage);
                ///


                ViewBag.RegistrationMessage = "Before you can Login, please confirm your " +
                    "email, by clicking on the confirmation link we have emailed you";

                return View();

            }

            return View(model);
        }

        [HttpPost] // Reviewed
        public async Task<IActionResult> Login(LoginUserViewModel UserLoginVM) //normal login
        {
            if (ModelState.IsValid)
            {
                var userExist = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == UserLoginVM.Email);

                if (userExist != null)
                {
                    bool passwordCorrect = await _userManager.CheckPasswordAsync(userExist, UserLoginVM.Password);

                    if (passwordCorrect)
                    {
                        await _signInManager.SignInAsync(userExist, UserLoginVM.RememberMe);

                        UserHelper.LoggedinUserId = userExist.Id;

                        IList<string> roles = await _userManager.GetRolesAsync(userExist);

                        if (roles.FirstOrDefault() != "Admin")
                            return RedirectToAction("Index", "Home");
                      
                        else
                            return RedirectToAction("GetNotVerifiedSellers", "Admins");

                    }
                }
            }
            ModelState.AddModelError("LoginError", "Wrong Email Or Password");
            return View(UserLoginVM);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            ViewBag.message = null;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPasswordVM);
            }

            var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Account",
                                new { email = user.Email, token = token }, Request.Scheme);

                var tokenExpiration = DateTimeOffset.Now.AddHours(1);

                /// sending  mail
                EmailMessage emailMessage = new EmailMessage
                {
                    To = forgotPasswordVM.Email,
                    Body = $"<p>To reset your password, please click the link below:</p>\r\n <a href='{forgotPasswordLink}'>Reset</a>",
                    Subject = "Reset Password"
                };

                _emailService.SendEmail(emailMessage);
                ViewBag.message = "Password reset link sent! Please check your email.";
            }
            return View(forgotPasswordVM);
        }

        [HttpGet] // reviewed
        public IActionResult ResetPassword(string email, string token)
        {
            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                //if (!IsPasswordCompatible(model.Password))
                //{
                //	ModelState.AddModelError("Password", "Passwords must be at least 6 characters, at least one digit, one lowercase letter, one uppercase letter, and one non-alphanumeric character.");
                //	return View(model);
                //}

                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var isValidToken = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, UserManager<AppUser>.ResetPasswordTokenPurpose, model.Token);

                if (isValidToken)
                {


                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, resetToken, model.Password);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid or expired reset token.");

                }
            }

            return View(model);
        }

        // REVIEWED
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme); // if google
            await _signInManager.SignOutAsync(); // if normal login
            return RedirectToAction("Login", "Account");
        }


        //private bool IsPasswordCompatible(string password)
        //{
        //	return password.Length >= 6
        //		&& password.Any(char.IsDigit)
        //		&& password.Any(char.IsLower)
        //		&& password.Any(char.IsUpper)
        //		&& password.Any(c => !char.IsLetterOrDigit(c));
        //}

        //private bool IsValidEmail(string email)
        //{
        //	string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        //	return Regex.IsMatch(email, emailPattern);
        //}
    }

}
