namespace LandingAPI.DTO.Files
{
    public class FileDetailsDTO
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public DateTime UploadedAt { get; set; }
        public string DownloadUrl { get; set; }
        public long FileSize { get; set; }
    }
}
