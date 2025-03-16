namespace LandingAPI.Models
{
    /// <summary>
    /// Data-class for files
    /// </summary>
    public class Files
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        // Связь с новостью (Many-to-One: News -> Files)
        public int NewsId { get; set; }
        public News? News { get; set; } = null;

        // Связь с мероприятием (Many-to-One: Event -> Files)
        public int EventId { get; set; }
        public Event? Event { get; set; } = null;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
