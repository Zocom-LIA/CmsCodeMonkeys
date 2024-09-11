using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Repository;
using CodeMonkeys.CMS.Public.Shared.Services;

public class SiteService : ISiteService
{
    public SiteService(ISiteRepository repository)
    {
        Repository = repository;
    }

    public ISiteRepository Repository { get; }

    public async Task CreateSiteAsync(Site site)
    {
        await Repository.CreateAsync(site);
    }

    public async Task UpdateSiteAsync(Site site)
    {
        await Repository.UpdateSiteAsync(site);
    }

    public async Task DeleteSiteAsync(Site site)
    {
        await Repository.DeleteSiteAsync(site);
    }

    public async Task<IEnumerable<Site>> GetUserSitesAsync(Guid userId, int pageIndex = 0, int pageSize = 10)
    {
        return await Repository.GetUserSitesAsync(userId, pageIndex, pageSize);
    }

    public async Task<Site?> GetUserSiteAsync(Guid userId, int siteId)
    {
        return await Repository.GetUserSiteAsync(userId, siteId);
    }

    // Used for testing purposes only
    public async Task<Site?> GetSiteAsync(int siteId)
    {
        return await Repository.GetSiteAsync(siteId);
    }
}