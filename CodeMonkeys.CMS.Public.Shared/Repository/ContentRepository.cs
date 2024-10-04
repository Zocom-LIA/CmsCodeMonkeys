using AutoMapper;

using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class ContentRepository : RepositoryBase, IContentRepository
    {
        public ContentRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IMapper mapper, ILogger<ContentRepository> logger) : base(contextFactory, mapper, logger) { }

        public async Task<Content> CreateContentAsync(Content content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content), "Content must not be null.");

            var context = GetContext();

            try
            {
                EntityEntry<Content> entry = await context.Contents.AddAsync(content);
                await context.SaveChangesAsync();
                content = entry.Entity;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while creating the content.", ex);
            }
            finally
            {
                await context.DisposeAsync();
            }

            return content;
        }

        // Consider adding web page ID to the method signature
        public async Task<Content?> DeleteContentAsync(int contentId)
        {
            if (contentId <= 0) throw new ArgumentOutOfRangeException("ContentId must be greater than zero.");

            var context = GetContext();
            Content? content;

            try
            {
                content = await context.Contents.FindAsync(contentId);
                if (content == null) throw new ElementNotFoundException($"Content with ID '{contentId}' not found.");

                EntityEntry<Content> entry = context.Contents.Remove(content);
                await context.SaveChangesAsync();

                content = entry.Entity;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while deleting the content.", ex);
            }
            finally
            {
                await context.DisposeAsync();
            }

            return content;
        }

        public async Task<IEnumerable<Content>> GetWebPageContentsAsync(int pageId, int pageIndex = 0, int pageSize = 10)
        {
            if (pageIndex < 0) throw new ArgumentOutOfRangeException("PageIndex must be a positive number.");
            if (pageSize <= 0) throw new ArgumentOutOfRangeException("PageSize must be greater than zero.");

            var context = GetContext();
            IEnumerable<Content> contents = Enumerable.Empty<Content>();

            try
            {
                contents = await context.Pages
                    .Where(page => page.WebPageId == pageId)
                    .Include(page => page.Contents)
                    .SelectMany(page => page.Contents)
                    .Include(content => content.Author)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the contents.", ex);
            }
            finally
            {
                await context.DisposeAsync();
            }

            return contents;
        }

        public async Task<IEnumerable<Content>> UpdateContentsAsync(WebPage webPage, IEnumerable<Content> contents)
        {
            if (webPage == null) throw new ArgumentNullException(nameof(webPage));
            if (contents == null) throw new ArgumentNullException(nameof(contents));

            var context = GetContext();
            var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                context.Set<Content>().UpdateRange(contents);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                contents = await context.Contents.Where(content => content.ContentId == webPage.WebPageId).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("An error occurred while updating the contents.", ex);
            }
            finally
            {
                await context.DisposeAsync();
                await transaction.DisposeAsync();
            }

            return contents;
        }

        public async Task<IEnumerable<Content>> UpdateOrdinalNumbersAsync(ICollection<Content> contents, bool persist)
        {
            if (contents == null) throw new ArgumentNullException(nameof(contents));

            if (persist)
            {
                var context = GetContext();
                var transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    context.Set<Content>().UpdateRange(contents);

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var page = context.Pages
                        .Where(page => page.WebPageId == contents.First().WebPageId)
                        .Include(page => page.Contents);
                    contents = await page
                        .SelectMany(page => page.Contents)
                        .OrderBy(content => content.OrdinalNumber)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new InvalidOperationException("An error occurred while updating the ordinal numbers.", ex);
                }
                finally
                {
                    await context.DisposeAsync();
                    await transaction.DisposeAsync();
                }
            }

            return contents;
        }
    }
}