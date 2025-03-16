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
        public string? ImageUrl { get; set; }

        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }

        // Связь с файлами через промежуточную таблицу
        public ICollection<EventFiles> EventFiles { get; set; } = new List<EventFiles>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
