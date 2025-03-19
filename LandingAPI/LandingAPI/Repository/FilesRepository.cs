using LandingAPI.Models;
using LandingAPI.Data;
using Microsoft.EntityFrameworkCore;
using LandingAPI.Interfaces.Repositories;

namespace LandingAPI.Repository
{
    public class FilesRepository : IFilesRepository
    {
        private DataContext _context;
        public FilesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> FileExistsByIdAsync(int id)
        {
            return await _context.Files.AnyAsync(f => f.FileId == id);
        }

        public async Task<bool> FileExistsByNameAsync(string name)
        {
            return await _context.Files.AnyAsync(f => f.FileName == name);
        }

        public async Task<Files> GetFileByIdAsync(int id)
        {
            return await _context.Files.Where(f => f.FileId == id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Files>> GetFilesAsync()
        {
            return await _context.Files.OrderBy(f => f.FileId).ToListAsync();
        }

        // Новый метод для получения файла, связанного с новостью
        public async Task<Files> GetFileByNewsIdAsync(int newsId)
        {
            return await _context.News
                .Where(n => n.NewsId == newsId)
                .Select(n => n.File)
                .FirstOrDefaultAsync();
        }

        // Новый метод для получения файла, связанного с событием
        public async Task<Files> GetFileByEventIdAsync(int eventId)
        {
            return await _context.Events
                .Where(e => e.EventId == eventId)
                .Select(e => e.File)
                .FirstOrDefaultAsync();
        }
    }
}