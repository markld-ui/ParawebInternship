using LandingAPI.Models;

namespace LandingAPI.Interfaces
{
    public interface IFilesRepository
    {
        ICollection<Files> GetFiles();
        ICollection<Files> GetFilesByType(string fileType);
        Files GetFileById(int id);
        bool FileExistsById(int id);
        bool FileExistsByName(string name);
    }
}
