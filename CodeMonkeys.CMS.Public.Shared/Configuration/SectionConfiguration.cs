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
            builder.Property(section => section.Name)
                .IsRequired()
                .HasMaxLength(100);

            //builder.Property(section => section.Description)
            //    .IsRequired()
            //    .HasMaxLength(1000);

            //builder.Property(section => section.OrdinalNumber)
            //    .IsRequired();

            //builder.Property(section => section.IsVisible)
            //    .IsRequired();

            builder.HasOne(section => section.WebPage)
                .WithMany(page => page.Sections)
                .HasForeignKey(section => section.WebPageId);
        }
    }
}