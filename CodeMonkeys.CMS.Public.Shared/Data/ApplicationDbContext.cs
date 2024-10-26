using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeMonkeys.CMS.Public.Shared.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>, IDisposable
    {
        private readonly ILogger _logger;
        private bool _isDisposed = false;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger) : base(options)
        {
        }

        public DbSet<PageStats> PageStats => Set<PageStats>();
        public DbSet<Site> Sites => Set<Site>();
        public DbSet<WebPage> Pages => Set<WebPage>();
        public DbSet<Content> Contents => Set<Content>();
        public DbSet<ContentItem> ContentItems => Set<ContentItem>();
        public DbSet<Section> Sections => Set<Section>();
        public DbSet<TextContent> TextContents => Set<TextContent>();
        public DbSet<ImageContent> ImageContent => Set<ImageContent>();
        public DbSet<VideoContent> VideoContent => Set<VideoContent>();
        public DbSet<LinkContent> LinkContent => Set<LinkContent>();
        public DbSet<CodeContent> CodeContent => Set<CodeContent>();
        public DbSet<FileContent> FileContent => Set<FileContent>();
        public DbSet<QuoteContent> QuoteContent => Set<QuoteContent>();
        public DbSet<Menu> Menus => Set<Menu>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configure the one-to-many relationship between Site and WebPage
            modelBuilder.Entity<Site>()
                .HasMany(s => s.Pages)
                .WithOne(p => p.Site)
                .HasForeignKey(p => p.SiteId);

            // Configure the one-to-one relationship between Site and LandingPage
            modelBuilder.Entity<Site>()
                .HasOne(s => s.LandingPage)
                .WithMany() // No navigation back from WebPage to Site for LandingPage
                .HasForeignKey(s => s.LandingPageId) // Foreign key in Site to WebPage
                .OnDelete(DeleteBehavior.NoAction); // Optionally set delete behavior to restrict or cascade

            modelBuilder.Entity<IdentityUserLogin<Guid>>().HasKey(x => new { x.LoginProvider, x.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserToken<Guid>>().HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}