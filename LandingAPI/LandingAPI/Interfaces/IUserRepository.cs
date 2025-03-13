using LandingAPI.Models;

namespace LandingAPI.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
    }
}
