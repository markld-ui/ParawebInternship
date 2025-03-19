using LandingAPI.Data;
using LandingAPI.Interfaces;
using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LandingAPI.Repository
{
    public class NewsRepository : INewsRepository
    {
        private DataContext _context;
        public NewsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<News>> GetNewsAsync()
        {
            return await _context.News.OrderBy(n => n.NewsId).ToListAsync();
        }

        public async Task<News> GetNewsAsync(int newsId)
        {
            return await _context.News.Where(n => n.NewsId == newsId).FirstOrDefaultAsync();
        }

        public async Task<News> GetNewsByTitleAsync(string title)
        {
            return await _context.News.Where(n => n.Title == title).FirstOrDefaultAsync();
        }

        public async Task<bool> NewsExistsAsync(int newsId)
        {
            return await _context.News.AnyAsync(n => n.NewsId == newsId);
        }

        public async Task<bool> NewsExistsByTitleAsync(string title)
        {
            return await _context.News.AnyAsync(n => n.Title == title);
        }
    }
}
