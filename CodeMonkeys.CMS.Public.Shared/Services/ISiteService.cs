using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface ISiteService
    {
        Task<Site> CreateSiteAsync(Site site);
        Task UpdateSiteAsync(Site site);
        Task DeleteSiteAsync(Site site);

        Task<IEnumerable<Site>> GetUserSitesAsync(Guid userId, int pageIndex = 0, int pageSize = 10);
        Task<Site?> GetUserSiteAsync(Guid userId, int siteId);
        Task<Site?> GetSiteAsync(int siteId);
    }
}