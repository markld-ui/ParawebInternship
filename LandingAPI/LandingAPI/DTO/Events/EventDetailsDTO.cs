using LandingAPI.DTO.News;
using LandingAPI.DTO.Common;
using System;

namespace LandingAPI.DTO.Events
{
    public class EventDetailsDTO
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public AuthorDTO CreatedBy { get; set; }
        public FileInfoDTO? File { get; set; }
    }
}
