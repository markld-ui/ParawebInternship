namespace LandingAPI.Models
{
    public class EventFiles
    {
        public int EventId { get; set; }
        public Event Event { get; set; }

        public int FileId { get; set; }
        public Files File { get; set; }
    }
}
