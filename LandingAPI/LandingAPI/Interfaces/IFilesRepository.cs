using LandingAPI.Models;

namespace LandingAPI.Interfaces
{
    public interface IFilesRepository
    {
        Task<ICollection<Files>> GetFilesAsync();
        Task<Files> GetFileByIdAsync(int id);
        Task<bool> FileExistsByIdAsync(int id);
        Task<bool> FileExistsByNameAsync(string name);

        // Новые методы для получения файлов, связанных с новостями и событиями
        Task<Files> GetFileByNewsIdAsync(int newsId);
        Task<Files> GetFileByEventIdAsync(int eventId);
    }
}