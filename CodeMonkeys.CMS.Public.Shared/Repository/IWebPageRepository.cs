using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;

public interface IWebPageRepository
{
    Task CreateWebPageAsync(WebPage webPage);
    Task<WebPage?> GetSiteWebPageAsync(int siteId, int pageId);
    Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10);
    Task<IEnumerable<ContentDto>> GetWebPageContentsAsync(int pageId);
    Task UpdateWebPageAsync(WebPage webPage);
}