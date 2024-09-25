using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IWebPageRepository
    {
        Task<WebPage> CreateWebPageAsync(WebPage webPage);
        Task<IEnumerable<Content>> CreateWebPageContentsAsync(WebPage webPage, Content content);
        Task DeleteWebPageAsync(WebPage page);
        Task<WebPage?> GetSiteWebPageAsync(int siteId, int pageId, bool includeContents = false, bool includeAuthor = false, bool includeSite = false);
        Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10);

        Task<IEnumerable<WebPageDto>> GetVisitorWebPageAsync(int? pageId);
        Task<WebPageIncludeDto?> GetWebPageAsync(int webPageId, bool includeContents = true, bool includeAuthor = true, CancellationToken cancellationToken = default);
        Task UpdateWebPageAsync(WebPage webPage);
        Task<IEnumerable<Content>> UpdateWebPageContentsAsync(WebPage webPage, IEnumerable<Content> contents);
    }
}