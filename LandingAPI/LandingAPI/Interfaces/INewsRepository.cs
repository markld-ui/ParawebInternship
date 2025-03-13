using LandingAPI.Models;

namespace LandingAPI.Interfaces
{
    public interface INewsRepository
    {
        ICollection<News> GetNews();
        News GetNews(int newsId);
        News GetNewsByTitle(string title);
        bool NewsExists(int newsId);
        bool NewsExistsByTitle(string title);
    }
}
