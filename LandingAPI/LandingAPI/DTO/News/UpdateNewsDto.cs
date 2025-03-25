﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LandingAPI.DTO.News
{
    public class UpdateNewsDTO
    {
        [Required(ErrorMessage = "Заголовок обязателен")]
        [StringLength(100, ErrorMessage = "Заголовок не должен превышать 100 символов")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Содержание обязательно")]
        [StringLength(5000, ErrorMessage = "Содержание не должно превышать 5000 символов")]
        public string Content { get; set; }

        public IFormFile? NewFile { get; set; }
        public bool? RemoveFile { get; set; }
    }
}