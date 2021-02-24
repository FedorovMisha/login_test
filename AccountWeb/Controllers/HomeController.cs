using System.Collections.Generic;
using System.Security.Claims;
using AccountWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AccountWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET
        public IActionResult Index()
        {
            var userPresentation = new UserPresentationModel()
            {
                UserId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                UserName = HttpContext.User?.Identity?.Name,
                Date = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.UserData)?.Value,
                // Date = _userManager.FindByIdAsync((HttpContext.User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value).Result.DateRegistration.ToString("hh::mm:ss")
            };

            return View(userPresentation);
        }
        [AllowAnonymous]
        public IActionResult Authenticate()
        {
            var grandmaClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Bob"),
                new Claim(ClaimTypes.Email, "boob@gmail.com"),
            };
            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var userPrincipal = new ClaimsPrincipal(new[]
            {
                grandmaIdentity
            });

            HttpContext.SignInAsync(userPrincipal);
            
            return RedirectToAction("Index");
        }
        
    }
}