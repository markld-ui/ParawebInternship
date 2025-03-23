#region Заголовок файла

/// <summary>
/// Файл: LoginDTO.cs
/// Класс Data Transfer Object (DTO) для передачи данных, необходимых для аутентификации пользователя.
/// Используется для передачи данных о входе в систему (email и пароль) между слоями приложения.
/// </summary>

#endregion

namespace LandingAPI.DTO
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
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя. Используется для проверки подлинности.
        /// </summary>
        public string Password { get; set; }

        #endregion
    }

    #endregion
}