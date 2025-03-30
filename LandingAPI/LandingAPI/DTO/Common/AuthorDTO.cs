namespace LandingAPI.DTO.Common
{
    #region Класс AuthorDTO
    /// <summary>
    /// Информация об авторе.
    /// Используется для передачи данных о пользователе, который создал или редактировал контент.
    /// </summary>
    public class AuthorDTO
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор пользователя.
        /// </summary>
        /// <example>1</example>
        public int UserId { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// Используется для отображения имени автора.
        /// </summary>
        /// <example>admin</example>
        public string UserName { get; set; }

        #endregion

        #region Примеры использования

        /// <summary>
        /// Пример создания экземпляра AuthorDTO.
        /// </summary>
        /// <example>
        /// var authorDto = new AuthorDTO
        /// {
        ///     UserId = 1,
        ///     UserName = "admin"
        /// };
        /// </example>
        #endregion
    }

    #endregion
}
