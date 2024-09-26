using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Extensions;
using CodeMonkeys.CMS.Public.Shared.Repository;

using System.Threading;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public class WebPageService : IWebPageService
    {
        public WebPageService(IWebPageRepository repository)
        {
            Repository = repository;
        }

        public IWebPageRepository Repository { get; }

        public async Task<WebPage> CreateWebPageAsync(int siteId, WebPage webPage)
        {
            if (!webPage.SiteId.HasValue)
            {
                webPage.SiteId = siteId;
            }
            else if (webPage.SiteId != siteId)
            {
                throw new InvalidOperationException($"Site ID does not match. Expected Site ID '{siteId}' Actual Site ID '{webPage.SiteId}'.");
            }

            webPage.CreatedDate = DateTime.Now;
            webPage.LastModifiedDate = DateTime.Now;

            return await Repository.CreateWebPageAsync(webPage);
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

        public async Task<WebPage?> GetSiteWebPageAsync(int siteId, int pageId, bool includeContents = false, bool includeAuthor = false, bool includeSite = false)
        {
            return await Repository.GetSiteWebPageAsync(siteId, pageId, includeContents, includeAuthor, includeSite);
        }

        public async Task<IEnumerable<WebPageDto>> GetVisitorPageAsync(int? pageId = null)
        {
            return await Repository.GetVisitorWebPageAsync(pageId);
        }

        public async Task<IEnumerable<Content>> UpdateOrdinalNumbersAsync(WebPage webPage, bool persist = true)
        {
            var contents = webPage.Contents.OrderBy(c => c.OrdinalNumber).ToArray();
            List<Content> unorderedContents = new List<Content>();
            for (int i = 0; i < contents.Count(); i++)
            {
                if (contents[i].OrdinalNumber != i)
                {
                    unorderedContents.Add(contents[i]);
                    contents[i].OrdinalNumber = i;
                }
            }


            return (!persist || unorderedContents.Count() == 0)
                ? contents.OrderBy(c => c.OrdinalNumber)
                : await Repository.UpdateWebPageContentsAsync(webPage, unorderedContents);
        }

        public async Task<IEnumerable<Content>> MoveContentUpAsync(WebPage page, int ordinalNumber)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            var items = page.Contents.OrderBy(c => c.OrdinalNumber).ToArray();
            var currentIndex = items.FindIndex(i => i.OrdinalNumber == ordinalNumber);

            if (currentIndex <= 0)
            {
                return await UpdateOrdinalNumbersAsync(page, true);
            }

            items[currentIndex - 1].OrdinalNumber = ordinalNumber;
            items[currentIndex].OrdinalNumber = ordinalNumber - 1;

            await UpdateOrdinalNumbersAsync(page, true);

            return await Repository.UpdateWebPageContentsAsync(page, [items[currentIndex], items[currentIndex - 1]]);
        }

        public async Task<IEnumerable<Content>> MoveContentDownAsync(WebPage page, int ordinalNumber)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            var items = page.Contents.OrderBy(c => c.OrdinalNumber).ToArray();
            var currentIndex = items.FindIndex(i => i.OrdinalNumber == ordinalNumber);

            if (currentIndex < 0 || currentIndex >= items.Length - 1)
            {
                return await UpdateOrdinalNumbersAsync(page, true);
            }

            items[currentIndex + 1].OrdinalNumber = ordinalNumber;
            items[currentIndex].OrdinalNumber = ordinalNumber + 1;

            return await Repository.UpdateWebPageContentsAsync(page, [items[currentIndex], items[currentIndex + 1]]);
        }

        public async Task<IEnumerable<Content>> CreateWebPageContentAsync(WebPage webPage, Content content)
        {
            content.CreatedDate = DateTime.Now;
            content.LastModifiedDate = DateTime.Now;

            if (content.OrdinalNumber == 0 || content.OrdinalNumber > webPage.Contents.Count())
            {
                content.OrdinalNumber = webPage.Contents.Count();
            }

            return await Repository.CreateWebPageContentsAsync(webPage, content);
        }

        public async Task<IEnumerable<Content>> UpdateWebPageContentsAsync(WebPage webPage, IEnumerable<Content> contents)
        {
            if (webPage == null) throw new ArgumentNullException(nameof(webPage), "WebPage cannot be null.");
            if (contents == null) throw new ArgumentNullException(nameof(contents), "Contents cannot be null.");

            return await Repository.UpdateWebPageContentsAsync(webPage, contents);
        }

        public Task DeleteWebPageAsync(WebPage page)
        {
            return Repository.DeleteWebPageAsync(page);
        }
    }
}