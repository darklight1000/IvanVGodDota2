using HorseClub.Data;
using HorseClub.Models;
using IvanVGodDota2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HorseClub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Promotions = await _context.Services
                .Where(s => s.IsPromo)
                .Take(2)
                .ToListAsync();

            ViewBag.NewArrivals = await _context.Services
                .Where(s => s.IsNew)
                .Take(3)
                .ToListAsync();

            ViewBag.PopularServices = await _context.Services
                .Take(3)
                .ToListAsync();

            ViewBag.Reviews = await _context.Reviews
                .OrderByDescending(r => r.CreatedAt)
                .Take(2)
                .ToListAsync();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}