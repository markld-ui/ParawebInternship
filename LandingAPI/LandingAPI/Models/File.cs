using LandingAPI.Models.Enums;

namespace LandingAPI.Models
{
    public class File
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public FileType Type { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
