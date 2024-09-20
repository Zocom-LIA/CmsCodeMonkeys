using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IWebPageRepository
    {
        Task CreateWebPageAsync(WebPage webPage, CancellationToken cancellation = default);
        Task<IEnumerable<Content>> CreateWebPageContentsAsync(WebPage webPage, Content content, CancellationToken cancellation = default);
        Task DeleteWebPageAsync(WebPage page, CancellationToken cancellation = default);
        Task<WebPage?> GetSiteWebPageAsync(int siteId, int pageId, CancellationToken cancellation = default);
        Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10, CancellationToken cancellation = default);

        Task<IEnumerable<WebPageDto>> GetVisitorWebPageAsync(int? pageId, CancellationToken cancellation = default);
        Task<WebPage?> GetWebPageAsync(int webPageId, CancellationToken cancellation = default);
        Task<IEnumerable<Content>> GetWebPageContentsAsync(int webPageId, CancellationToken cancellation = default);
        Task<IEnumerable<ContentDto>> GetWebPageContentsAsync(int webPageId, bool sortContent, CancellationToken cancellation = default);
        Task UpdateWebPageAsync(WebPage webPage, CancellationToken cancellation = default);
        Task<IEnumerable<Content>> UpdateWebPageContentsAsync(WebPage webPage, IEnumerable<Content> contents, CancellationToken cancellation = default);
        Task<IEnumerable<Content>> UpdateWebPageContentsAsync(WebPage webPage, IEnumerable<Content> contents, bool persist = false, CancellationToken cancellation = default);
        Task SaveChangesAsync(CancellationToken cancellation = default);
        Task<bool> ExistsAsync(int webPageId, CancellationToken cancellation);
    }
}
