using System;

namespace LandingAPI.DTO.News
{
    public class NewsDetailsDTO
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public AuthorDTO CreatedBy { get; set; }
        public FileInfoDTO? File { get; set; }
    }

    public class AuthorDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class FileInfoDTO
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string DownloadUrl { get; set; }
    }
}