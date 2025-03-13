namespace LandingAPI.Models
{
    /// <summary>
    /// Data-class for file types
    /// </summary>
    public class FileType
    {
        public int FileTypeId { get; set; }
        public required string Name { get; set; }

        // Связь с файлами (One-to-Many: FileType -> Files)
        public ICollection<Files> Files { get; set; } = new List<Files>();
    }
}
