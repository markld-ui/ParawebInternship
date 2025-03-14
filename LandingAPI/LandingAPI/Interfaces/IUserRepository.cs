using LandingAPI.Models;

namespace LandingAPI.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUserById(int id);
        User GetUserByName(string username);
        ICollection<News> GetNewsByUserId(int userId);
        bool UserExistsById(int id);
        bool UserExistsByName(string username);
    }
}
