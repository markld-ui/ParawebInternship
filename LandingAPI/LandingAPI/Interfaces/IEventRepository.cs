using LandingAPI.Models;
using System.Threading.Tasks;

namespace LandingAPI.Interfaces
{
    public interface IEventRepository
    {
        Task<ICollection<Event>> GetEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task<ICollection<Event>> GetEventsByUserIdAsync(int userId);
        Task<bool> EventExistsByIdAsync(int id);

    }
}
