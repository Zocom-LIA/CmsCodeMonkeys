using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Repository;

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

        public async Task<IEnumerable<WebPageDto>> GetVisitorPageAsync(int? pageId = null)
        {
            return await Repository.GetVisitorWebPageAsync(pageId);
        }

        public async Task<WebPage> UpdateOrdinalNumbersAsync(WebPage webPage, bool persist = true)
        {
            var contents = webPage.Contents
                .OrderBy(c => c.OrdinalNumber)
                .Select((c, i) =>
                {
                    c.OrdinalNumber = i + 1;
                    return c;
                });

            if (persist)
            {
                await Repository.UpdateWebPageContentsAsync(webPage);
            }

            return webPage;
        }


        public async Task MoveUpAsync(int webPageId, int contentId)
        {
            // Get the content
            // Get the content's ordinal number
            // Get the content with the ordinal number one less than the current content
            // Swap the ordinal numbers
            // Update the contents
            var items = await GetWebPageContentsAsync(webPageId);
        }

        public Task<WebPage> MoveContentDownAsync(WebPage webPage, Content content, bool persist = true)
        {
            if (content.OrdinalNumber < 1) throw new InvalidOperationException($"Content with ID '{content.ContentId}' has an illegal state '{content.OrdinalNumber}'.");
            if (content.OrdinalNumber == 1) return Task.FromResult(webPage);

            var contents = webPage.Contents;
            var item = webPage.Contents.FirstOrDefault(c => c.ContentId == content.ContentId);

            if (item == null)
            {
                throw new InvalidOperationException($"Attempting to move content with ID '{content.ContentId}' up from web page with ID '{webPage.WebPageId}', but it is not found in its contents.");
            }

            content.OrdinalNumber--;

            // Move previous content up
            var previous = contents.FirstOrDefault(c => (int)c.OrdinalNumber == content.OrdinalNumber);

            if (previous == null) throw new InvalidOperationException("Should never happen.");

            previous.OrdinalNumber++;

            return UpdateOrdinalNumberAsync(webPage, contents, persist);
        }

        private async Task<WebPage> UpdateOrdinalNumberAsync(WebPage webPage, ICollection<Content> contents, bool persist)
        {
            throw new NotImplementedException();
        }

        public Task<WebPage> MoveContentUpAsync(WebPage webPage, Content content, bool persist = true)
        {
            var contents = webPage.Contents;

            if (content.OrdinalNumber > contents.Count()) throw new InvalidOperationException($"Content with ID '{content.ContentId}' has an illegal state '{content.OrdinalNumber}'.");
            if (content.OrdinalNumber == contents.Count()) return Task.FromResult(webPage);

            var item = webPage.Contents.FirstOrDefault(c => c.ContentId == content.ContentId);

            if (item == null)
            {
                throw new InvalidOperationException($"Attempting to move content with ID '{content.ContentId}' up from web page with ID '{webPage.WebPageId}', but it is not found in its contents.");
            }

            content.OrdinalNumber++;

            // Move next content down
            var next = contents.FirstOrDefault(c => c.OrdinalNumber == content.OrdinalNumber);

            if (next == null) throw new InvalidOperationException("Should never happen.");

            next.OrdinalNumber--;
            webPage.Contents = contents.OrderBy(c => c.OrdinalNumber).ToList();

            return UpdateOrdinalNumberAsync(webPage, contents, persist);
        }
    }
}