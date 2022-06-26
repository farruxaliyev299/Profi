using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profi.Models;
using Profi.ViewModels.Account;
using System.Threading.Tasks;

namespace Profi.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AdminLoginController : Controller
    {
        private UserManager<AppUser> _userManager { get; }
        private SignInManager<AppUser> _signInManager { get; }

        public AdminLoginController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM user)
        {
            AppUser userDb = await _userManager.FindByEmailAsync(user.Email);
            if (userDb == null)
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
                return View();
            }
            var signInResult = await _signInManager.PasswordSignInAsync(userDb, user.Password, user.IsPersistent, lockoutOnFailure: true);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
                return View();
            }
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Too many failed attempts , please try again in a few minutes");
                return View();
            }
            if (!userDb.IsActivated)
            {
                ModelState.AddModelError("", "You have no permisson to access AdminPanel right now , Wait for activation by Admin");
                return View();
            }
            return RedirectToAction("Index","Dashboard");
        }
    }
}
