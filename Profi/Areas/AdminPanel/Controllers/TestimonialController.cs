using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profi.Data;
using Profi.Models;
using Profi.Utilities;
using Profi.ViewModels;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Profi.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class TestimonialController : Controller { 
        private AppDbContext _context { get; }
        private IWebHostEnvironment _env { get; }

        public TestimonialController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            TestimonialVM testimonial = new TestimonialVM
            {
                Testimonials = _context.Testimonials.ToList()
            };
            return View(testimonial);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Testimonial testimonial)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!testimonial.Photo.CheckType())
            {
                ModelState.AddModelError("Photo", "File type must be Image");
                return View();
            }
            if (!testimonial.Photo.CheckSize(100))
            {
                ModelState.AddModelError("Photo", "Photo size must be less than 200kb");
                return View();
            }
            if(!(testimonial.Rating >= 0 && testimonial.Rating <= 100))
            {
                ModelState.AddModelError("Rating", "Rating must be between 0 and 10");
                return View();
            }

            testimonial.Url = await testimonial.Photo.SaveFileAsync(_env.WebRootPath, "assets", "images");
            await _context.Testimonials.AddAsync(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }
            var testimonialDb = await _context.Testimonials.FindAsync(id);
            if (testimonialDb == null)
            {
                return NotFound();
            }
            var path = Extension.GetPath(_env.WebRootPath, "assets", "images") + testimonialDb.Url;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.Testimonials.Remove(testimonialDb);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var testimonialDb = _context.Testimonials.Find(id);
            if(testimonialDb == null)
            {
                return NotFound();
            }
            return View(testimonialDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,Testimonial testimonial)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (id == null)
            {
                return BadRequest();
            }
            if (!testimonial.Photo.CheckType())
            {
                ModelState.AddModelError("Photo", "File type must be Image");
                return View();
            }
            if (!testimonial.Photo.CheckSize(200))
            {
                ModelState.AddModelError("Photo", "Photo size must be less than 200kb");
                return View();
            }
            if (!(testimonial.Rating >= 0 && testimonial.Rating <= 100))
            {
                ModelState.AddModelError("Rating", "Rating must be between 0 and 10");
                return View();
            }
            var testimonialDb = _context.Testimonials.Find(id);
            if (testimonialDb == null)
            {
                return NotFound();
            }
            var path = Extension.GetPath(_env.WebRootPath, "assets", "images") + testimonialDb.Url;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            testimonial.Url = await testimonial.Photo.SaveFileAsync(_env.WebRootPath, "assets", "images");
            testimonialDb.Url = testimonial.Url;
            testimonialDb.Name = testimonial.Name;
            testimonialDb.Speciality = testimonial.Speciality;
            testimonialDb.Description = testimonial.Description;
            testimonialDb.Rating = testimonial.Rating;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
