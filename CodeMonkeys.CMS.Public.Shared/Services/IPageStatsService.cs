
using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface IPageStatsService
    {
        Task<int> GetPageVisitsAsync(string pageUrl);
        Task<IEnumerable<PageStats>> GetPageStatsAsync(int siteId);
    }
}