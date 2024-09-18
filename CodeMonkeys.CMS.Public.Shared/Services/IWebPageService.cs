using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Repository;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface IWebPageService
    {
        Task CreateWebPageAsync(int siteId, WebPage webPage);
        Task<IEnumerable<Content>> CreateWebPageContentAsync(WebPage webPage, Content content);
        Task DeleteWebPageAsync(WebPage page);
        Task<WebPage?> GetSiteWebPageAsync(int siteId, int pageId);
        Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10);
        Task<IEnumerable<WebPageDto>> GetVisitorPageAsync(int? pageId = null);
        Task<WebPage?> GetWebPageAsync(int webPageId);
        Task<IEnumerable<ContentDto>> GetWebPageContentsAsync(int pageId);
        Task<IEnumerable<Content>> MoveContentDownAsync(WebPage page, int ordinalNumber, CancellationToken cancellation = default);
        Task<IEnumerable<Content>> MoveContentUpAsync(WebPage page, int ordinalNumber, CancellationToken cancellation = default);
        Task<IEnumerable<Content>> UpdateOrdinalNumbersAsync(WebPage webPage, bool persist = true, CancellationToken cancellation = default);
        Task UpdateWebPageAsync(WebPage webPage);
        Task<IEnumerable<Content>> UpdateWebPageContentsAsync(WebPage webPage, IEnumerable<Content> contents);
    }
}