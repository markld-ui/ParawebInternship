using LandingAPI.Data;
using LandingAPI.Interfaces;
using LandingAPI.Models;

namespace LandingAPI.Repository
{
    public class NewsRepository : INewsRepository
    {
        private DataContext _context;
        public NewsRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<News> GetNews()
        {
            return _context.News.OrderBy(n => n.NewsId).ToList();
        }

        public News GetNews(int newsId)
        {
            return _context.News.Where(n => n.NewsId == newsId).FirstOrDefault();
        }

        public News GetNewsByTitle(string title)
        {
            return _context.News.Where(n => n.Title == title).FirstOrDefault();
        }

        public bool NewsExists(int newsId)
        {
            return _context.News.Any(n => n.NewsId == newsId);
        }

        public bool NewsExistsByTitle(string title)
        {
            return _context.News.Any(n => n.Title == title);
        }
    }
}
