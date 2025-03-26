#region Заголовок файла

/// <summary>
/// Файл: AssignRoleDTO.cs
/// Класс Data Transfer Object (DTO) для передачи данных о назначении или удалении роли пользователю.
/// Используется для взаимодействия между слоями приложения.
/// </summary>

#endregion

#region Пространства имен

using System.ComponentModel.DataAnnotations;

#endregion

namespace LandingAPI.DTO.Roles
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
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Идентификатор роли, которая назначается или удаляется.
        /// </summary>
        [Required]
        public int RoleId { get; set; }

        #endregion
    }

    #endregion
}