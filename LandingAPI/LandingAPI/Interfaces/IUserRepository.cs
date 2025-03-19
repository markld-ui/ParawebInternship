using LandingAPI.Models;

namespace LandingAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByNameAsync(string username);
        Task<ICollection<News>> GetNewsByUserIdAsync(int userId);
        Task<bool> UserExistsByIdAsync(int id);
        Task<bool> UserExistsByNameAsync(string username);
    }
}
