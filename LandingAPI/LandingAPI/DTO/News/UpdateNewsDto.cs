#region Пространство имен

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

#endregion

namespace LandingAPI.DTO.News
{
    #region Класс UpdateNewsDTO
    /// <summary>
    /// Data Transfer Object (DTO) для обновления информации о новостной записи.
    /// </summary>
    public class UpdateNewsDTO
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
        /// Новый файл для прикрепления к новости. Может быть null, если файл не загружается.
        /// </summary>
        public IFormFile? NewFile { get; set; }

        /// <summary>
        /// Указывает, нужно ли удалить существующий файл. Может быть null.
        /// </summary>
        public bool? RemoveFile { get; set; }
        #endregion
    }
    #endregion
}