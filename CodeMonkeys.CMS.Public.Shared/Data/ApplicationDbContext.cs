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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger) : base(options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public DbSet<PageStats> PageStats => Set<PageStats>();
        public DbSet<Site> Sites => Set<Site>();
        public DbSet<WebPage> Pages => Set<WebPage>();
        public DbSet<Content> Contents => Set<Content>();
        public DbSet<Menu> Menus => Set<Menu>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    }
}