using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public class WebPageService : IWebPageService
    {
        public WebPageService(IWebPageRepository repository)
        {
            Repository = repository;
        }

        public IWebPageRepository Repository { get; }

        public async Task CreateWebPageAsync(WebPage webPage)
        {
            await Repository.CreateWebPageAsync(webPage);
        }

        public async Task UpdateWebPageAsync(WebPage webPage)
        {
            await Repository.UpdateWebPageAsync(webPage);
        }

        public async Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10)
        {
            if (pageIndex < 0) throw new ArgumentOutOfRangeException("PageIndex must be a positive number.");
            if (pageSize <= 0) throw new ArgumentOutOfRangeException("PageSize must be greater than zero.");

            return await Repository.GetSiteWebPagesAsync(siteId, pageIndex, pageSize);
        }

        public async Task<WebPage?> GetSiteWebPageAsync(int siteId, int pageId)
        {
            return await Repository.GetSiteWebPageAsync(siteId, pageId);
        }

        public async Task<IEnumerable<ContentDto>> GetWebPageContentsAsync(int pageId)
        {
            return await Repository.GetWebPageContentsAsync(pageId);
        }

        public async Task<IEnumerable<Content>> MoveContentUpAsync(WebPage webPage, int ordinalNumber)
        {
            return await Repository.MoveContentUpAsync(webPage, ordinalNumber);
        }

        public async Task<IEnumerable<Content>> MoveContentDownAsync(WebPage webPage, int ordinalNumber)
        {
            return await Repository.MoveContentDownAsync(webPage, ordinalNumber);
        }
    }
}