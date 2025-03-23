#region Заголовок файла

/// <summary>
/// Файл: JwtService.cs
/// Сервис для генерации JWT-токенов.
/// Используется для создания токенов аутентификации на основе данных пользователя.
/// </summary>

#endregion

#region Пространства имен

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using LandingAPI.Models;
using Microsoft.Extensions.Configuration;

#endregion

namespace LandingAPI.Services.Auth
{
    #region Класс JwtService

    /// <summary>
    /// Сервис для генерации JWT-токенов.
    /// </summary>
    public class JwtService
    {
        #region Поля

        private readonly IConfiguration _configuration;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="JwtService"/>.
        /// </summary>
        /// <param name="configuration">Конфигурация приложения, содержащая настройки JWT.</param>
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Генерирует JWT-токен для пользователя.
        /// </summary>
        /// <param name="user">Пользователь, для которого создается токен.</param>
        /// <returns>Строка, представляющая JWT-токен.</returns>
        public string GenerateToken(User user)
        {
            // Получение настроек JWT из конфигурации
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

            // Описание токена
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            // Создание и запись токена
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion
    }

    #endregion
}