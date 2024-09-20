using CodeMonkeys.CMS.Public.Shared.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CodeMonkeys.CMS.Public.Shared.Configuration
{
    public class ContentConfiguration : IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
        {
            builder.HasKey(c => c.ContentId);
            builder.Property(c => c.ContentId).ValueGeneratedOnAdd();

            builder.HasOne(c => c.WebPage)
                .WithMany(wp => wp.Contents)
                .HasForeignKey(c => c.WebPageId)
                .IsRequired(false); // Allow null web page

            builder.HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .IsRequired(false); // Allow null author

            builder.Property(c => c.CreatedDate).HasDefaultValueSql("GETDATE()");
            builder.Property(c => c.LastModifiedDate).HasDefaultValueSql("GETDATE()");
        }
    }

    public class SiteConfiguration : IEntityTypeConfiguration<Site>
    {
        public void Configure(EntityTypeBuilder<Site> builder)
        {
            builder.HasKey(s => s.SiteId);
            builder.Property(s => s.SiteId).ValueGeneratedOnAdd();

            builder.HasMany(s => s.Pages)
                .WithOne(p => p.Site)
                .HasForeignKey(p => p.SiteId);

            builder.HasOne(s => s.LandingPage)
                .WithMany() // No inverse navigation property
                .HasForeignKey(s => s.LandingPageId)
                .IsRequired(false); // Allow null landing page

            builder.HasMany(s => s.Pages)
                .WithOne(p => p.Site)
                .HasForeignKey(p => p.SiteId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion of Site if Pages exist

            builder.HasMany(s => s.Menus)
                .WithOne(m => m.Site)
                .HasForeignKey(m => m.SiteId)
                .IsRequired();

            builder.Property(c => c.CreatedDate).HasDefaultValueSql("GETDATE()");
            builder.Property(c => c.LastModifiedDate).HasDefaultValueSql("GETDATE()");
        }
    }

    public class WebPageConfiguration : IEntityTypeConfiguration<WebPage>
    {
        public void Configure(EntityTypeBuilder<WebPage> builder)
        {
            builder.HasKey(wp => wp.WebPageId);
            builder.Property(wp => wp.WebPageId).ValueGeneratedOnAdd();

            builder.HasOne(wp => wp.Site)
                .WithMany(s => s.Pages)
                .HasForeignKey(wp => wp.SiteId)
                .IsRequired();

            builder.HasOne(wp => wp.Author)
                .WithMany(u => u.Pages)
                .HasForeignKey(wp => wp.AuthorId)
                .IsRequired(false); // Allow null author

            builder.Property(c => c.CreatedDate).HasDefaultValueSql("GETDATE()");
            builder.Property(c => c.LastModifiedDate).HasDefaultValueSql("GETDATE()");
        }
    }
}