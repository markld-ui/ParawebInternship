using LandingAPI.Models;
using LandingAPI.Interfaces;
using LandingAPI.Data;

namespace LandingAPI.Repository
{
    public class EventRepository : IEventRepository
    {
        private DataContext _context;
        public EventRepository(DataContext context)
        {
            _context = context;
        }

        public bool EventExistsById(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }


        public Event GetEventById(int id)
        {
            return _context.Events.Where(e => e.EventId == id).FirstOrDefault();
        }

        public ICollection<Event> GetEvents()
        {
            return _context.Events.OrderBy(e => e.EventId).ToList();
        }

        public ICollection<Event> GetEventsByUserId(int userId)
        {
            return _context.Events.Where(e => e.CreatedById == userId).ToList();
        }
    }
}
