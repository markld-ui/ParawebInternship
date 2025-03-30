#region Пространство имен

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using LandingAPI.Services.Validations;

#endregion

namespace LandingAPI.DTO.Files
{
    #region Класс FileUploadDTO
    /// <summary>
    /// Data Transfer Object (DTO) для загрузки файла.
    /// </summary>
    public class FileUploadDTO
    {
        #region Свойства
        /// <summary>
        /// Файл, который необходимо загрузить.
        /// Это обязательное поле.
        /// </summary>
        [Required(ErrorMessage = "Файл обязателен")]
        [MaxFileSize(10240 * 1024)] // 10 Mb
        public IFormFile File { get; set; }
        #endregion
    }
    #endregion
}
