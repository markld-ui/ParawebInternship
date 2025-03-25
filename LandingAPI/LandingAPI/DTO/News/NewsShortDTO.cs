namespace LandingAPI.DTO.News
{
    public class NewsShortDTO
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorName { get; set; }
        public bool HasAttachment { get; set; }
    }
}
