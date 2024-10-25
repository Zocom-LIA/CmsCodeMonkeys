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
        Task DeleteMenuItemAsync(int menuItemId);
        Task DeleteMenuAsync(int menuItemId);
        Task<Menu?> GetMenuAsync(int menuId);
        Task<IEnumerable<Menu>> GetMenusBySiteIdAsync(int siteId);
        Task UpdateMenuAsync(Menu menu);
    }
}