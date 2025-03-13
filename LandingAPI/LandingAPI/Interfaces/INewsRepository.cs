using LandingAPI.Models;

namespace LandingAPI.Interfaces
{
    public interface INewsRepository
    {
        ICollection<News> GetNews();
    }
}
