#region Заголовок файла

/// <summary>
/// Файл: PasswordHasher.cs
/// Сервис для хеширования и проверки паролей с использованием алгоритма BCrypt.
/// Реализует интерфейс <see cref="IPasswordHasher"/>.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Interfaces.Auth;

#endregion

namespace LandingAPI.Services.Auth
{
    #region Класс PasswordHasher

    /// <summary>
    /// Сервис для хеширования и проверки паролей.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        #region Методы

        /// <summary>
        /// Генерирует хеш пароля с использованием алгоритма BCrypt.
        /// </summary>
        /// <param name="password">Пароль, который необходимо хешировать.</param>
        /// <returns>Хешированный пароль.</returns>
        public string Generate(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        /// <summary>
        /// Проверяет соответствие пароля его хешу с использованием алгоритма BCrypt.
        /// </summary>
        /// <param name="password">Пароль для проверки.</param>
        /// <param name="hashedPassword">Хешированный пароль для сравнения.</param>
        /// <returns>
        /// <c>true</c>, если пароль соответствует хешу, иначе <c>false</c>.
        /// </returns>
        public bool Verify(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);

        #endregion
    }

    #endregion
}