using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Repository;
using CodeMonkeys.CMS.Public.Shared.Services;

public class SiteService : ISiteService
{
    public SiteService(ISiteRepository repository, CancellationToken cancellation = default)
    {
        Repository = repository;
    }

    public ISiteRepository Repository { get; }

    public async Task<Site> CreateSiteAsync(Site site, CancellationToken cancellation = default)
    {
        return await Repository.CreateAsync(site);
    }

    public async Task UpdateSiteAsync(Site site, CancellationToken cancellation = default)
    {
        await Repository.UpdateSiteAsync(site);
    }

    public async Task DeleteSiteAsync(Site site, CancellationToken cancellation = default)
    {
        await Repository.DeleteSiteAsync(site);
    }

    public async Task<IEnumerable<Site>> GetUserSitesAsync(Guid userId, int pageIndex = 0, int pageSize = 10, CancellationToken cancellation = default)
    {
        return await Repository.GetUserSitesAsync(userId, pageIndex, pageSize);
    }

    public async Task<Site?> GetUserSiteAsync(Guid userId, int siteId, CancellationToken cancellation = default)
    {
        return await Repository.GetUserSiteAsync(userId, siteId);
    }

    // Used for testing purposes only
    public async Task<Site?> GetSiteAsync(int siteId, bool includeLandingPage = false, bool includePages = false, bool includeSections = false, bool includeContents = false, bool includeAuthor = false, CancellationToken cancellation = default)
    {
        return await Repository.GetSiteAsync(siteId, includeLandingPage, includePages, includeSections, includeContents, includeAuthor, cancellation);
    }
}