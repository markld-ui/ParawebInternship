#region Заголовок файла

/// <summary>
/// Файл: Role.cs
/// Класс, представляющий сущность "Роль" (Role).
/// Используется для хранения данных о ролях пользователей и их связях с пользователями.
/// </summary>

#endregion

namespace LandingAPI.Models
{
    #region Класс Role

    /// <summary>
    /// Класс, представляющий сущность "Роль".
    /// </summary>
    public class Role
    {
        #region Свойства

        /// <summary>
        /// Идентификатор роли.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Название роли.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Коллекция связей между ролями и пользователями (Many-to-Many: Role <-> Users).
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        #endregion
    }

    #endregion
}