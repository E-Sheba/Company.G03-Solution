using Company.G03.DAL.Models;
using Company.G03.PL.Helpers;
using Company.G03.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Company.G03.PL.Controllers
{
    public class AccountController : Controller
    {
        #region Injection
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //SignIn Manage is responsible for Authentication
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        } 
        #endregion

        #region SignUp

        [HttpGet]  //BaseUrl // Account /SignUp
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //SignUp
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    if (user == null)
                    {
                        user = await _userManager.FindByEmailAsync(model.Email);
                        if (user is null)
                        {
                            user = new ApplicationUser()
                            {
                                UserName = model.UserName,
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Email = model.Email,
                                IsAgree = model.IsAgree

                            };
                            var result = await _userManager.CreateAsync(user, model.Password);
                            if (result.Succeeded)
                            {
                                return RedirectToAction("SignIn");
                            }
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                        ModelState.AddModelError(string.Empty, "Email Does Already Exist");
                        return View(model);
                    }
                    ModelState.AddModelError(string.Empty, "UserName Does Already Exist");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }
        #endregion

        #region SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is not null)
                    {
                        //Check Password
                        var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                        if (flag)
                        {
                            //Generate Token
                            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                            if (result.Succeeded)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Invalid LogIn");
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }

        #endregion

        #region SignOut

        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn");
        }

        #endregion

        #region Forget Password
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is not null)
                    {
                        //Create Token so that I can't change my friends' passwords [Hacking them litterally]
                        //so we generate token for each request sent by the user and sent it along with the request of resetting the password
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                        //Create Reset Password Url
                        //the Controller class contains a property called url that we inherit from
                        //we also inherit a property called Request that contains another property called Scheme that Represents the static part of the URL
                        //that's predefined in the appsettings.json

                        var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                        //https//localhost/7040/Account/ResetPassword?email=ahmed@gmail.com&token

                        //Create Email
                        var email = new Email()
                        {
                            To = model.Email,
                            Subject = "Reset Password",
                            Body = url
                        };
                        //Send Email ()
                        EmailSettings.SendEmail(email);

                        return RedirectToAction(nameof(CheckYourInbox));
                    }
                    ModelState.AddModelError(string.Empty, "Invalid operation , please try again");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string? token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var email = TempData["email"] as string;
                    var token = TempData["token"] as string;
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user is not null)
                    {

                        var ResetResult = await _userManager.ResetPasswordAsync(user, token, model.Password);
                        if (ResetResult.Succeeded)
                        {
                            return RedirectToAction(nameof(SignIn));
                        }
                    }

                    ModelState.AddModelError(string.Empty, "Invalid operation , please try again");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty,ex.Message);
                }
            }

            return View(model);
        }


        #endregion

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
