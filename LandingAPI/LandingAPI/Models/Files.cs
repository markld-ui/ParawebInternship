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
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}