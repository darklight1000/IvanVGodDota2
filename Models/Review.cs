using System;
using System.ComponentModel.DataAnnotations;

namespace IvanVGodDota2.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        public string AuthorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите текст отзыва")]
        public string Text { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int Rating { get; set; } = 5;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}