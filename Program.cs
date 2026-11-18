using HorseClub.Data;
using IvanVGodDota2;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IvanVGodDota2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. БАЗА ДАННЫХ
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Строка подключения 'DefaultConnection' не найдена.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // 2. IDENTITY (пользователи + роли)
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // 3. НАСТРОЙКА КУКИ
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Login";

                options.Events.OnSigningIn = async context =>
                {
                    var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
                    var user = await userManager.GetUserAsync(context.Principal!);
                    if (user != null)
                    {
                        var roles = await userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            ((System.Security.Claims.ClaimsIdentity)context.Principal.Identity).AddClaim(
                                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role));
                        }
                    }
                };
            });

            builder.Services.AddRazorPages();

            // 4. MVC
            builder.Services.AddControllersWithViews();

            // 5. НАШ НОВЫЙ СЕРВИС ДЛЯ ТАЙМ-СЛОТОВ
            builder.Services.AddScoped<HorseClub.Services.BookingService>();

            var app = builder.Build();

            // 6. PIPELINE
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.MapRazorPages();

            // 7. ИНИЦИАЛИЗАЦИЯ БД И РОЛЕЙ
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
                DbSeeder.Seed(db);

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roles = { "Admin", "Manager", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                string[] adminEmails = {
                    "admin@centaur.ru",
                    "boss@gmail.com"
                };

                string adminPassword = "Admin123";

                foreach (var email in adminEmails)
                {
                    var adminUser = await userManager.FindByEmailAsync(email);

                    if (adminUser == null)
                    {
                        adminUser = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                        var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                        if (createResult.Succeeded)
                        {
                            await userManager.AddToRoleAsync(adminUser, "Admin");
                        }
                    }
                    else
                    {
                        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                        {
                            await userManager.AddToRoleAsync(adminUser, "Admin");
                        }
                    }
                }
            }

            app.Run();
        }
    }
}