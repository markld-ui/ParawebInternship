#region Заголовок файла

/// <summary>
/// Файл: EventConfiguration.cs
/// Класс для настройки конфигурации сущности <see cref="Event"/> в базе данных с использованием Entity Framework Core.
/// Определяет правила маппинга, ограничения и связи для таблицы, соответствующей сущности <see cref="Event"/>.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace LandingAPI.Data.Configurations
{
    #region Класс EventConfiguration

    /// <summary>
    /// Конфигурация сущности <see cref="Event"/> для Entity Framework Core.
    /// </summary>
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        #region Метод Configure

        /// <summary>
        /// Настраивает сущность <see cref="Event"/> и ее маппинг в базе данных.
        /// </summary>
        /// <param name="builder">Объект <see cref="EntityTypeBuilder{Event}"/> для настройки сущности.</param>
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            // Установка первичного ключа
            builder.HasKey(e => e.EventId);

            // Настройка обязательных полей
            builder.Property(e => e.Title).IsRequired();
            builder.Property(e => e.Description).IsRequired();
            builder.Property(e => e.StartDate).IsRequired();
            builder.Property(e => e.EndDate).IsRequired();
            builder.Property(e => e.Location).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();

            // Настройка связи с сущностью User (создатель события)
            builder.HasOne(e => e.CreatedBy)
                   .WithMany(u => u.CreatedEvents)
                   .HasForeignKey(e => e.CreatedById)
                   .OnDelete(DeleteBehavior.Cascade);

            // Настройка связи с сущностью File (файл, связанный с событием)
            builder.HasOne(e => e.File)
                .WithMany()
                .HasForeignKey(e => e.FileId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        #endregion
    }

    #endregion
}