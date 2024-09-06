using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeMonkeys.CMS.Public.Shared.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User, Role, Guid>(options)
    {
        public DbSet<PageStats> PageStats => Set<PageStats>();
        public DbSet<Site> Sites => Set<Site>();
        public DbSet<WebPage> Pages => Set<WebPage>();
        public DbSet<Content> Contents => Set<Content>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // Ensure Identity configurations are applied

            // Configure the one-to-many relationship between Site and WebPage
            modelBuilder.Entity<Site>()
                .HasMany(s => s.Pages)
                .WithOne(p => p.Site)
                .HasForeignKey(p => p.SiteId);

            // Configure the one-to-one relationship between Site and LandingPage
            modelBuilder.Entity<Site>()
                .HasOne(s => s.LandingPage)
                .WithMany()  // No navigation back from WebPage to Site for LandingPage
                .HasForeignKey(s => s.LandingPageId)  // Foreign key in Site to WebPage
                .OnDelete(DeleteBehavior.NoAction); // Optionally set delete behavior to restrict or cascade
        }
    }
}