using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HorseClub.Data;          // Для ApplicationDbContext
using HorseClub.Models;        // Для модели Service

namespace IvanVGodDota2.Controllers
{
    public class PromotionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PromotionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Берем из БД только те услуги, у которых IsPromo == true
            var promotions = await _context.Services
                                           .Where(s => s.IsPromo)
                                           .OrderByDescending(s => s.CreatedAt)
                                           .ToListAsync();
            return View(promotions);
        }
    }
}