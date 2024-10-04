using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeMonkeys.CMS.Public.Shared.Configuration
{
    public class WebPageConfiguration : IEntityTypeConfiguration<WebPage>
    {
        public void Configure(EntityTypeBuilder<WebPage> builder)
        {
            builder.Property(page => page.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(page => page.CreatedDate)
                .IsRequired();

            builder.Property(page => page.LastModifiedDate)
                .IsRequired();
            builder.HasKey(page => page.WebPageId);

            builder.Property(page => page.AuthorId)
                .IsRequired();

            //builder.Property(page => page.Description)
            //    .IsRequired()
            //    .HasMaxLength(1000);

            //builder.Property(page => page.IsPublished)
            //    .IsRequired();

            //builder.HasOne(page => page.Theme)
            //    .WithMany()
            //    .HasForeignKey(page => page.ThemeId);
        }
    }
}