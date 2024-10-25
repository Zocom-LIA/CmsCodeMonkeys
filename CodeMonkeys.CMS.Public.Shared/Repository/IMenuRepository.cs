using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IMenuRepository
    {
        Task AddAsync(Menu menu);
        Task AddMenuItemAsync(MenuItem menuItem);
        Task DeleteMenuAsync(int menuId);
        Task<Menu?> GetMenuAsync(int menuId);
        Task<IEnumerable<Menu>> GetMenusBySiteIdAsync(int siteId);
        Task UpdateMenuAsync(Menu menu);
    }
}