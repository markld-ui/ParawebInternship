#region Заголовок файла

/// <summary>
/// Файл: DataContext.cs
/// Класс контекста базы данных, представляющий сессию с базой данных и позволяющий выполнять операции с сущностями.
/// Наследуется от <see cref="DbContext"/> и настраивает связи между сущностями, а также применяет конфигурации для каждой сущности.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using LandingAPI.Data.Configurations;

#endregion

namespace LandingAPI.Data
{
    #region Класс DataContext

    /// <summary>
    /// Контекст базы данных для приложения LandingAPI.
    /// </summary>
    public class DataContext : DbContext
    {
        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DataContext"/>.
        /// </summary>
        /// <param name="options">Параметры конфигурации для контекста базы данных.</param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        #endregion

        #region DbSet (Таблицы базы данных)

        /// <summary>
        /// Таблица пользователей.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Таблица событий.
        /// </summary>
        public DbSet<Event> Events { get; set; }

        /// <summary>
        /// Таблица новостей.
        /// </summary>
        public DbSet<News> News { get; set; }

        /// <summary>
        /// Таблица файлов.
        /// </summary>
        public DbSet<Files> Files { get; set; }

        /// <summary>
        /// Таблица ролей.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Таблица связи пользователей и ролей.
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }

        #endregion

        #region Метод OnModelCreating

        /// <summary>
        /// Настраивает модель базы данных и применяет конфигурации для сущностей.
        /// </summary>
        /// <param name="modelBuilder">Объект <see cref="ModelBuilder"/> для настройки модели.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Применение конфигураций для сущностей
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new FilesConfiguration());
        }

        #endregion
    }

    #endregion
}