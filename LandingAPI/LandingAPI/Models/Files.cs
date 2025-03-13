namespace LandingAPI.Models
{
    /// <summary>
    /// Data-class for files
    /// </summary>
    public class Files
    {
        public int FileId { get; set; }
        public required string FileName { get; set; }
        public required string FilePath { get; set; }

        // Связь с типом файла (Many-to-One: FileType -> Files)
        public int FileTypeId { get; set; }
        public FileType FileType { get; set; }

        // Связь с новостью (Many-to-One: News -> Files)
        public int NewsId { get; set; }
        public News? News { get; set; }

        // Связь с мероприятием (Many-to-One: Event -> Files)
        public int EventId { get; set; }
        public Event? Event { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
