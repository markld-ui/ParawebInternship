using System;
using System.ComponentModel.DataAnnotations;

namespace LandingAPI.DTO.Events
{
    public class UpdateEventDTO
    {
        [StringLength(100, ErrorMessage = "Заголовок не должен превышать 100 символов")]
        public string? Title { get; set; }

        [StringLength(5000, ErrorMessage = "Описание не должно превышать 5000 символов")]
        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Location { get; set; }
        public int? FileId { get; set; }
        public bool? RemoveFile { get; set; }
    }
}
