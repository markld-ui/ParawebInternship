using LandingAPI.Data;
using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LandingAPI.Services.Seeding
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

            // Seed Files if they don't exist
            if (!_dataContext.Files.Any())
            {
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
            }

            // Seed News if they don't exist
            if (!_dataContext.News.Any())
            {
                var adminUser = _dataContext.Users.FirstOrDefault(u => u.Username == "admin");
                var editorUser = _dataContext.Users.FirstOrDefault(u => u.Username == "editor");
                var file1 = _dataContext.Files.FirstOrDefault(f => f.FileName == "file1.txt");

                if (adminUser == null || editorUser == null || file1 == null)
                {
                    throw new Exception("Required users or files are missing for seeding news.");
                }

                var news = new List<News>
                {
                    new News
                    {
                        Title = "News 1",
                        Content = "Content for News 1",
                        CreatedById = adminUser.UserId,
                        CreatedBy = adminUser,
                        FileId = file1.FileId,
                        File = file1
                    },
                    new News
                    {
                        Title = "News 2",
                        Content = "Content for News 2",
                        CreatedById = editorUser.UserId,
                        CreatedBy = editorUser,
                        FileId = null // Пример без привязки к файлу
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
                var file2 = _dataContext.Files.FirstOrDefault(f => f.FileName == "file2.txt");

                if (adminUser == null || user1 == null || file2 == null)
                {
                    throw new Exception("Required users or files are missing for seeding events.");
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
                        CreatedById = adminUser.UserId,
                        CreatedBy = adminUser,
                        FileId = file2.FileId,
                        File = file2
                    },
                    new Event
                    {
                        Title = "Event 2",
                        Description = "Description for Event 2",
                        StartDate = DateTime.UtcNow.AddDays(2),
                        EndDate = DateTime.UtcNow.AddDays(3),
                        Location = "Location 2",
                        CreatedById = user1.UserId,
                        CreatedBy = user1,
                        FileId = null // Пример без привязки к файлу
                    }
                };

                _dataContext.Events.AddRange(events);
                _dataContext.SaveChanges();
            }
        }
    }
}