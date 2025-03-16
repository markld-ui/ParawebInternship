using LandingAPI.Models;

namespace LandingAPI.Interfaces
{
    public interface IFilesRepository
    {
        ICollection<Files> GetFiles();
        Files GetFileById(int id);
        bool FileExistsById(int id);
        bool FileExistsByName(string name);
    }
}
