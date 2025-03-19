using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LandingAPI.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.EventId);

            builder.Property(e => e.Title).IsRequired();
            builder.Property(e => e.Description).IsRequired();
            builder.Property(e => e.StartDate).IsRequired();
            builder.Property(e => e.EndDate).IsRequired();
            builder.Property(e => e.Location).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasOne(e => e.CreatedBy)
                   .WithMany(u => u.CreatedEvents)
                   .HasForeignKey(e => e.CreatedById)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.File)
                .WithMany()
                .HasForeignKey(e => e.FileId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
