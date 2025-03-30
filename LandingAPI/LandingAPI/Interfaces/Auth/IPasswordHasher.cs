#region Заголовок файла

/// <summary>
/// Файл: IPasswordHasher.cs
/// Интерфейс для хеширования и проверки паролей.
/// Определяет методы для генерации хеша пароля и проверки его соответствия.
/// </summary>

#endregion

namespace LandingAPI.Interfaces.Auth
{
    #region Интерфейс IPasswordHasher

    /// <summary>
    /// Интерфейс для хеширования и проверки паролей.
    /// </summary>
    public interface IPasswordHasher
    {
        #region Методы

        /// <summary>
        /// Генерирует хеш пароля.
        /// </summary>
        /// <param name="password">Пароль, который необходимо хешировать.</param>
        /// <returns>Хешированный пароль в виде строки.</returns>
        string Generate(string password);

        /// <summary>
        /// Проверяет соответствие пароля его хешу.
        /// </summary>
        /// <param name="password">Пароль для проверки.</param>
        /// <param name="hashedPassword">Хешированный пароль для сравнения.</param>
        /// <returns>
        /// Возвращает <c>true</c>, если пароль соответствует хешу, иначе <c>false</c>.
        /// </returns>
        bool Verify(string password, string hashedPassword);

        #endregion
    }

    #endregion
}