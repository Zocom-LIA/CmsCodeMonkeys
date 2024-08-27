using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Repository;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface IWebPageService
    {
        IWebPageRepository Repository { get; }

        Task CreateWebPageAsync(WebPage webPage);
        Task<WebPage?> GetSiteWebPageAsync(int siteId, int pageId);
        Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10);
        Task<WebPage?> GetSiteWebPageAsync(int siteId, int pageId);
        Task UpdateWebPageAsync(WebPage webPage);
        Task<IEnumerable<ContentDto>> GetWebPageContentsAsync(int pageId);
    }
}