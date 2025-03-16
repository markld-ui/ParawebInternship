using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LandingAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<EventFiles> EventFiles { get; set; }
        public DbSet<NewsFiles> NewsFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка первичных ключей
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Role>().HasKey(r => r.RoleId);
            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<News>().HasKey(n => n.NewsId);
            modelBuilder.Entity<Event>().HasKey(e => e.EventId);
            modelBuilder.Entity<Files>().HasKey(f => f.FileId);

            // Настройка промежуточных таблиц
            modelBuilder.Entity<EventFiles>().HasKey(ef => new { ef.EventId, ef.FileId });
            modelBuilder.Entity<NewsFiles>().HasKey(nf => new { nf.NewsId, nf.FileId });

            // Связи для промежуточных таблиц
            modelBuilder.Entity<EventFiles>()
                .HasOne(ef => ef.Event)
                .WithMany(e => e.EventFiles)
                .HasForeignKey(ef => ef.EventId);

            modelBuilder.Entity<EventFiles>()
                .HasOne(ef => ef.File)
                .WithMany(f => f.EventFiles)
                .HasForeignKey(ef => ef.FileId);

            modelBuilder.Entity<NewsFiles>()
                .HasOne(nf => nf.News)
                .WithMany(n => n.NewsFiles)
                .HasForeignKey(nf => nf.NewsId);

            modelBuilder.Entity<NewsFiles>()
                .HasOne(nf => nf.File)
                .WithMany(f => f.NewsFiles)
                .HasForeignKey(nf => nf.FileId);

            // Остальные связи
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<News>()
                .HasOne(n => n.CreatedBy)
                .WithMany(u => u.News)
                .HasForeignKey(n => n.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.CreatedBy)
                .WithMany(u => u.CreatedEvents)
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}