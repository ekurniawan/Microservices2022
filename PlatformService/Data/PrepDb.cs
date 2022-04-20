using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateAsyncScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
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