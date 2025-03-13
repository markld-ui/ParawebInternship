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
        public DbSet<FileType> FileTypes { get; set; }
        public DbSet<AdminLog> AdminLogs { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                 .HasKey(u => u.UserId);
            modelBuilder.Entity<User>()
                .Property(u => u.UserId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .HasPrincipalKey(r => r.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<News>()
                .HasKey(n => n.NewsId);
            modelBuilder.Entity<News>()
                .Property(n => n.NewsId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<News>()
                .HasOne(e => e.CreatedBy)
                .WithMany(u => u.News)
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<News>()
                .HasOne(n => n.CreatedBy)
                .WithMany(u => u.News)
                .HasForeignKey(n => n.CreatedById)
                .HasPrincipalKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
                .HasKey(e => e.EventId);
            modelBuilder.Entity<Event>()
                .Property(e => e.EventId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Event>()
                .HasOne(e => e.CreatedBy)
                .WithMany(e => e.CreatedEvents)
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Event>()
                .HasOne(e => e.CreatedBy)
                .WithMany(u => u.CreatedEvents)
                .HasForeignKey(e => e.CreatedById)
                .HasPrincipalKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Files>()
                .HasKey(f => f.FileId);
            modelBuilder.Entity<Files>()
                .HasOne(f => f.FileType)
                .WithMany(ft => ft.Files)
                .HasForeignKey(f => f.FileTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Files>()
                .HasOne(f => f.News)
                .WithMany(n => n.Files)
                .HasForeignKey(f => f.NewsId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Files>()
                .HasOne(f => f.Event)
                .WithMany(e => e.Files)
                .HasForeignKey(f => f.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AdminLog>()
                .HasOne(al => al.Admin)
                .WithMany()
                .HasForeignKey(al => al.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserEvent>()
                .HasKey(ue => new { ue.UserId, ue.EventId });
            modelBuilder.Entity<UserEvent>()
                .HasOne(ue => ue.User)
                .WithMany(u => u.UserEvents)
                .HasForeignKey(ue => ue.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserEvent>()
                .HasOne(ue => ue.Event)
                .WithMany(e => e.UserEvents)
                .HasForeignKey(ue => ue.EventId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
