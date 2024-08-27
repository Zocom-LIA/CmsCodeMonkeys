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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Site>().HasMany<WebPage>(s => s.Pages).WithOne().HasForeignKey(s => s.SiteId);
        }
    }
}