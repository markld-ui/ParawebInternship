using LandingAPI.Data;
using LandingAPI.Interfaces;
using LandingAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LandingAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderBy(u => u.UserId).ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.Where(n => n.UserId == id).FirstOrDefault();
        }

        public User GetUserByName(string username)
        {
            return _context.Users.Where(n => n.Username == username).FirstOrDefault();
        }

        public ICollection<News> GetNewsByUserId(int userId)
        {
            return _context.News.Where(n => n.CreatedById == userId).ToList();
        }
        public bool UserExistsById(int id)
        {
            return _context.Users.Any(n => n.UserId == id);
        }

        public bool UserExistsByName(string username)
        {
            return _context.Users.Any(n => n.Username == username);
        }
    }
}
