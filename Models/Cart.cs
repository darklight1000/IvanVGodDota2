using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HorseClub.Models
{
    // Корзина пользователя
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }

        public List<CartItem> Items { get; set; } = new();
    }

    // Позиция в корзине (конкретный товар и его количество)
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        public Cart? Cart { get; set; }

        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        [Range(1, 100, ErrorMessage = "Количество должно быть от 1 до 100")]
        public int Quantity { get; set; } = 1;

        // Поле для хранения даты (используется только для услуг)
        [Display(Name = "Дата и время")]
        public DateTime? BookingDate { get; set; }

        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; }
    }
}