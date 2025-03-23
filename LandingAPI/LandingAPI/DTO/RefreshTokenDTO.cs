#region Заголовок файла

/// <summary>
/// Файл: RefreshTokenDTO.cs
/// Класс Data Transfer Object (DTO) для передачи данных о токене и refresh-токене.
/// Используется для обновления JWT-токена с помощью refresh-токена.
/// </summary>

#endregion

#region Пространства имен

// Здесь можно добавить используемые пространства имен, если они есть.
// Например:
// using LandingAPI.Models;

#endregion

namespace LandingAPI.DTO
{
    #region Класс RefreshTokenDTO

    /// <summary>
    /// Data Transfer Object (DTO) для передачи данных о токене и refresh-токене.
    /// </summary>
    public class RefreshTokenDTO
    {
        #region Свойства

        /// <summary>
        /// JWT-токен, который требуется обновить.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Refresh-токен, используемый для обновления JWT-токена.
        /// </summary>
        public string RefreshToken { get; set; }

        #endregion
    }

    #endregion
}