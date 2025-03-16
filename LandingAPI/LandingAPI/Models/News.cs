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

        // Связь с пользователем (One-to-Many: User -> News)
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }

        // Связь с файлами (One-to-Many: News -> Files)
        public ICollection<Files> Files { get; set; } = new List<Files>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
