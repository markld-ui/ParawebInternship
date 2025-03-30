#region Заголовок файла

/// <summary>
/// Файл: UserService.cs
/// Сервис для работы с пользователями.
/// Предоставляет методы для регистрации пользователей и других операций, связанных с учетными записями.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Interfaces.Auth;
using System.Threading.Tasks;

#endregion

namespace LandingAPI.Services
{
    #region Класс UserService

    /// <summary>
    /// Сервис для работы с пользователями.
    /// </summary>
    public class UserService
    {
        #region Поля

        private readonly IPasswordHasher _passwordHasher;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserService"/>.
        /// </summary>
        /// <param name="passwordHasher">Сервис для хеширования паролей.</param>
        public UserService(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        #endregion

        #region Методы

        #region Register
        /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="email">Электронная почта пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        public async Task Register(string userName, string email, string password)
        {
            var hashedPassword = _passwordHasher.Generate(password);

        }
        #endregion

        #endregion
    }

    #endregion
}