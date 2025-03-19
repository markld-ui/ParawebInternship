using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LandingAPI.Data.Configurations
{
    public class FilesConfiguration : IEntityTypeConfiguration<Files>
    {
        public void Configure(EntityTypeBuilder<Files> builder)
        {
            builder.HasKey(f => f.FileId);

            builder.Property(f => f.FileName).IsRequired();
            builder.Property(f => f.FilePath).IsRequired();
            builder.Property(f => f.UploadedAt).IsRequired();
        }
    }
}
