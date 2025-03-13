namespace LandingAPI.Models
{
    /// <summary>
    /// Data-class for User participation in Event
    /// </summary>
    public class UserEvent
    {
        public int UserId { get; set; }
        public required User User { get; set; }

        public int EventId { get; set; }
        public required Event Event { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
