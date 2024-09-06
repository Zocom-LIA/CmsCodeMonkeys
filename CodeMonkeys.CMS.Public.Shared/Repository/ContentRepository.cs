using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class ContentRepository : IContentRepository
    {
        public ContentRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public ApplicationDbContext Context { get; }

        public async Task<Content> CreateContentAsync(Content content)
        {
            EntityEntry<Content> entry = await Context.Contents.AddAsync(content);
            await Context.SaveChangesAsync();

            return entry.Entity;
        }

        // Consider adding web page ID to the method signature
        public async Task<Content> DeleteContentAsync(int contentId)
        {
            if (contentId <= 0) throw new ArgumentOutOfRangeException("ContentId must be greater than zero.");

            var content = await Context.Contents.FindAsync(new object[] { contentId });

            if (content == null) throw new InvalidOperationException($"Content with ID '{contentId}' not found.");

            var entry = Context.Contents.Remove(content);
            await Context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<IEnumerable<Content>> GetWebPageContentsAsync(int pageId, int pageIndex = 0, int pageSize = 10)
        {
            if (pageIndex < 0) throw new ArgumentOutOfRangeException("PageIndex must be a positive number.");
            if (pageSize <= 0) throw new ArgumentOutOfRangeException("PageSize must be greater than zero.");

            return await Context.Pages
                .Where(page => page.WebPageId == pageId)
                .Include(page => page.Contents)
                .SelectMany(page => page.Contents)
                .Include(content => content.Author)
                .ToListAsync();
        }

        public async Task<IEnumerable<Content>> UpdateContentsAsync(WebPage webPage, IEnumerable<Content> contents)
        {
            Context.Contents.UpdateRange(contents);
            await Context.SaveChangesAsync();

            return webPage.Contents.OrderBy(content => content.OrdinalNumber);
        }

        public async Task<IEnumerable<Content>> UpdateOrdinalNumbersAsync(ICollection<Content> contents, bool persist)
        {
            if (contents == null) throw new ArgumentNullException(nameof(contents));

            if (persist)
            {
                Context.Contents.UpdateRange(contents);
                await Context.SaveChangesAsync();
            }

            return contents.OrderBy(content => content.OrdinalNumber);
        }
    }
}