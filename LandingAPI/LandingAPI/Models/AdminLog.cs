namespace LandingAPI.Models
{
    /// <summary>
    /// Data-class for admin logging
    /// </summary>
    public class AdminLog
    {
        public required int AdminLogId { get; set; }

        // Связь с пользователем-администратором (One-to-Many: User -> AdminLogs)
        public required int AdminId { get; set; }
        public required User Admin { get; set; }

        public required string Action { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
