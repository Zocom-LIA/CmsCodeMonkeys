using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface IMenuService
    {
        Task AddMenuItemAsync(int webPageId, int order = 0);
        Task<Menu> CreateMenuAsync(int siteId, string name);
        Task<Menu?> GetMenuAsync(int menuId);
        Task<IEnumerable<Menu>> GetMenusBySiteIdAsync(int siteId);
        Task UpdateMenuAsync(Menu menu);
        Task DeleteMenuAsync(int menuId);
        Task DeleteMenuItemAsync(int menuItemId);
    }
}