#region Пространство имен

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

#endregion

namespace LandingAPI.DTO.Events
{
    #region Класс CreateEventDTO
    public class CreateEventDTO
    {
        #region Свойства
        /// <summary>
        /// Заголовок события. Обязательное поле, не должно превышать 100 символов.
        /// </summary>
        [Required(ErrorMessage = "Заголовок обязателен")]
        [StringLength(100, ErrorMessage = "Заголовок не должен превышать 100 символов")]
        /// <example>Конференция по разработке ПО</example>
        public string Title { get; set; }

        /// <summary>
        /// Описание события. Обязательное поле, не должно превышать 5000 символов.
        /// </summary>
        [Required(ErrorMessage = "Описание обязательно")]
        [StringLength(5000, ErrorMessage = "Описание не должно превышать 5000 символов")]
        /// <example>Это годовая конференция, посвященная последним достижениям в области разработки программного обеспечения.</example>
        public string Description { get; set; }

        /// <summary>
        /// Дата начала события. Обязательное поле.
        /// </summary>
        [Required(ErrorMessage = "Дата начала обязательна")]
        /// <example>2023-10-01T10:00:00</example>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания события. Обязательное поле.
        /// </summary>
        [Required(ErrorMessage = "Дата окончания обязательна")]
        /// <example>2023-10-01T18:00:00</example>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Место проведения события. Необязательное поле.
        /// </summary>
        /// <example>Городской конференц-центр</example>
        public string? Location { get; set; }

        /// <summary>
        /// ID файла, связанного с событием. Необязательное поле.
        /// </summary>
        /// <example>42</example>
        public int? FileId { get; set; }

        /// <summary>
        /// Файл, связанный с событием. Необязательное поле.
        /// </summary>
        public IFormFile? File { get; set; }

        #endregion 
    }
    #endregion
}
