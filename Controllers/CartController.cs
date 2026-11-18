using HorseClub.Data;
using HorseClub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IvanVGodDota2.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var cart = await GetOrCreateCart(userId);

            var cartWithItems = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Service)
                .Include(c => c.Items)
                .ThenInclude(i => i.Trainer)
                .FirstOrDefaultAsync(c => c.Id == cart.Id);

            return View(cartWithItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int serviceId, int quantity = 1, DateTime? bookingDate = null, int? trainerId = null)
        {
            var userId = _userManager.GetUserId(User);
            var cart = await GetOrCreateCart(userId);

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ServiceId == serviceId && i.BookingDate == bookingDate && i.TrainerId == trainerId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ServiceId = serviceId,
                    Quantity = quantity,
                    BookingDate = bookingDate,
                    TrainerId = trainerId
                });
            }

            await _context.SaveChangesAsync();

            if (bookingDate.HasValue)
                return RedirectToAction(nameof(Index));

            return Redirect(Request.Headers["Referer"].ToString() ?? "/Catalog");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            var userId = _userManager.GetUserId(User);
            var cartItem = await _context.CartItems
                .Include(i => i.Cart)
                .FirstOrDefaultAsync(i => i.Id == cartItemId && i.Cart.UserId == userId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            var userId = _userManager.GetUserId(User);

            // ВАЖНО: Подгружаем Service через ThenInclude, иначе у item.Service может быть null
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Service)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
                return RedirectToAction(nameof(Index));

            foreach (var item in cart.Items)
            {
                var bookingDate = item.BookingDate ?? DateTime.Now.AddDays(1);

                _context.Bookings.Add(new Booking
                {
                    UserId = userId!,
                    ServiceId = item.ServiceId,
                    BookingDate = bookingDate,
                    Status = "Ожидает",
                    Quantity = item.Quantity,
                    TrainerId = item.TrainerId,
                    Comment = item.Service?.Category == "Экипировка" ? "Заказ товара" : "Запись из корзины"
                });
            }

            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Заказ успешно оформлен!";
            return RedirectToAction("Index", "Account");
        }

        private async Task<Cart> GetOrCreateCart(string userId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
            return cart;
        }
    }
}