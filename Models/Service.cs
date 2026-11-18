using System.ComponentModel.DataAnnotations;

namespace HorseClub.Models
{
    // Услуга или товар клуба: тренировка, прокат, экипировка
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название")]
        [StringLength(150)]
        [Display(Name = "Название")]
        public string Name { get; set; } = string.Empty;

        [StringLength(2000)]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Range(0, 1000000, ErrorMessage = "Цена должна быть от 0 до 1 000 000")]
        [Display(Name = "Цена, ₽")]
        public decimal Price { get; set; }

        [Display(Name = "Изображение (URL)")]
        public string? ImageUrl { get; set; }

        // Категория: "Обучение", "Прокат", "Экипировка", "Иппотерапия"
        [Required]
        [StringLength(50)]
        [Display(Name = "Категория")]
        public string Category { get; set; } = "Обучение";

        // Для раздела "Новинки" на главной
        [Display(Name = "Новинка")]
        public bool IsNew { get; set; } = false;

        // Для раздела "Акции"
        [Display(Name = "На акции")]
        public bool IsPromo { get; set; } = false;

        [Display(Name = "Дата добавления")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Тренер")]
        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; }
    }
}