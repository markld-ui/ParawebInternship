using LandingAPI.Models;

namespace LandingAPI.DTO
{
    public class EventDTO
    {
        public int EventId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required string? Location { get; set; }
        public required string? ImageUrl { get; set; }
    }
}
