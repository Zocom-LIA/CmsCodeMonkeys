using CodeMonkeys.CMS.Public.Shared.Entities;

public interface IWebPageRepository
{
    Task CreateWebPageAsync(WebPage webPage);
    Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10);
}