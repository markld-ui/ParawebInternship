using LandingAPI.Models;

namespace LandingAPI.Interfaces
{
    public interface IFilesRepository
    {
        ICollection<Files> GetFiles();
        Files GetFileById(int id);
        bool FileExistsById(int id);
        bool FileExistsByName(string name);

        // Новые методы для получения файлов, связанных с новостями и событиями
        ICollection<Files> GetFilesByNewsId(int newsId);
        ICollection<Files> GetFilesByEventId(int eventId);
    }
}
