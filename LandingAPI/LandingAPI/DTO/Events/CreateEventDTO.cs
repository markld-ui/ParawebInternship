using System;
using System.ComponentModel.DataAnnotations;

namespace LandingAPI.DTO.Events
{
    public class CreateEventDTO
    {
        [Required(ErrorMessage = "Заголовок обязателен")]
        [StringLength(100, ErrorMessage = "Заголовок не должен превышать 100 символов")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Описание обязательно")]
        [StringLength(5000, ErrorMessage = "Описание не должно превышать 5000 символов")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Дата начала обязательна")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Дата окончания обязательна")]
        public DateTime EndDate { get; set; }

        public string? Location { get; set; }
        public int? FileId { get; set; }
        public IFormFile? File { get; set; }
    }
}
