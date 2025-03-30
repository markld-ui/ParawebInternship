#region Пространство имен

using System;

#endregion

namespace LandingAPI.DTO.Users
{
    #region Класс UserShortDTO
    /// <summary>
    /// Data Transfer Object (DTO) для представления краткой информации о пользователе.
    /// </summary>
    public class UserShortDTO
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
        /// Основная роль пользователя.
        /// </summary>
        public string MainRole { get; set; }
        #endregion
    }
    #endregion
}
