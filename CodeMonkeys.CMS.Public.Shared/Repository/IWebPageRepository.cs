using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IWebPageRepository
    {
        ApplicationDbContext Context { get; }

        Task CreateWebPageAsync(WebPage webPage);
        Task<WebPage?> GetSiteWebPageAsync(int siteId, int pageId);
        Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10);
        Task<IEnumerable<WebPageDto>> GetVisitorWebPageAsync(int? pageId);
        Task<WebPage?> GetWebPageAsync(int webPageId);
        Task<IEnumerable<ContentDto>> GetWebPageContentsAsync(int pageId, bool sortContent = false);
        Task UpdateWebPageAsync(WebPage webPage);
        Task<IEnumerable<Content>> UpdateWebPageContentsAsync(WebPage webPage, IEnumerable<Content> contents);
    }
}