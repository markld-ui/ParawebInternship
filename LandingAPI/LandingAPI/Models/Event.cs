namespace LandingAPI.Models
{
    /// <summary>
    /// Data-class for events
    /// </summary>
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; }

        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }

        // Внешний ключ для связи с файлами
        public int? FileId { get; set; }
        public Files? File { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}