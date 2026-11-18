using HorseClub.Data;
using HorseClub.Models;
using IvanVGodDota2.Models;
using Microsoft.AspNetCore.Authorization; // <-- ДОБАВЛЕНО для [Authorize]
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace IvanVGodDota2.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Показать все отзывы (доступно всем)
        public IActionResult Index()
        {
            var reviews = _context.Reviews.OrderByDescending(r => r.CreatedAt).ToList();
            return View(reviews);
        }

        // Обработка формы добавления отзыва
        [HttpPost]
        [Authorize] // <-- ДОБАВЛЕНО: Только для авторизованных пользователей
        [ValidateAntiForgeryToken] // <-- ДОБАВЛЕНО: Защита от CSRF-атак
        public IActionResult AddReview(Review review)
        {
            if (ModelState.IsValid)
            {
                review.CreatedAt = DateTime.Now;

                // Если вы хотите принудительно записывать имя из аккаунта, раскомментируйте строку ниже:
                // review.AuthorName = User.Identity?.Name ?? review.AuthorName;

                _context.Reviews.Add(review);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Если есть ошибки валидации, возвращаем обратно с тем же списком
            var reviews = _context.Reviews.OrderByDescending(r => r.CreatedAt).ToList();
            return View("Index", reviews);
        }
    }
}