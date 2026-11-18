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

            // 1. Если выбрана дата в прошлом — сразу возвращаем пустой список
            if (date.Date < DateTime.Now.Date)
            {
                return slots;
            }

            // 2. Получаем уже занятые слоты на эту дату
            var busySlots = await _context.Bookings
                .Where(b => b.BookingDate.Date == date.Date
                         && b.ServiceId == serviceId
                         && b.TrainerId == trainerId
                         && b.Status != "Отменено")
                .Select(b => b.BookingDate.TimeOfDay)
                .ToListAsync();

            var now = DateTime.Now.TimeOfDay;

            for (var time = startTime; time < endTime; time += step)
            {
                // 3. Если это СЕГОДНЯ, и время уже прошло — пропускаем
                if (date.Date == DateTime.Now.Date && time <= now)
                {
                    continue;
                }

                // 4. Если слот не занят — добавляем
                if (!busySlots.Contains(time))
                {
                    slots.Add(time);
                }
            }

            return slots;
        }
    }
}