using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeMonkeys.CMS.Public.Shared.Configuration
{
    public class ContentItemConfiguration : IEntityTypeConfiguration<ContentItem>
    {
        public void Configure(EntityTypeBuilder<ContentItem> builder)
        {
            builder.HasKey(ci => ci.ContentId);
            builder.Property(ci => ci.ContentType).IsRequired();
            builder.HasOne(ci => ci.Section).WithMany(s => s.ContentItems).HasForeignKey(ci => ci.SectionId);
        }
    }
}