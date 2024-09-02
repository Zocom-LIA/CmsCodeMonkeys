using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface ISiteService
    {
        Task CreateSiteAsync(Site site);
        Task UpdateSiteAsync(Site site);

        Task<IEnumerable<Site>> GetUserSitesAsync(Guid userId, int pageIndex = 0, int pageSize = 10);
        Task<Site?> GetUserSiteAsync(Guid userId, int siteId);
    }
}