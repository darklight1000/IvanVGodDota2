using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HorseClub.Data;
using HorseClub.Models;

namespace IvanVGodDota2.Controllers
{
    public class NewArrivalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewArrivalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Берем из БД только те услуги, у которых IsNew == true
            var newArrivals = await _context.Services
                                            .Where(s => s.IsNew)
                                            .OrderByDescending(s => s.CreatedAt)
                                            .ToListAsync();
            return View(newArrivals);
        }
    }
}