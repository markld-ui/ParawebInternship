#region Пространства имен

using System.Collections.Generic;

#endregion

namespace LandingAPI.Helper
{
    #region Класс PagedResponse
    /// <summary>
    /// Объект ответа с пагинацией, представляющий данные и информацию о пагинации.
    /// </summary>
    /// <typeparam name="T">Тип данных, который будет содержаться в ответе.</typeparam>
    public class PagedResponse<T>
    {
        #region Свойства
        /// <summary>
        /// Список данных текущей страницы.
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// Общее количество элементов во всех страницах.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Номер текущей страницы.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Размер страницы (количество элементов на странице).
        /// </summary>
        public int PageSize { get; set; }
        #endregion
    }
    #endregion
}
