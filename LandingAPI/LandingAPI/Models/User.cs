#region Заголовок файла

/// <summary>
/// Файл: User.cs
/// Класс, представляющий сущность "Пользователь" (User).
/// Используется для хранения данных о пользователях, их ролях, созданных новостях и мероприятиях.
/// </summary>

#endregion

namespace LandingAPI.Models
{
    #region Класс User

    /// <summary>
    /// Класс, представляющий сущность "Пользователь".
    /// </summary>
    public class User
    {
        #region Свойства

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Электронная почта пользователя.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Хэш пароля пользователя.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Коллекция связей между пользователями и ролями (Many-to-Many: User <-> Roles).
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        /// <summary>
        /// Коллекция новостей, созданных пользователем (One-to-Many: User -> News).
        /// </summary>
        public ICollection<News> News { get; set; } = new List<News>();

        /// <summary>
        /// Коллекция мероприятий, созданных пользователем (One-to-Many: User -> Events).
        /// </summary>
        public ICollection<Event> CreatedEvents { get; set; } = new List<Event>();

        /// <summary>
        /// Дата и время создания пользователя. По умолчанию устанавливается текущее время в UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Refresh-токен пользователя, используемый для обновления JWT-токена.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Дата и время истечения срока действия refresh-токена.
        /// </summary>
        public DateTime RefreshTokenExpiryTime { get; set; }

        #endregion
    }

    #endregion
}