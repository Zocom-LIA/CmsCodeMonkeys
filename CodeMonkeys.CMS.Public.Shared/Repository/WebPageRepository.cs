using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class WebPageRepository : IWebPageRepository
    {
        public WebPageRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public ApplicationDbContext Context { get; }

        public async Task CreateWebPageAsync(WebPage webPage)
        {
            Context.Pages.Add(webPage);

            await Context.SaveChangesAsync();
        }

        public async Task<WebPage?> GetSiteWebPageAsync(int siteId, int pageId)
        {
            var page = Context.Pages.Where(page => page.SiteId == siteId && page.WebPageId == pageId)
                .Include(page => page.Contents)
                .Include(page => page.Site)
                .Include(page => page.Author);

            return await page.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ContentDto>> GetWebPageContentsAsync(int pageId, bool sortContent = false)
        {
            var contents = Context.Pages
                .Where(page => page.WebPageId == pageId)
                .Include(page => page.Contents)
                .SelectMany(page => page.Contents.Select(content => new ContentDto
                {
                    Title = content.Title,
                    ContentType = content.ContentType,
                    Body = content.Body,
                    OrdinalNumber = content.OrdinalNumber
                }));

            if (sortContent)
            {
                contents = contents.OrderBy(content => content.OrdinalNumber);
            }

            return await contents.ToListAsync();
        }

        public async Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10)
        {
            if (pageIndex < 0) throw new ArgumentOutOfRangeException("PageIndex must be a positive number.");
            if (pageSize <= 0) throw new ArgumentOutOfRangeException("PageSize must be greater than zero.");

            return await Context.Pages
                .Where(page => page.SiteId == siteId)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Include(page => page.Contents)
                .Include(page => page.Site)
                .Include(page => page.Author)
                .ToListAsync();
        }

        public async Task UpdateWebPageAsync(WebPage webPage)
        {
            Context.Pages.Update(webPage);

            await Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WebPageDto>> GetVisitorWebPageAsync(int? pageId)
        {
            return (IEnumerable<WebPageDto>)await (pageId == null ? Context.PageStats : Context.PageStats.Where(pageStat => pageStat.PageStatsId == pageId)).ToListAsync();
        }

        public async Task<IEnumerable<Content>> UpdateWebPageContentsAsync(WebPage webPage, IEnumerable<Content> contents)
        {
            Context.Contents.UpdateRange(contents);
            await Context.SaveChangesAsync();

            return webPage.Contents.OrderBy(content => content.OrdinalNumber);
        }

        // Used for testing purposes only
        public Task<WebPage?> GetWebPageAsync(int webPageId)
        {
            return Context.Pages.FirstOrDefaultAsync(page => page.WebPageId == webPageId);
        }
    }
}