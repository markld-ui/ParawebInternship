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

        // Связи через промежуточные таблицы
        public ICollection<EventFiles> EventFiles { get; set; } = new List<EventFiles>();
        public ICollection<NewsFiles> NewsFiles { get; set; } = new List<NewsFiles>();
    }
}
