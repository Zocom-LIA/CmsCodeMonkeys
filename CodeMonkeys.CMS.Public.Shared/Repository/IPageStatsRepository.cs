
using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IPageStatsRepository
    {
        Task<IEnumerable<PageStats>> GetPageStatsAsync(int siteId);
        Task<int> GetPageVisitsAsync(string pageUrl);
        Task UpdatePageCountAsync(int siteId, int pageId, string pageUrl);
    }
}