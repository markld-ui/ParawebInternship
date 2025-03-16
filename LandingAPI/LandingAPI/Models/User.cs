namespace LandingAPI.Models
{
    /// <summary>
    /// General user data-class
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // Связь с ролью (Many-to-Many: Role <-> Users) -> (One-to-Many; Many-to-One)
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // Связь с новостями (One-to-Many: User -> News)
        public ICollection<News> News { get; set; } = new List<News>();

        // Связь с мероприятиями (One-to-Many: User -> Events)
        public ICollection<Event> CreatedEvents { get; set; } = new List<Event>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
