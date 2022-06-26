using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profi.Data;
using Profi.Models;
using Profi.ViewModels;
using System.Linq;

namespace Profi.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context { get; }

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM home = new HomeVM
            {
                Testimonials = _context.Testimonials.ToList()
            };
            return View(home);
        }
    }
}
