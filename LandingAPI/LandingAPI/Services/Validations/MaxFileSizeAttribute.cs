#region Пространство имен

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

#endregion

namespace LandingAPI.Services.Validations
{
    #region Класс MaxFileSizeAttribute
    /// <summary>
    /// Атрибут валидации для ограничения максимального размера загружаемого файла.
    /// Наследует класс <see cref="ValidationAttribute"/> и переопределяет метод <see cref="IsValid(object, ValidationContext)"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        #region Поля 

        /// <summary>
        /// Максимальный размер файла в байтах.
        /// </summary>
        private readonly int _maxFileSize;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MaxFileSizeAttribute"/> с указанным максимальным размером файла.
        /// </summary>
        /// <param name="maxFileSize">Максимальный размер файла в байтах.</param>
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        #endregion

        #region Методы

        #region IsValid
        /// <summary>
        /// Проверяет, является ли значение допустимым, исходя из максимального размера файла.
        /// </summary>
        /// <param name="value">Значение, которое нужно проверить (должно быть типом <see cref="IFormFile"/>).</param>
        /// <param name="validationContext">Контекст валидации, содержащий информацию о текущем объекте.</param>
        /// <returns>
        /// <see cref="ValidationResult"/>. Возвращает <see cref="ValidationResult.Success"/>, если размер файла допустим,
        /// или сообщение об ошибке, если файл превышает максимальный размер.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null && file.Length > _maxFileSize)
            {
                return new ValidationResult($"Размер файла не должен превышать {_maxFileSize / 1024} КБ.");
            }

            return ValidationResult.Success;
        }

        #endregion

        #endregion
    }
    #endregion
}
