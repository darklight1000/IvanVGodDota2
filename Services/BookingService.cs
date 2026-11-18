using HorseClub.Data;
using Microsoft.EntityFrameworkCore;

namespace HorseClub.Services
{
    public class BookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Метод возвращает список доступных временных слотов на выбранную дату
        public async Task<List<TimeSpan>> GetAvailableSlotsAsync(DateTime date, int serviceId, int? trainerId)
        {
            var slots = new List<TimeSpan>();

            // Рабочий день с 9:00 до 18:00
            var startTime = new TimeSpan(9, 0, 0);
            var endTime = new TimeSpan(18, 0, 0);
            var step = TimeSpan.FromHours(1); // Шаг 1 час

            // Получаем уже занятые слоты на эту дату
            var busySlots = await _context.Bookings
                .Where(b => b.BookingDate.Date == date.Date
                         && b.ServiceId == serviceId
                         && b.TrainerId == trainerId
                         && b.Status != "Отменено")
                .Select(b => b.BookingDate.TimeOfDay)
                .ToListAsync();

            for (var time = startTime; time < endTime; time += step)
            {
                if (!busySlots.Contains(time))
                {
                    slots.Add(time);
                }
            }

            return slots;
        }
    }
}