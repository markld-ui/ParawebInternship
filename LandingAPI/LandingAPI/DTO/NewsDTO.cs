using LandingAPI.Models;

namespace LandingAPI.DTO
{
    public class NewsDTO
    {
        public int NewsId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string? ImageUrl { get; set; }
    }
}
