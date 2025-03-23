#region Заголовок файла

/// <summary>
/// Файл: RegisterDTO.cs
/// Класс Data Transfer Object (DTO) для передачи данных, необходимых для регистрации пользователя.
/// Используется для передачи данных о регистрации (имя пользователя, email и пароль) между слоями приложения.
/// </summary>

#endregion

namespace LandingAPI.DTO
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
        public string Username { get; set; }

        /// <summary>
        /// Электронная почта пользователя. Используется для идентификации и связи с пользователем.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя. Используется для аутентификации.
        /// </summary>
        public string Password { get; set; }

        #endregion
    }

    #endregion
}