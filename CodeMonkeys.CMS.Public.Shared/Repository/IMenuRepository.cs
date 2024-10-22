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
    }
    public class MenuRepository :RepositoryBase, IMenuRepository
    {
        public MenuRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IMapper mapper, ILogger<MenuRepository> logger) : base(contextFactory, mapper, logger)
        {
        }


        public async Task AddAsync(Menu menu)
        {
            using (ApplicationDbContext context = GetContext())
            {
                await context.Menus.AddAsync(menu);
                await context.SaveChangesAsync();
            }
        }
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
    }
}