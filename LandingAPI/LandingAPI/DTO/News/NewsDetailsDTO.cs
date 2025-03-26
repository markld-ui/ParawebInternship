using System;
using LandingAPI.DTO.Common;

namespace LandingAPI.DTO.News
{
    public class NewsDetailsDTO
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public AuthorDTO CreatedBy { get; set; }
        public FileInfoDTO? File { get; set; }
    }
}