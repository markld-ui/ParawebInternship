namespace LandingAPI.Models
{
    /// <summary>
    /// Data-class for news
    /// </summary>
    public class News
    {
        public int NewsId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string? ImageUrl { get; set; }

        // Связь с пользователем (One-to-Many: User -> News)
        public int CreatedById { get; set; }
        public required User CreatedBy { get; set; }

        // Связь с файлами (One-to-Many: News -> Files)
        public ICollection<Files> Files { get; set; } = new List<Files>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
