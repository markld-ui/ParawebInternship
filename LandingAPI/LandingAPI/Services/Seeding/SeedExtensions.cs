using LandingAPI.Data;

namespace LandingAPI.Services.Seeding
{
    public static class SeedExtensions
    {
        public static void SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    var seeder = new Seed(context);
                    seeder.SeedDataContext();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Произошла ошибка при заполнении базы данных!");
                }
            }
        }
    }
}
