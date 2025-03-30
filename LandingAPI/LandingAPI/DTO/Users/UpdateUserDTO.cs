#region Пространство имен

using System.ComponentModel.DataAnnotations;

#endregion

namespace LandingAPI.DTO.Users
{
    #region Класс UpdateUserDTO
    /// <summary>
    /// Data Transfer Object (DTO) для обновления информации о пользователе.
    /// </summary>
    public class UpdateUserDTO
    {
        #region Свойства
        /// <summary>
        /// Имя пользователя. Не должно превышать 50 символов.
        /// </summary>
        [StringLength(50, ErrorMessage = "Имя пользователя не должно превышать 50 символов")]
        public string? Username { get; set; }

        /// <summary>
        /// Email пользователя. Должен соответствовать корректному формату email.
        /// </summary>
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string? Email { get; set; }

        /// <summary>
        /// Пароль пользователя. Должен содержать минимум 6 символов.
        /// </summary>
        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        public string? Password { get; set; }

        /// <summary>
        /// Идентификатор роли пользователя. Может быть null, если роль не изменяется.
        /// </summary>
        public int? RoleId { get; set; }
        #endregion
    }
    #endregion
}
