using LandingAPI.Models;

namespace LandingAPI.Interfaces
{
    public interface IEventRepository
    {
        ICollection<Event> GetEvents();
        Event GetEventById(int id);
        ICollection<Event> GetEventsByUserId(int userId);
        bool EventExistsById(int id);

    }
}
