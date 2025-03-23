#region Заголовок файла

/// <summary>
/// Файл: AssignRoleDTO.cs
/// Класс Data Transfer Object (DTO) для передачи данных о назначении или удалении роли пользователю.
/// Используется для взаимодействия между слоями приложения.
/// </summary>

#endregion

#region Пространства имен

// Здесь можно добавить используемые пространства имен, если они есть.
// Например:
// using LandingAPI.Models;

#endregion

namespace LandingAPI.DTO
{
    #region Класс AssignRoleDTO

    /// <summary>
    /// Data Transfer Object (DTO) для передачи данных о назначении или удалении роли пользователю.
    /// </summary>
    public class AssignRoleDTO
    {
        #region Свойства

        /// <summary>
        /// Идентификатор пользователя, которому назначается или удаляется роль.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Идентификатор роли, которая назначается или удаляется.
        /// </summary>
        public int RoleId { get; set; }

        #endregion
    }

    #endregion
}