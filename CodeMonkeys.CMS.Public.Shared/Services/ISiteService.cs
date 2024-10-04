using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface ISiteService
    {
        Task<Site> CreateSiteAsync(Site site, CancellationToken cancellation = default);
        Task UpdateSiteAsync(Site site, CancellationToken cancellation = default);
        Task DeleteSiteAsync(Site site, CancellationToken cancellation = default);

        Task<IEnumerable<Site>> GetUserSitesAsync(Guid userId, int pageIndex = 0, int pageSize = 10, CancellationToken cancellation = default);
        Task<Site?> GetUserSiteAsync(Guid userId, int siteId, CancellationToken cancellation = default);
        Task<Site?> GetSiteAsync(int siteId, bool includeLandingPage = false, bool includePages = false, bool includeSections = false, bool includeContents = false, bool includeAuthor = false, CancellationToken cancellation = default);
    }
}