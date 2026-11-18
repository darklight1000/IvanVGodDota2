using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HorseClub.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }

        [Required]
        [Display(Name = "Дата и время")]
        public DateTime BookingDate { get; set; }

        public string Status { get; set; } = "Ожидает";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? Comment { get; set; }

        
        [Display(Name = "Количество")]
        public int Quantity { get; set; } = 1;

        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; }
    }
}