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

        public async Task<IEnumerable<ContentDto>> GetWebPageContentsAsync(int pageId)
        {
            return await Context.Pages
                .Where(page => page.WebPageId == pageId)
                .Include(page => page.Contents)
                .SelectMany(page => page.Contents.Select(content => new ContentDto
                {
                    Title = content.Title,
                    ContentType = content.ContentType,
                    Body = content.Body,
                    OrdinalNumber = content.OrdinalNumber
                }))
                .ToListAsync();
        }

        public async Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10)
        {
            if (pageIndex < 0) throw new ArgumentOutOfRangeException("PageIndex must be a positive number.");
            if (pageSize <= 0) throw new ArgumentOutOfRangeException("PageSize must be greater than zero.");

            return await Context.Pages
                .Where(page => page.SiteId == siteId)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .OrderBy(page => page)
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

        public async Task<IEnumerable<Content>> MoveContentDownAsync(WebPage webPage, int ordinalNumber)
        {
            var contents = webPage.Contents.OrderBy(content => content.OrdinalNumber).ToArray();
            int index = Array.FindIndex(contents, c => c.OrdinalNumber == ordinalNumber);

            if (index < 0 || index >= contents.Length - 1)
            {
                return webPage.Contents;
            }

            (contents[index], contents[index + 1]) = (contents[index + 1], contents[index]);

            return await UpdateOrdinalNumbersAsync(contents);
        }

        public async Task<IEnumerable<Content>> MoveContentUpAsync(WebPage webPage, int ordinalNumber)
        {
            var contents = webPage.Contents.ToArray();
            int index = Array.FindIndex(contents, c => c.OrdinalNumber == ordinalNumber);

            if (index <= 0)
            {
                return webPage.Contents;
            }

            (contents[index], contents[index - 1]) = (contents[index - 1], contents[index]);

            return await UpdateOrdinalNumbersAsync(contents);
        }

        private async Task<IEnumerable<Content>> UpdateOrdinalNumbersAsync(Content[] contents)
        {
            List<Content> contentsToUpdate = new List<Content>();

            for (int i = 0; i < contents.Length; i++)
            {
                if (contents[i].OrdinalNumber != i)
                {
                    contents[i].OrdinalNumber = i;
                    contentsToUpdate.Add(contents[i]);
                }
            }

            Context.Contents.UpdateRange(contentsToUpdate);
            await Context.SaveChangesAsync();

            return contents.OrderBy(content => content.OrdinalNumber);
        }
    }
}