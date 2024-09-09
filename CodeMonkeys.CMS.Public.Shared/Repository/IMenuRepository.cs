using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;

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
    public class MenuRepository : IMenuRepository
    {
        public MenuRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public ApplicationDbContext Context { get; }

        public Task AddAsync(Menu menu) => throw new NotImplementedException();
        public Task AddMenuItemAsync(MenuItem menuItem) => throw new NotImplementedException();
        public Task<Menu?> GetMenuAsync(int menuId) => throw new NotImplementedException();
        public Task<IEnumerable<Menu>> GetMenusBySiteIdAsync(int siteId) => throw new NotImplementedException();
        public Task SaveChangesAsync() => throw new NotImplementedException();
    }
}