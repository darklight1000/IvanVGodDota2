using HorseClub.Models;

namespace HorseClub.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Если услуги уже есть — ничего не делаем
            if (context.Services.Any())
                return;

            var services = new List<Service>
            {
                new Service
                {
                    Name = "Индивидуальное занятие верховой ездой",
                    Description = "Персональное занятие с тренером для любого уровня подготовки. Длительность — 60 минут.",
                    Price = 3500,
                    Category = "Обучение",
                    ImageUrl = "https://images.unsplash.com/photo-1553284965-83fd3e82fa5a?w=600",
                    IsNew = false,
                    IsPromo = true
                },
                new Service
                {
                    Name = "Групповое занятие (до 6 человек)",
                    Description = "Занятие в мини-группе под руководством тренера. 60 минут.",
                    Price = 2000,
                    Category = "Обучение",
                    ImageUrl = "https://images.unsplash.com/photo-1534773728080-33d31da27ae5?w=600"
                },
                new Service
                {
                    Name = "Прокат лошади с инструктором (1 час)",
                    Description = "Прогулка по живописным маршрутам в сопровождении инструктора.",
                    Price = 2500,
                    Category = "Прокат",
                    ImageUrl = "https://images.unsplash.com/photo-1598974357801-cbca100e65d3?w=600",
                    IsNew = true
                },
                new Service
                {
                    Name = "Детская группа 5–10 лет",
                    Description = "Специальная программа для самых юных наездников. 45 минут.",
                    Price = 1800,
                    Category = "Обучение",
                    ImageUrl = "https://images.unsplash.com/photo-1568605114967-8130f3a36994?w=600",
                    IsNew = true
                },
                new Service
                {
                    Name = "Иппотерапия (сеанс 45 мин)",
                    Description = "Лечебная верховая езда с квалифицированным специалистом.",
                    Price = 3000,
                    Category = "Иппотерапия",
                    ImageUrl = "https://images.unsplash.com/photo-1551969014-7d2c4cddf0b6?w=600",
                    IsNew = true
                },
                new Service
                {
                    Name = "Шлем для верховой езды",
                    Description = "Сертифицированный шлем, размеры S-XL, регулируемый подбородочный ремень.",
                    Price = 4500,
                    Category = "Экипировка",
                    ImageUrl = "https://images.unsplash.com/photo-1553284965-83fd3e82fa5a?w=600"
                },
                new Service
                {
                    Name = "Краги (перчатки) кожаные",
                    Description = "Профессиональные перчатки для верховой езды, натуральная кожа.",
                    Price = 1800,
                    Category = "Экипировка",
                    ImageUrl = "https://images.unsplash.com/photo-1534773728080-33d31da27ae5?w=600"
                },
                new Service
                {
                    Name = "Выезды в поля (2 часа)",
                    Description = "Прогулки по живописным маршрутам Подмосковья с инструктором.",
                    Price = 4500,
                    Category = "Прокат",
                    ImageUrl = "https://images.unsplash.com/photo-1598974357801-cbca100e65d3?w=600",
                    IsPromo = true
                }
            };

            context.Services.AddRange(services);
            context.SaveChanges();
        }
    }
}