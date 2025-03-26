#region Пространство имен

using System;
using System.Collections.Generic;

#endregion

namespace LandingAPI.DTO.Users
{
    #region Класс UserDetailsDTO
    /// <summary>
    /// Data Transfer Object (DTO) для представления информации о пользователе.
    /// </summary>
    public class UserDetailsDTO
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор пользователя.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email пользователя.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Дата и время создания учетной записи пользователя.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Список ролей, присвоенных пользователю.
        /// </summary>
        public List<RoleDTO> Roles { get; set; } = new();
        #endregion
    }
    #endregion

    #region Класс RoleDTO
    /// <summary>
    /// Data Transfer Object (DTO) для представления роли пользователя.
    /// </summary>
    public class RoleDTO
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор роли.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Название роли.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
    #endregion
}
