using CodeMonkeys.CMS.Public.Shared.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CodeMonkeys.CMS.Public.Shared.Configuration
{
    public class PageStatsConfiguration : IEntityTypeConfiguration<PageStats>
    {
        public void Configure(EntityTypeBuilder<PageStats> builder)
        {
            builder.HasKey(ps => ps.PageStatsId);
            builder.Property(ps => ps.PageStatsId).ValueGeneratedOnAdd();

            builder.HasIndex(ps => ps.PageUrl).IsUnique();
        }
    }

}
