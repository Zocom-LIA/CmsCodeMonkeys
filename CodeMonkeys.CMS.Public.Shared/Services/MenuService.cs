using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Repository;

using Microsoft.EntityFrameworkCore;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repository;

        public MenuService(IMenuRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Menu> CreateMenuAsync(int siteId, string name)
        {
            var menu = new Menu { SiteId = siteId, Name = name };
            await _repository.AddAsync(menu);
            return menu;
        }

        public async Task AddMenuItemAsync(int webPageId, int order = 0)
        {
            var menuItem = new MenuItem { WebPageId = webPageId, Order = order };
            await _repository.AddMenuItemAsync(menuItem);
        }

        public async Task<Menu?> GetMenuAsync(int menuId)
        {
            var menu = await _repository.GetMenuAsync(menuId);
            return menu;
        }

        public async Task<IEnumerable<Menu>> GetMenusBySiteIdAsync(int siteId)
        {
            return await _repository.GetMenusBySiteIdAsync(siteId);
        }

        public async Task UpdateMenuAsync(Menu menu)
        {
            await _repository.UpdateMenuAsync(menu);
        }

        public async Task DeleteMenuAsync(int menuId)
        {
            await _repository.DeleteMenuAsync(menuId);
        }

        public async Task DeleteMenuItemAsync(int menuItemId)
        {
            await _repository.DeleteMenuItemAsync(menuItemId);
        }
    }
}
