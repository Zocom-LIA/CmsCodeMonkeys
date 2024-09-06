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
        Task<IEnumerable<WebPageDto>> GetVisitorPageAsync(int? pageId = null);
        Task<WebPage?> GetWebPageAsync(int webPageId);
        Task<IEnumerable<ContentDto>> GetWebPageContentsAsync(int pageId);
        Task<IEnumerable<Content>> MoveContentDownAsync(WebPage page, int ordinalNumber);
        Task<IEnumerable<Content>> MoveContentUpAsync(WebPage page, int ordinalNumber);
        Task<IEnumerable<Content>> UpdateOrdinalNumbersAsync(WebPage webPage, bool persist = true);
        Task UpdateWebPageAsync(WebPage webPage);
        Task<IEnumerable<Content>> UpdateWebPageContentsAsync(WebPage webPage, IEnumerable<Content> contents);
    }
}