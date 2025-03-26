using System;

namespace LandingAPI.DTO.Events
{
    public class EventShortDTO
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; }
        public string AuthorName { get; set; }
        public bool HasAttachment { get; set; }
    }
}
