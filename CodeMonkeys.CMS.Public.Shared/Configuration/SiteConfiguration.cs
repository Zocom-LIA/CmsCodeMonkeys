//using CodeMonkeys.CMS.Public.Shared.Entities;

//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace CodeMonkeys.CMS.Public.Shared.Configuration
//{
//    public class SiteConfiguration : IEntityTypeConfiguration<Site>
//    {
//        public void Configure(EntityTypeBuilder<Site> builder)
//        {
//            builder.Property(site => site.Name)
//                .IsRequired()
//                .HasMaxLength(100);

//            builder.HasOne(site => site.Theme)
//                .WithMany()
//                .HasForeignKey(site => site.ThemeId);

//            builder.HasOne(site => site.Creator)
//                .WithMany()
//                .HasForeignKey(site => site.CreatorId);

//            builder.HasOne(site => site.LandingPage)
//                .WithMany()
//                .HasForeignKey(site => site.LandingPageId);

//            builder.HasMany(site => site.Pages)
//                .WithOne(page => page.Site)
//                .HasForeignKey(page => page.SiteId);

//            builder.HasMany(site => site.Menus)
//                .WithOne(menu => menu.Site)
//                .HasForeignKey(menu => menu.SiteId);

//            builder.Property(site => site.CreatedDate)
//                .IsRequired()
//                .HasDefaultValueSql("GETUTCDATE()");

//            builder.Property(site => site.LastModifiedDate)
//                .IsRequired()
//                .HasDefaultValueSql("GETUTCDATE()");

//            builder.Property(site => site.Logo)
//                .HasMaxLength(100);
//            builder.Property(site => site.Name)
//                .HasMaxLength(100);

//            builder.Property(site => site.Favicon)
//                .HasMaxLength(100);

//            builder.HasIndex(site => site.Name)
//                .IsUnique();
//        }
//    }
//}
