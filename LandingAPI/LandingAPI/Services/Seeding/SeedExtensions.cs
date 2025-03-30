#region Заголовок файла

/// <summary>
/// Файл: SeedExtensions.cs
/// Класс расширений для сидирования базы данных.
/// Предоставляет метод для заполнения базы данных начальными данными при запуске приложения.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Data;
using LandingAPI.Interfaces.Auth;
using LandingAPI.Services.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

#endregion

namespace LandingAPI.Services.Seeding
{
    #region Класс SeedExtensions

    /// <summary>
    /// Класс расширений для сидирования базы данных.
    /// </summary>
    public static class SeedExtensions
    {
        #region Методы

        #region SeedData
        /// <summary>
        /// Заполняет базу данных начальными данными при запуске приложения.
        /// </summary>
        /// <param name="host">Хост приложения.</param>
        public static void SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Получение контекста базы данных
                    var context = services.GetRequiredService<DataContext>();

                    // Создание экземпляра сидирования и заполнение данных
                    var seeder = new Seed(context);
                    seeder.SeedDataContext();
                }
                catch (Exception ex)
                {
                    // Логирование ошибки, если сидирование не удалось
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Произошла ошибка при заполнении базы данных!");
                }
            }
        }
        #endregion

        #endregion
    }

    #endregion
}