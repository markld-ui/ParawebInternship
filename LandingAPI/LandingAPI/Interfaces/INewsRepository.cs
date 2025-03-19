using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LandingAPI.Interfaces
{
    public interface INewsRepository
    {
        Task<ICollection<News>> GetNewsAsync();
        Task<News> GetNewsAsync(int newsId);
        Task<News> GetNewsByTitleAsync(string title);
        Task<bool> NewsExistsAsync(int newsId);
        Task<bool> NewsExistsByTitleAsync(string title);
    }
}
