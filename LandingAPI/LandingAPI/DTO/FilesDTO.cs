using LandingAPI.Models;

namespace LandingAPI.DTO
{
    public class FilesDTO
    {
        public int FileId { get; set; }
        public required string FileName { get; set; }
        public required string FilePath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Новые свойства для отображения связанных новостей и событий
        public ICollection<int> NewsIds { get; set; } = new List<int>();
        public ICollection<int> EventIds { get; set; } = new List<int>();
    }
}
