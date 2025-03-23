using LandingAPI.Models;

namespace LandingAPI.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByNameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<ICollection<News>> GetNewsByUserIdAsync(int userId);
        Task<bool> UserExistsByIdAsync(int id);
        Task<bool> UserExistsByNameAsync(string username);
        Task<bool> UserExistsByEmailAsync(string email);
        Task AddUserAsync(User user);
    }
}
