using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LandingAPI.DTO.Files
{
    public class FileUploadDTO
    {
        [Required(ErrorMessage = "Файл обязателен")]
        public IFormFile File { get; set; }
    }
}
