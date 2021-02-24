#region

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace AccountWeb.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;

        private UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public ViewResult Login(string returnUrl)
        {
            return View(new RegisterModel() {ReturnUrl = returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View("Login", model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user,
                    model.Password,
                    false,
                    false);
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Country, "Russia"));
            }
            else
            {
                ApplicationUser newUser = new()
                {
                    Email = model.Email,
                    UserName = model.Email,
                    DateRegistration = DateTime.Now
                };

                await _userManager.CreateAsync(newUser, model.Password);

                var result = await _signInManager.PasswordSignInAsync(newUser,
                    model.Password,
                    false,
                    false);
            }

            return Redirect(model.ReturnUrl);
        }


        public IActionResult GoogleLogin(string returnUrl)
        {
            var url = Url.Action("GoogleResponse", "Account", new {ReturnUrl = returnUrl});
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(
                GoogleDefaults.AuthenticationScheme,
                url);

            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, properties);
        }

        public async Task<IActionResult> GoogleResponse(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null) return RedirectToAction("Login");

            var user = await _userManager
                .FindByEmailAsync(info.Principal.FindFirst(ClaimTypes.Email)?.Value);

            if (user != null)
            {
                await _signInManager.SignInAsync(user, false);
                (HttpContext.User.Identity as ClaimsIdentity)?.AddClaim(new Claim(ClaimTypes.UserData,
                    user.DateRegistration.ToString("hh:mm:ss")));
                await HttpContext.SignInAsync(User);
                return Redirect(returnUrl);
            }

            user = new ApplicationUser()
            {
                UserName = info.Principal.FindFirst(ClaimTypes.Name)?.Value ?? "Laika",
                Email = info.Principal.FindFirst(ClaimTypes.Email)?.Value,
                DateRegistration = DateTime.Now
            };
            var identityResult = await _userManager.CreateAsync(user);
            if (identityResult.Succeeded)
            {
                identityResult = await _userManager.AddLoginAsync(user, info);
                if (identityResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return Redirect(returnUrl);
                }
            }

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}