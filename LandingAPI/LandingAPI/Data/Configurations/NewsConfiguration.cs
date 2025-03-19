using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LandingAPI.Data.Configurations
{
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.HasKey(n => n.NewsId);

            builder.Property(n => n.Title).IsRequired();
            builder.Property(n => n.Content).IsRequired();
            builder.Property(n => n.CreatedAt).IsRequired();

            builder.HasOne(n => n.CreatedBy)
                .WithMany(u => u.News)
                .HasForeignKey(n => n.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(n => n.File)
                   .WithMany()
                   .HasForeignKey(n => n.FileId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
