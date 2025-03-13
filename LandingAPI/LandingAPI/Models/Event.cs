namespace LandingAPI.Models
{
    /// <summary>
    /// Data-class for events
    /// </summary>
    public class Event
    {
        public int EventId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required string? Location { get; set; }
        public required string? ImageUrl { get; set; }

        // Связь с пользователем (One-to-Many: User -> Events)
        public int CreatedById { get; set; }
        public required User CreatedBy { get; set; }

        // Связь с файлами (One-to-Many: Event -> Files)
        public ICollection<Files> Files { get; set; } = new List<Files>();

        // Связь Many-to-Many с пользователями (участники мероприятия через UserEvent)
        public ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();

        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
