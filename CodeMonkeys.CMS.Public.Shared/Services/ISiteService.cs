using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface ISiteService
    {
        Task CreateSiteAsync(Site site);
        Task<IEnumerable<Site>> GetSitesAsync(Guid userId, int pageIndex = 0, int pageSize = 10);
        Task<Site?> GetSiteAsync(Guid userId, int siteId);
    }
}