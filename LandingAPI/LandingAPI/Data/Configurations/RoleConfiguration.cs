#region Заголовок файла

/// <summary>
/// Файл: RoleConfiguration.cs
/// Класс для настройки конфигурации сущности <see cref="Role"/> в базе данных с использованием Entity Framework Core.
/// Определяет правила маппинга, ограничения и связи для таблицы, соответствующей сущности <see cref="Role"/>.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace LandingAPI.Data.Configurations
{
    #region Класс RoleConfiguration

    /// <summary>
    /// Конфигурация сущности <see cref="Role"/> для Entity Framework Core.
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        #region Метод Configure

        /// <summary>
        /// Настраивает сущность <see cref="Role"/> и ее маппинг в базе данных.
        /// </summary>
        /// <param name="builder">Объект <see cref="EntityTypeBuilder{Role}"/> для настройки сущности.</param>
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Установка первичного ключа
            builder.HasKey(r => r.RoleId);

            // Настройка обязательного поля
            builder.Property(rn => rn.Name).IsRequired();
        }

        #endregion
    }

    #endregion
}