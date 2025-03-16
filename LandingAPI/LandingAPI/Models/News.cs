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

        // Связь с файлами через промежуточную таблицу
        public ICollection<NewsFiles> NewsFiles { get; set; } = new List<NewsFiles>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
