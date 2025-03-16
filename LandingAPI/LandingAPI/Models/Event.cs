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

        // Связь с пользователем (One-to-Many: User -> Events)
        public int CreatedById { get; set; } //FK
        public User CreatedBy { get; set; } //navigation prop

        // Связь с файлами (One-to-Many: Event -> Files)
        public ICollection<Files> Files { get; set; } = new List<Files>();
        public  DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
