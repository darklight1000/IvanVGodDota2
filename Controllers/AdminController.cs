using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HorseClub.Data;
using HorseClub.Models;
using IvanVGodDota2.Models;

namespace IvanVGodDota2.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index() => View();

        // ==================== УСЛУГИ ====================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Services()
        {
            var services = await _context.Services.Include(s => s.Trainer).ToListAsync();
            return View(services);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateService()
        {
            ViewBag.Trainers = _context.Trainers.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateService(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Services));
            }
            ViewBag.Trainers = _context.Trainers.ToList();
            return View(service);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditService(int? id)
        {
            if (id == null) return NotFound();
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            ViewBag.Trainers = _context.Trainers.ToList();
            return View(service);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(int id, Service service)
        {
            if (id != service.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Services.Any(e => e.Id == service.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Services));
            }
            ViewBag.Trainers = _context.Trainers.ToList();
            return View(service);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Services));
        }

        // ==================== ТРЕНЕРЫ ====================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Trainers() => View(await _context.Trainers.ToListAsync());

        [Authorize(Roles = "Admin")]
        public IActionResult CreateTrainer() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainer(Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                _context.Trainers.Add(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Trainers));
            }
            return View(trainer);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditTrainer(int? id)
        {
            if (id == null) return NotFound();
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null) return NotFound();
            return View(trainer);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrainer(int id, Trainer trainer)
        {
            if (id != trainer.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Trainers.Any(e => e.Id == trainer.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Trainers));
            }
            return View(trainer);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Trainers));
        }

        // ==================== АКЦИИ ====================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Promotions()
        {
            var promotions = await _context.Services.Where(s => s.IsPromo).ToListAsync();
            return View(promotions);
        }

        // ==================== НОВИНКИ ====================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NewArrivals()
        {
            var newArrivals = await _context.Services.Where(s => s.IsNew).ToListAsync();
            return View(newArrivals);
        }

        // ==================== ОТЗЫВЫ ====================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reviews()
        {
            var reviews = await _context.Reviews.OrderByDescending(r => r.CreatedAt).ToListAsync();
            return View(reviews);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Reviews));
        }

        // ==================== БРЕНДЫ ====================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Brands() => View(await _context.Brands.ToListAsync());

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Brands));
        }

        // ==================== БРОНИРОВАНИЯ ====================
        public async Task<IActionResult> Bookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Service)
                .Include(b => b.Trainer) // ВЫВОДИМ ТРЕНЕРА В БРОНИРОВАНИЯХ
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBookingStatus(int id, string status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = status;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Bookings));
        }

        // ==================== ПОЛЬЗОВАТЕЛИ ====================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users() => View(await _userManager.Users.ToListAsync());

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUserRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();
            ViewBag.User = user;
            ViewBag.UserRoles = userRoles;
            ViewBag.AllRoles = allRoles;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUserRoles(string userId, List<string> selectedRoles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (selectedRoles != null && selectedRoles.Any())
                await _userManager.AddToRolesAsync(user, selectedRoles);
            return RedirectToAction(nameof(Users));
        }
    }
}