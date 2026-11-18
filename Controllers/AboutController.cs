using HorseClub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IvanVGodDota2.Controllers
{
    public class AboutController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AboutController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            ViewBag.Trainers = await _context.Trainers.ToListAsync();
            return View();
        }
    }
}