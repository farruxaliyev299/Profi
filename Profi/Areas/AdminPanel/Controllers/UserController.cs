using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Profi.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class UserController : Controller
    {
        public UserManager<AppUser> _userManager { get; set; }

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            List<AppUser> users= _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> Activate(string id)
        {
            AppUser userDb = await _userManager.FindByNameAsync(id);
            userDb.IsActivated = true;
            await _userManager.UpdateAsync(userDb);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Deactivate(string id)
        {
            AppUser userDb = await _userManager.FindByNameAsync(id);
            userDb.IsActivated = false;
            await _userManager.UpdateAsync(userDb);
            return RedirectToAction("Index");
        }
    }
}
