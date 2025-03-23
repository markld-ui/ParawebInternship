#region Заголовок файла

/// <summary>
/// Файл: UserDTO.cs
/// Класс Data Transfer Object (DTO) для сущности <see cref="User"/>.
/// Используется для передачи данных о пользователях между слоями приложения.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Models;

#endregion

namespace LandingAPI.DTO
{
    #region Класс UserDTO

    /// <summary>
    /// Data Transfer Object (DTO) для сущности <see cref="User"/>.
    /// </summary>
    public class UserDTO
    {
        #region Свойства

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Имя пользователя. Обязательное поле.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Электронная почта пользователя. Обязательное поле.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Пароль пользователя. Обязательное поле.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Идентификатор роли пользователя.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Дата и время создания пользователя. По умолчанию устанавливается текущее время в UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        #endregion
    }

    #endregion
}