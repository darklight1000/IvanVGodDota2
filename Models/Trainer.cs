using System.ComponentModel.DataAnnotations;

namespace HorseClub.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите имя тренера")]
        [Display(Name = "Имя тренера")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Специализация")]
        public string Specialization { get; set; } = string.Empty;

        [Display(Name = "Фото (URL)")]
        public string? ImageUrl { get; set; }

        [Display(Name = "О тренере")]
        public string? Description { get; set; }

        
        public List<Service> Services { get; set; } = new();
    }
}