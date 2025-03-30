#region Пространство имен

using LandingAPI.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

#endregion

namespace LandingAPI.DTO.News
{
    #region Класс CreateNewsDTO
    /// <summary>
    /// Data Transfer Object (DTO) для создания новостной записи.
    /// Используется для передачи данных о заголовке, содержании и файле новости.
    /// </summary>
    public class CreateNewsDTO
    {
        #region Свойства
        /// <summary>
        /// Заголовок новости. Обязательное поле, не должно превышать 100 символов.
        /// </summary>
        [Required(ErrorMessage = "Заголовок обязателен")]
        [StringLength(100, ErrorMessage = "Заголовок не должен превышать 100 символов")]
        public string Title { get; set; }

        /// <summary>
        /// Содержание новости. Обязательное поле, не должно превышать 5000 символов.
        /// </summary>
        [Required(ErrorMessage = "Содержание обязательно")]
        [StringLength(5000, ErrorMessage = "Содержание не должно превышать 5000 символов")]
        public string Content { get; set; }

        /// <summary>
        /// Файл, связанный с новостью (например, изображение). Необязательное поле.
        /// </summary>
        public IFormFile? File { get; set; }

        /// <summary>
        /// ID файла, связанного с новостью. Необязательное поле.
        /// </summary>
        /// <example>42</example>
        public int? FileId { get; set; }
        #endregion
    }
    #endregion
}
