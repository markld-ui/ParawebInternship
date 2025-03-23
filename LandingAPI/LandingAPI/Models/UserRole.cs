#region Заголовок файла

/// <summary>
/// Файл: UserRole.cs
/// Класс, представляющий сущность "Связь пользователя и роли" (UserRole).
/// Используется для реализации связи Many-to-Many между сущностями <see cref="User"/> и <see cref="Role"/>.
/// </summary>

#endregion

namespace LandingAPI.Models
{
    #region Класс UserRole

    /// <summary>
    /// Класс, представляющий сущность "Связь пользователя и роли".
    /// </summary>
    public class UserRole
    {
        #region Свойства

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Пользователь, связанный с ролью.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Идентификатор роли.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Роль, связанная с пользователем.
        /// </summary>
        public Role Role { get; set; }

        #endregion
    }

    #endregion
}