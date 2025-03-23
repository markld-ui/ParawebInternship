#region Заголовок файла

/// <summary>
/// Файл: NewsConfiguration.cs
/// Класс для настройки конфигурации сущности <see cref="News"/> в базе данных с использованием Entity Framework Core.
/// Определяет правила маппинга, ограничения и связи для таблицы, соответствующей сущности <see cref="News"/>.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace LandingAPI.Data.Configurations
{
    #region Класс NewsConfiguration

    /// <summary>
    /// Конфигурация сущности <see cref="News"/> для Entity Framework Core.
    /// </summary>
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        #region Метод Configure

        /// <summary>
        /// Настраивает сущность <see cref="News"/> и ее маппинг в базе данных.
        /// </summary>
        /// <param name="builder">Объект <see cref="EntityTypeBuilder{News}"/> для настройки сущности.</param>
        public void Configure(EntityTypeBuilder<News> builder)
        {
            // Установка первичного ключа
            builder.HasKey(n => n.NewsId);

            // Настройка обязательных полей
            builder.Property(n => n.Title).IsRequired();
            builder.Property(n => n.Content).IsRequired();
            builder.Property(n => n.CreatedAt).IsRequired();

            // Настройка связи с сущностью User (создатель новости)
            builder.HasOne(n => n.CreatedBy)
                .WithMany(u => u.News)
                .HasForeignKey(n => n.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);

            // Настройка связи с сущностью File (файл, связанный с новостью)
            builder.HasOne(n => n.File)
                   .WithMany()
                   .HasForeignKey(n => n.FileId)
                   .OnDelete(DeleteBehavior.SetNull);
        }

        #endregion
    }

    #endregion
}