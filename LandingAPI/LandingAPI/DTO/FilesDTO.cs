using LandingAPI.Models;

namespace LandingAPI.DTO
{
    public class FilesDTO
    {
        public int FileId { get; set; }
        public required string FileName { get; set; }
        public required string FilePath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
