using LandingAPI.Models;
using LandingAPI.Interfaces;
using LandingAPI.Data;

namespace LandingAPI.Repository
{
    public class FilesRepository : IFilesRepository
    {
        private DataContext _context;
        public FilesRepository(DataContext context)
        {
            _context = context;
        }
        public bool FileExistsById(int id)
        {
            return _context.Files.Any(f => f.FileId == id);
        }

        public bool FileExistsByName(string name)
        {
            return _context.Files.Any(f => f.FileName == name);
        }

        public Files GetFileById(int id)
        {
            return _context.Files.Where(f => f.FileId == id).FirstOrDefault();
        }

        public ICollection<Files> GetFiles()
        {
            return _context.Files.OrderBy(f => f.FileId).ToList();
        }

        // Новый метод для получения файлов, связанных с новостью
        public ICollection<Files> GetFilesByNewsId(int newsId)
        {
            return _context.NewsFiles
                .Where(nf => nf.NewsId == newsId)
                .Select(nf => nf.File)
                .ToList();
        }

        // Новый метод для получения файлов, связанных с событием
        public ICollection<Files> GetFilesByEventId(int eventId)
        {
            return _context.EventFiles
                .Where(ef => ef.EventId == eventId)
                .Select(ef => ef.File)
                .ToList();
        }
    }
}
