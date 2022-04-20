using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app,bool isProd)
        {
            using(var serviceScope = app.ApplicationServices.CreateAsyncScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(),isProd);
            }
        }

        private static void SeedData(AppDbContext context,bool isProd)
        {
            if(isProd)
            {
                Console.WriteLine("--> Jalankan Perintah Migrasi");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Perintah Migrasi gagal {ex.Message}");
                }
            }

            if(!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.Platforms.AddRange(
                    new Platform {Name="ASP Core",Publisher="Microsoft",Cost="Free"},
                    new Platform {Name="SQL Server Express",Publisher="Microsoft",Cost="Free"},
                    new Platform {Name="Kurbernetes",Publisher="Cloud Native Foundation",Cost="Free"}
                );
                context.SaveChanges();
            }
            else 
            {
                Console.WriteLine("--> Data Platforms sudah ada..");
            }
        }
    }
}