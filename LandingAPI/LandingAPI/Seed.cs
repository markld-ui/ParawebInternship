using LandingAPI.Models;
using LandingAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace LandingAPI
{
    public class Seed
    {
        private readonly DataContext _dataContext;

        public Seed(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void SeedDataContext()
        {
            if (!_dataContext.Roles.Any())
            {
                var roles = new List<Role> 
                { 
                    new ()
                    {
                        Name = "Admin",
                        Users = new List<User>()
                    },
                    new ()
                    {
                        Name = "User",
                        Users = new List<User>()
                    },
                };
                _dataContext.Roles.AddRange(roles);
                _dataContext.SaveChanges();
                _dataContext.ChangeTracker.Clear();

            }

            var adminRoleId = _dataContext.Roles.FirstOrDefault(r => r.Name == "Admin")?.RoleId;
            if (adminRoleId == null)
                throw new Exception("Admin role was not created correctly.");

            var userRoleId = _dataContext.Roles.AsNoTracking().FirstOrDefault(r => r.Name == "User")?.RoleId;
            if (userRoleId == null)
                throw new Exception("User role was not created correctly.");

            if (!_dataContext.Users.Any())
            {
                var adminRole = _dataContext.Roles.FirstOrDefault(r => r.RoleId == adminRoleId.Value);
                if (adminRole == null)
                    throw new Exception("Admin role not found.");

                var userRole = _dataContext.Roles.FirstOrDefault(r => r.RoleId == userRoleId.Value);
                if (userRole == null)
                    throw new Exception("User role not found.");

                var users = new List<User>
                {
                    new ()
                    {
                        Username = "admin",
                        Email = "admin@example.com",
                        PasswordHash = "admin",
                        RoleId = adminRoleId.Value,
                        Role = adminRole,
                        News = new List<News>(),
                        CreatedEvents = new List<Event>(),
                        UserEvents = new List<UserEvent>()
                    },
                    new ()
                    {
                        Username = "user",
                        Email = "user@example.com",
                        PasswordHash = "user",
                        RoleId = userRoleId.Value,
                        Role = userRole,
                        News = new List<News>(),
                        CreatedEvents = new List<Event>(),
                        UserEvents = new List<UserEvent>()
                    }
                };
                _dataContext.Users.AddRange(users);
                _dataContext.SaveChanges();
            }

            if (!_dataContext.Events.Any())
            {
                var adminUser = _dataContext.Users.FirstOrDefault();
                if (adminUser == null)
                    throw new Exception("Admin user not found.");

                var events = new List<Event>
                {
                    new Event
                    {
                        CreatedById = adminUser.UserId,
                        CreatedBy = adminUser,
                        Title = "Test event",
                        Description = "Test event description",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddDays(1),
                        Location = "Tomsk, Lenina 40",
                        ImageUrl = "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png",
                        CreatedAt = DateTime.UtcNow,
                        Files = new List<Files>(),
                        UserEvents = new List<UserEvent>()
                    }
                };
                _dataContext.Events.AddRange(events);
                _dataContext.SaveChanges();
            }

            if (!_dataContext.FileTypes.Any())
            {
                var fileType = new FileType
                {
                    Name = "txt",
                    Files = new List<Files>()
                };
                _dataContext.FileTypes.Add(fileType);
                _dataContext.SaveChanges();
            }

            if (!_dataContext.News.Any())
            {
                var adminUser = _dataContext.Users.FirstOrDefault();
                if (adminUser == null)
                    throw new Exception("Admin user not found.");

                var news = new List<News>
                {
                    new News
                    {
                        Title = "Breaking News",
                        Content = "This is a breaking news content",
                        ImageUrl = "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png",
                        CreatedById = adminUser.UserId,
                        CreatedBy = adminUser,
                        CreatedAt = DateTime.UtcNow
                    },
                    new News
                    {
                        Title = "The New York Times",
                        Content = "This is The New York Times content",
                        ImageUrl = "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png",
                        CreatedById = adminUser.UserId,
                        CreatedBy = adminUser,
                        CreatedAt = DateTime.UtcNow
                    },
                    new News
                    {
                        Title = "Rambler",
                        Content = "This is Rambler content",
                        ImageUrl = "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png",
                        CreatedById = adminUser.UserId,
                        CreatedBy = adminUser,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                _dataContext.News.AddRange(news);
                _dataContext.SaveChanges();
            }

            var newsId = _dataContext.News.FirstOrDefault()?.NewsId;
            if (newsId == null)
                throw new Exception("News was not seeded properly.");

            if (!_dataContext.Files.Any())
            {
                var firstEvent = _dataContext.Events.FirstOrDefault();
                var fileTypeId = _dataContext.FileTypes.FirstOrDefault()?.FileTypeId;

                if (firstEvent == null)
                    throw new Exception("Event was not seeded properly.");
                if (fileTypeId == null)
                    throw new Exception("FileType was not seeded properly.");

                var files = new List<Files>
                {
                    new Files
                    {
                        FileTypeId = fileTypeId.Value,
                        EventId = firstEvent.EventId,
                        FileName = "Test file",
                        FilePath = "test/test.txt",
                        UploadedAt = DateTime.UtcNow,
                        NewsId = (int)newsId
                    }
                };
                _dataContext.Files.AddRange(files);
                _dataContext.SaveChanges();
            }
        }

    }
}
