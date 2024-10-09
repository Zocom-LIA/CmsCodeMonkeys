using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IWebPageRepository
    {
        Task<WebPage> CreateWebPageAsync(WebPage webPage, CancellationToken cancellation = default);
        Task<IEnumerable<Content>> CreateWebPageContentsAsync(WebPage webPage, Content content, CancellationToken cancellation = default);
        Task DeleteWebPageAsync(WebPage page, CancellationToken cancellation = default);
        Task<WebPage?> GetSiteWebPageAsync(int siteId, int pageId, bool includeContents = false, bool includeAuthor = false, bool includeSite = false, CancellationToken cancellation = default);
        Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10, CancellationToken cancellation = default);

        Task<IEnumerable<WebPageDto>> GetVisitorWebPageAsync(int? pageId, CancellationToken cancellation = default);
        Task<WebPage?> GetVisitorWebPageAsync(int webPageId, bool includeSite, bool includeAuthor, bool includeSections, bool includeContents, CancellationToken cancellation = default);
        Task<WebPageIncludeDto?> GetWebPageAsync(int webPageId, bool includeSite = false, bool includeAuthor = false, bool includeSections = false, bool includeContents = false, CancellationToken cancellation = default);
        Task UpdateWebPageAsync(WebPage webPage, CancellationToken cancellation = default);
        Task<IEnumerable<Content>> UpdateWebPageContentsAsync(WebPage webPage, IEnumerable<Content> contents, CancellationToken cancellation = default);
    }
}