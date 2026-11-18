using System;
using System.Threading.Tasks;
using HorseClub.Data;
using HorseClub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HorseClub.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BookingController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int serviceId, DateTime bookingDate, string? comment, int? trainerId)
        {
            if (bookingDate < DateTime.Now)
            {
                TempData["BookingError"] = "Дата бронирования должна быть в будущем.";
                return RedirectToAction("Details", "Catalog", new { id = serviceId });
            }

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var booking = new Booking
            {
                ServiceId = serviceId,
                UserId = userId,
                BookingDate = bookingDate,
                Comment = comment,
                Status = "Ожидает",
                Quantity = 1,
                TrainerId = trainerId // ДОБАВЛЕНО
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["BookingSuccess"] = "Бронирование успешно создано!";
            return RedirectToAction("Index", "Account");
        }
    }
}