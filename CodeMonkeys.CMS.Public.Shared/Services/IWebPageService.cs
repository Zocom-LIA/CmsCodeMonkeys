using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Repository;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface IWebPageService
    {
        IWebPageRepository Repository { get; }

        Task<WebPage> CreateWebPageAsync(int siteId, WebPage webPage, CancellationToken cancellation = default);
        Task<IEnumerable<Content>> CreateWebPageContentAsync(WebPage webPage, Content content, CancellationToken cancellation = default);
        Task DeleteWebPageAsync(WebPage page, CancellationToken cancellation = default);
        Task<WebPage?> GetSiteWebPageAsync(int siteId, int pageId, bool includeContents = false, bool includeAuthor = false, bool includeSite = false, CancellationToken cancellation = default);
        Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10, CancellationToken cancellation = default);
        Task<IEnumerable<WebPageDto>> GetVisitorPageAsync(int? pageId = null, CancellationToken cancellation = default);
        Task<WebPage?> GetVisitorWebPageAsync(int webPageId, bool includeSite = false, bool includeAuthor = false, bool includeSections = false, bool includeContents = false, CancellationToken cancellation = default);
        Task<WebPageIncludeDto?> GetWebPageAsync(int webPageId, bool includeSite = false, bool includeAuthor = false, bool includeSections = false, bool includeContents = false, CancellationToken cancellation = default);
        Task<IEnumerable<Content>> MoveContentDownAsync(WebPage page, int ordinalNumber, CancellationToken cancellation = default);
        Task<IEnumerable<Content>> MoveContentUpAsync(WebPage page, int ordinalNumber, CancellationToken cancellation = default);
        Task<IEnumerable<Content>> UpdateOrdinalNumbersAsync(WebPage webPage, bool persist = true, CancellationToken cancellation = default);
        Task UpdateWebPageAsync(WebPage webPage, CancellationToken cancellation = default);
        Task<IEnumerable<Content>> UpdateWebPageContentsAsync(WebPage webPage, IEnumerable<Content> contents, CancellationToken cancellation = default);
    }
}