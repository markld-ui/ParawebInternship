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
    }
}
