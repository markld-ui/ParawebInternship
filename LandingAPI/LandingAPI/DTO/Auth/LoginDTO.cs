﻿#region Заголовок файла
// Файл: LoginDTO.cs
// Класс Data Transfer Object (DTO) для передачи данных, необходимых для аутентификации пользователя.
// Используется для передачи данных о входе в систему (email и пароль) между слоями приложения.
#endregion

#region Пространства имен

using System.ComponentModel.DataAnnotations;

#endregion

namespace LandingAPI.DTO.Auth
{
    #region Класс LoginDTO
    /// <summary>
    /// Data Transfer Object (DTO) для передачи данных, необходимых для аутентификации пользователя.
    /// </summary>
    public class LoginDTO
    {
        #region Свойства

        /// <summary>
        /// Электронная почта пользователя. Используется для идентификации пользователя.
        /// </summary>
        /// <example>user@example.com</example>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя. Используется для проверки подлинности.
        /// </summary>
        /// <example>Pa$$w0rd</example>
        [Required]
        public string Password { get; set; }

        #endregion
    }
    #endregion
}
