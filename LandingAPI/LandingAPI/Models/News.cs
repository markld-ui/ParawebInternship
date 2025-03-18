namespace LandingAPI.Models
{
    /// <summary>
    /// Data-class for news
    /// </summary>
    public class News
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }

        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }

        // Внешний ключ для связи с файлами
        public int? FileId { get; set; }
        public Files? File { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}