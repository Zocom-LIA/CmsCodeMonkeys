using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeMonkeys.CMS.Public.Shared.Configuration
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            // Configure primary key
            builder.HasKey(s => s.SectionId);

            // Configure other properties (e.g., required, maxlength, navigation properties)
            builder.Property(s => s.Name).IsRequired().HasMaxLength(50);
            builder.HasOne(s => s.WebPage).WithMany(wp => wp.Sections).HasForeignKey(s => s.WebPageId);
            builder.HasMany(s => s.ContentItems).WithOne(ci => ci.Section).HasForeignKey(ci => ci.SectionId);
        }
    }
}