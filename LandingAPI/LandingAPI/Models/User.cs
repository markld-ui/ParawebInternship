namespace LandingAPI.Models
{
    /// <summary>
    /// General user data-class
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }

        // Связь с ролью (One-to-Many: Role -> Users)
        public int RoleId { get; set; }
        public required Role Role { get; set; }

        // Связь с новостями (One-to-Many: User -> News)
        public ICollection<News> News { get; set; } = new List<News>();

        // Связь с мероприятиями (One-to-Many: User -> Events)
        public ICollection<Event> CreatedEvents { get; set; } = new List<Event>();

        // Связь с логами администратора (One-to-Many: User -> AdminLogs)
        public ICollection<AdminLog> AdminLogs { get; set; } = new List<AdminLog>();

        // Связь с участием в мероприятиях (Many-to-Many: User ↔ Event через UserEvent)
        public ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
