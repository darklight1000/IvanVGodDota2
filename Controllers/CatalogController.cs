using HorseClub.Data;
using HorseClub.Models;
using HorseClub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IvanVGodDota2.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BookingService _bookingService;

        public CatalogController(ApplicationDbContext context, BookingService bookingService)
        {
            _context = context;
            _bookingService = bookingService;
        }

        public async Task<IActionResult> Index(string? category, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Services.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(s => s.Category == category);
            if (minPrice.HasValue && minPrice.Value > 0)
                query = query.Where(s => s.Price >= minPrice.Value);
            if (maxPrice.HasValue && maxPrice.Value > 0)
                query = query.Where(s => s.Price <= maxPrice.Value);

            var viewModel = new CatalogViewModel
            {
                Services = await query.OrderBy(s => s.Name).ToListAsync(),
                Categories = await _context.Services.Select(s => s.Category).Distinct().OrderBy(c => c).ToListAsync(),
                SelectedCategory = category,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            ViewData["Title"] = "Каталог";
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var service = await _context.Services
                .Include(s => s.Trainer)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null) return NotFound();

            // ПЕРЕДАЕМ ВСЕХ ТРЕНЕРОВ ДЛЯ ВЫПАДАЮЩЕГО СПИСКА
            ViewBag.Trainers = await _context.Trainers.ToListAsync();

            ViewData["Title"] = service.Name;
            return View(service);
        }

        [HttpGet]
        public async Task<IActionResult> GetSlots(string date, int serviceId, int? trainerId)
        {
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {
                var slots = await _bookingService.GetAvailableSlotsAsync(parsedDate, serviceId, trainerId);
                var result = slots.Select(s => s.ToString(@"hh\:mm")).ToList();
                return Json(result);
            }
            return BadRequest("Неверный формат даты");
        }
    }
}