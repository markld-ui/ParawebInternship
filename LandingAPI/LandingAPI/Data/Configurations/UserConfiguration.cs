#region Заголовок файла

/// <summary>
/// Файл: UserConfiguration.cs
/// Класс для настройки конфигурации сущности <see cref="User"/> в базе данных с использованием Entity Framework Core.
/// Определяет правила маппинга, ограничения и связи для таблицы, соответствующей сущности <see cref="User"/>.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace LandingAPI.Data.Configurations
{
    #region Класс UserConfiguration

    /// <summary>
    /// Конфигурация сущности <see cref="User"/> для Entity Framework Core.
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        #region Метод Configure

        /// <summary>
        /// Настраивает сущность <see cref="User"/> и ее маппинг в базе данных.
        /// </summary>
        /// <param name="builder">Объект <see cref="EntityTypeBuilder{User}"/> для настройки сущности.</param>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Установка первичного ключа
            builder.HasKey(u => u.UserId);

            // Настройка обязательных полей
            builder.Property(u => u.Username).IsRequired();

            // Установка уникального индекса для поля Email
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Email).IsRequired();

            // Настройка обязательных полей
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.CreatedAt).IsRequired();
        }

        #endregion
    }

    #endregion
}