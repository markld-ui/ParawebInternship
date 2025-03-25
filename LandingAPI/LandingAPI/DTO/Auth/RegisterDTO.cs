#region Заголовок файла

/// <summary>
/// Файл: RegisterDTO.cs
/// Класс Data Transfer Object (DTO) для передачи данных, необходимых для регистрации пользователя.
/// Используется для передачи данных о регистрации (имя пользователя, email и пароль) между слоями приложения.
/// </summary>

#endregion

#region Пространства имен

using System.ComponentModel.DataAnnotations;

#endregion

namespace LandingAPI.DTO.Auth
{
    #region Класс RegisterDTO

    /// <summary>
    /// Data Transfer Object (DTO) для передачи данных, необходимых для регистрации пользователя.
    /// </summary>
    public class RegisterDTO
    {
        #region Свойства

        /// <summary>
        /// Имя пользователя. Используется для идентификации пользователя в системе.
        /// </summary>
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [MinLength(3, ErrorMessage = "Имя пользователя должно содержать минимум 3 символа")]
        [MaxLength(20, ErrorMessage = "Имя пользователя не должно превышать 20 символов")]
        public string Username { get; set; }

        /// <summary>
        /// Электронная почта пользователя. Используется для идентификации и связи с пользователем.
        /// </summary> 
        [Required(ErrorMessage = "Почта обязательна")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя. Используется для аутентификации.
        /// </summary>
        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(8, ErrorMessage = "Пароль должен содержать минимум 8 символов")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
                ErrorMessage = "Пароль должен содержать цифры, заглавные и строчные буквы")]
        public string Password { get; set; }

        #endregion
    }

    #endregion
}