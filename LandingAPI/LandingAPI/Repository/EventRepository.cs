using LandingAPI.Models;
using LandingAPI.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LandingAPI.Interfaces.Repositories;

namespace LandingAPI.Repository
{
    public class EventRepository : IEventRepository
    {
        private DataContext _context;
        public EventRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> EventExistsByIdAsync(int id)
        {
            return await _context.Events.AnyAsync(e => e.EventId == id);
        }


        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _context.Events.FirstOrDefaultAsync(e => e.EventId == id);
        }

        public async Task<ICollection<Event>> GetEventsAsync()
        {
            return await _context.Events.OrderBy(e => e.EventId).ToListAsync();
        }

        public async Task<ICollection<Event>> GetEventsByUserIdAsync(int userId)
        {
            return await _context.Events.Where(e => e.CreatedById == userId).ToListAsync();
        }
    }
}
