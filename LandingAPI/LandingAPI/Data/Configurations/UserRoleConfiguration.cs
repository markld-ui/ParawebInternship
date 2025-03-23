#region Заголовок файла

/// <summary>
/// Файл: UserRoleConfiguration.cs
/// Класс для настройки конфигурации сущности <see cref="UserRole"/> в базе данных с использованием Entity Framework Core.
/// Определяет правила маппинга, ограничения и связи для таблицы, соответствующей сущности <see cref="UserRole"/>.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace LandingAPI.Data.Configurations
{
    #region Класс UserRoleConfiguration

    /// <summary>
    /// Конфигурация сущности <see cref="UserRole"/> для Entity Framework Core.
    /// </summary>
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        #region Метод Configure

        /// <summary>
        /// Настраивает сущность <see cref="UserRole"/> и ее маппинг в базе данных.
        /// </summary>
        /// <param name="builder">Объект <see cref="EntityTypeBuilder{UserRole}"/> для настройки сущности.</param>
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            // Установка составного первичного ключа (UserId и RoleId)
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            // Настройка связи с сущностью User
            builder.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            // Настройка связи с сущностью Role
            builder.HasOne(ur => ur.Role)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
        }

        #endregion
    }

    #endregion
}