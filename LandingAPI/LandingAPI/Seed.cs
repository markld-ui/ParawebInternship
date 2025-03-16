using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LandingAPI.Data
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
            // Ensure the database is created
            _dataContext.Database.EnsureCreated();

            // Seed Roles if they don't exist
            if (!_dataContext.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "User" },
                    new Role { Name = "Editor" }
                };
                _dataContext.Roles.AddRange(roles);
                _dataContext.SaveChanges();
            }

            // Seed Users if they don't exist
            if (!_dataContext.Users.Any())
            {
                var adminRole = _dataContext.Roles.FirstOrDefault(r => r.Name == "Admin");
                var userRole = _dataContext.Roles.FirstOrDefault(r => r.Name == "User");
                var editorRole = _dataContext.Roles.FirstOrDefault(r => r.Name == "Editor");

                if (adminRole == null || userRole == null || editorRole == null)
                {
                    throw new Exception("Required roles are missing for seeding users.");
                }

                var users = new List<User>
                {
                    new User
                    {
                        Username = "admin",
                        Email = "admin@example.com",
                        PasswordHash = "admin",
                        UserRoles = new List<UserRole>
                        {
                            new UserRole { Role = adminRole }
                        }
                    },
                    new User
                    {
                        Username = "user1",
                        Email = "user1@example.com",
                        PasswordHash = "user1",
                        UserRoles = new List<UserRole>
                        {
                            new UserRole { Role = userRole }
                        }
                    },
                    new User
                    {
                        Username = "user2",
                        Email = "user2@example.com",
                        PasswordHash = "user2",
                        UserRoles = new List<UserRole>
                        {
                            new UserRole { Role = userRole }
                        }
                    },
                    new User
                    {
                        Username = "editor",
                        Email = "editor@example.com",
                        PasswordHash = "editor",
                        UserRoles = new List<UserRole>
                        {
                            new UserRole { Role = editorRole }
                        }
                    }
                };
                _dataContext.Users.AddRange(users);
                _dataContext.SaveChanges();
            }

            // Seed News if they don't exist
            if (!_dataContext.News.Any())
            {
                var adminUser = _dataContext.Users.FirstOrDefault(u => u.Username == "admin");
                var editorUser = _dataContext.Users.FirstOrDefault(u => u.Username == "editor");

                if (adminUser == null || editorUser == null)
                {
                    throw new Exception("Required users are missing for seeding news.");
                }

                var news = new List<News>
                {
                    new News
                    {
                        Title = "News 1",
                        Content = "Content for News 1",
                        ImageUrl = "https://example.com/news1.png",
                        CreatedById = adminUser.UserId,
                        CreatedBy = adminUser
                    },
                    new News
                    {
                        Title = "News 2",
                        Content = "Content for News 2",
                        ImageUrl = "https://example.com/news2.png",
                        CreatedById = editorUser.UserId,
                        CreatedBy = editorUser
                    }
                };
                _dataContext.News.AddRange(news);
                _dataContext.SaveChanges();
            }

            // Seed Events if they don't exist
            if (!_dataContext.Events.Any())
            {
                var adminUser = _dataContext.Users.FirstOrDefault(u => u.Username == "admin");
                var user1 = _dataContext.Users.FirstOrDefault(u => u.Username == "user1");

                if (adminUser == null || user1 == null)
                {
                    throw new Exception("Required users are missing for seeding events.");
                }

                var events = new List<Event>
                {
                    new Event
                    {
                        Title = "Event 1",
                        Description = "Description for Event 1",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddDays(1),
                        Location = "Location 1",
                        ImageUrl = "https://example.com/image1.png",
                        CreatedById = adminUser.UserId,
                        CreatedBy = adminUser
                    },
                    new Event
                    {
                        Title = "Event 2",
                        Description = "Description for Event 2",
                        StartDate = DateTime.UtcNow.AddDays(2),
                        EndDate = DateTime.UtcNow.AddDays(3),
                        Location = "Location 2",
                        ImageUrl = "https://example.com/image2.png",
                        CreatedById = user1.UserId,
                        CreatedBy = user1
                    }
                };
                _dataContext.Events.AddRange(events);
                _dataContext.SaveChanges();
            }

            // Seed Files if they don't exist
            if (!_dataContext.Files.Any())
            {
                var news1 = _dataContext.News.FirstOrDefault(n => n.Title == "News 1");
                var event1 = _dataContext.Events.FirstOrDefault(e => e.Title == "Event 1");

                if (news1 == null || event1 == null)
                {
                    throw new Exception("Required news or events are missing for seeding files.");
                }

                var files = new List<Files>
                {
                    new Files
                    {
                        FileName = "file1.txt",
                        FilePath = "/path/to/file1.txt",
                        UploadedAt = DateTime.UtcNow
                    },
                    new Files
                    {
                        FileName = "file2.txt",
                        FilePath = "/path/to/file2.txt",
                        UploadedAt = DateTime.UtcNow
                    }
                };

                _dataContext.Files.AddRange(files);
                _dataContext.SaveChanges();

                // Seed NewsFiles and EventFiles
                var file1 = files[0];
                var file2 = files[1];

                var newsFiles = new List<NewsFiles>
                {
                    new NewsFiles
                    {
                        NewsId = news1.NewsId,
                        FileId = file1.FileId
                    }
                };

                var eventFiles = new List<EventFiles>
                {
                    new EventFiles
                    {
                        EventId = event1.EventId,
                        FileId = file2.FileId
                    }
                };

                _dataContext.NewsFiles.AddRange(newsFiles);
                _dataContext.EventFiles.AddRange(eventFiles);
                _dataContext.SaveChanges();
            }
        }
    }
}