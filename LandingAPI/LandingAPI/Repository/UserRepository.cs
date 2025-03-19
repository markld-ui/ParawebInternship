using LandingAPI.Data;
using LandingAPI.Interfaces.Repositories;
using LandingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LandingAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<User>> GetUsersAsync()
        {
            return await _context.Users.OrderBy(u => u.UserId).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.Where(n => n.UserId == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByNameAsync(string username)
        {
            return await _context.Users.Where(n => n.Username == username).FirstOrDefaultAsync();
        }

        public async Task<ICollection<News>> GetNewsByUserIdAsync(int userId)
        {
            return await _context.News.Where(n => n.CreatedById == userId).ToListAsync();
        }
        public async Task<bool> UserExistsByIdAsync(int id)
        {
            return await _context.Users.AnyAsync(n => n.UserId == id);
        }

        public async Task<bool> UserExistsByNameAsync(string username)
        {
            return await _context.Users.AnyAsync(n => n.Username == username);
        }
    }
}