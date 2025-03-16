namespace LandingAPI.Models
{
    public class NewsFiles
    {
        public int NewsId { get; set; }
        public News News { get; set; }

        public int FileId { get; set; }
        public Files File { get; set; }
    }
}
