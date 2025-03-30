#region Заголовок файла

/// <summary>
/// Файл: FilesConfiguration.cs
/// Класс для настройки конфигурации сущности <see cref="Files"/> в базе данных с использованием Entity Framework Core.
/// Определяет правила маппинга, ограничения и связи для таблицы, соответствующей сущности <see cref="Files"/>.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace LandingAPI.Data.Configurations
{
    #region Класс FilesConfiguration

    /// <summary>
    /// Конфигурация сущности <see cref="Files"/> для Entity Framework Core.
    /// </summary>
    public class FilesConfiguration : IEntityTypeConfiguration<Files>
    {
        #region Метод Configure

        /// <summary>
        /// Настраивает сущность <see cref="Files"/> и ее маппинг в базе данных.
        /// </summary>
        /// <param name="builder">Объект <see cref="EntityTypeBuilder{Files}"/> для настройки сущности.</param>
        public void Configure(EntityTypeBuilder<Files> builder)
        {
            // Установка первичного ключа
            builder.HasKey(f => f.FileId);

            // Настройка обязательных полей
            builder.Property(f => f.FileName).IsRequired();
            builder.Property(f => f.FilePath).IsRequired();
            builder.Property(f => f.UploadedAt).IsRequired();
        }

        #endregion
    }

    #endregion
}