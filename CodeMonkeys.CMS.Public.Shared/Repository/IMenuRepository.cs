using AutoMapper;
using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IMenuRepository
    {
        Task AddAsync(Menu menu);
        Task AddMenuItemAsync(MenuItem menuItem);
        Task<Menu?> GetMenuAsync(int menuId);
        Task<IEnumerable<Menu>> GetMenusBySiteIdAsync(int siteId);
        Task SaveChangesAsync();
    }
    public class MenuRepository :RepositoryBase, IMenuRepository
    {
        public MenuRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IMapper mapper, ILogger<MenuRepository> logger) : base(contextFactory, mapper, logger)
        {
        }


        public Task AddAsync(Menu menu) => throw new NotImplementedException();
        public Task AddMenuItemAsync(MenuItem menuItem) => throw new NotImplementedException();
        public Task<Menu?> GetMenuAsync(int menuId) => throw new NotImplementedException();
        public async Task<IEnumerable<Menu>> GetMenusBySiteIdAsync(int siteId)
        {
            using (ApplicationDbContext context = GetContext())
            {
                return await context.Menus.AsNoTracking()
                                          .Where(menu => menu.SiteId == siteId)
                                          .Include(menu => menu.Items)
                                          .ThenInclude(item => item.WebPage)
                                          .ToListAsync();
            }
        }
        public Task SaveChangesAsync() => throw new NotImplementedException();
    }
}