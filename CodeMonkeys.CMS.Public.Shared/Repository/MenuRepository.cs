using AutoMapper;
using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class MenuRepository : RepositoryBase, IMenuRepository
    {
        public MenuRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IMapper mapper, ILogger<MenuRepository> logger) : base(contextFactory, mapper, logger)
        {
        }


        public async Task AddAsync(Menu menu)
        {
            using (ApplicationDbContext context = GetContext())
            {
                try
                {
                    await context.Menus.AddAsync(menu);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to add a menu");
                    throw new RepositoryException("Failed to add a menu", ex);
                }
            }
        }
        public Task AddMenuItemAsync(MenuItem menuItem) => throw new NotImplementedException();

        public async Task DeleteMenuAsync(int menuId)
        {
            using (ApplicationDbContext context = GetContext())
            {
                try
                {
                    Menu? menu = await context.Menus.FindAsync(menuId);
                    if (menu == null) throw new InvalidOperationException($"No menu with number{menuId} found.");
                    context.Menus.Remove(menu);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to delete a menu");
                    throw new RepositoryException("Failed to delete a menu", ex);
                }
            }
        }

        public async Task DeleteMenuItemAsync(int menuItemId)
        {
            using (ApplicationDbContext context = GetContext())
            {
                try
                {
                    MenuItem? menuItem = await context.MenuItems.FindAsync(menuItemId);
                    if (menuItem == null) throw new InvalidOperationException($"No menu item with number{menuItemId} found.");
                    context.MenuItems.Remove(menuItem);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to delete a menu item ");
                    throw new RepositoryException("Failed to delete a menu item", ex);
                }
            }
        }

        public Task<Menu?> GetMenuAsync(int menuId) => throw new NotImplementedException();
        public async Task<IEnumerable<Menu>> GetMenusBySiteIdAsync(int siteId)
        {
            using (ApplicationDbContext context = GetContext())
            {
                try
                {
                    return await context.Menus.AsNoTracking()
                                          .Where(menu => menu.SiteId == siteId)
                                          .Include(menu => menu.Items)
                                          .ThenInclude(item => item.WebPage)
                                          .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error when retrieving menus");
                    throw new RepositoryException("Error when retrieving menus", ex);
                }
            }
        }

        public async Task UpdateMenuAsync(Menu menu)
        {
            using (ApplicationDbContext context = GetContext())
            {
                try
                {
                    context.Menus.Update(menu);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update a menu");
                    throw new RepositoryException("Failed to update a menu", ex);
                }
            }
        }
    }
}