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
    }
}
