using Azure;

using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Linq;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class WebPageRepository : RepositoryBase, IWebPageRepository
    {
        public WebPageRepository(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<WebPageRepository> logger) : base(contextFactory, logger)
        { }


        public async Task CreateWebPageAsync(WebPage webPage)
        {
            if (webPage == null) throw new ArgumentNullException(nameof(webPage), "WebPage must not be null.");

            var context = GetContext();

            try
            {
                await context.Pages.AddAsync(webPage);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new web page.");
                throw new RepositoryException("Error when creating a web page.", ex);
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<WebPage?> GetSiteWebPageAsync(int siteId, int pageId)
        {
            if (siteId <= 0) throw new ArgumentOutOfRangeException("SiteId must be greater than zero.");
            if (pageId <= 0) throw new ArgumentOutOfRangeException("PageId must be greater than zero.");

            var context = GetContext();
            WebPage? page = null;

            try
            {
                page = await context.Pages
                    .Include(page => page.Contents)
                    .Include(page => page.Site)
                    .Include(page => page.Author)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(page => page.SiteId == siteId && page.WebPageId == pageId);
            }
            finally
            {
                await context.DisposeAsync();
            }

            return page;
        }

        public async Task<IEnumerable<ContentDto>> GetWebPageContentsAsync(int pageId, bool sortContent = false)
        {
            if (pageId <= 0) throw new ArgumentOutOfRangeException("PageId must be greater than zero.");

            var context = GetContext();
            IEnumerable<ContentDto> contents = Enumerable.Empty<ContentDto>();

            try
            {
                var query = context.Pages
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

                contents = await query.AsNoTracking().ToListAsync();
            }
            finally
            {
                await context.DisposeAsync();
            }

            return contents;
        }

        public async Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10)
        {
            if (pageIndex < 0) throw new ArgumentOutOfRangeException("PageIndex must be a positive number.");
            if (pageSize <= 0) throw new ArgumentOutOfRangeException("PageSize must be greater than zero.");

            var context = GetContext();
            IEnumerable<WebPage> pages = Enumerable.Empty<WebPage>();

            try
            {
                pages = await context.Pages
                    .Where(page => page.SiteId == siteId)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(page => page.LastModifiedDate)
                    .Include(page => page.Contents)
                    .Include(page => page.Site)
                    .Include(page => page.Author)
                    .AsNoTracking()
                    .ToListAsync();
            }
            finally
            {
                await context.DisposeAsync();
            }

            return pages;
        }

        public async Task UpdateWebPageAsync(WebPage webPage)
        {
            if (webPage == null) throw new ArgumentNullException(nameof(webPage), "WebPage must not be null.");

            var context = GetContext();

            try
            {
                context.Pages.Update(webPage);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a web page.");
                throw new RepositoryException("Error when updating a web page.", ex);
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<IEnumerable<WebPageDto>> GetVisitorWebPageAsync(int? pageId)
        {
            var context = GetContext();
            IEnumerable<WebPageDto> pages = Enumerable.Empty<WebPageDto>();

            try
            {
                pages = await context.Pages
                    .Where(page => page.WebPageId == pageId)
                    .Select(page => new WebPageDto
                    {
                        Title = page.Title,
                        Contents = page.Contents.Select(content => new ContentDto
                        {
                            Title = content.Title,
                            ContentType = content.ContentType,
                            Body = content.Body,
                            OrdinalNumber = content.OrdinalNumber
                        })
                    })
                    .AsNoTracking()
                    .ToListAsync();
            }
            finally
            {
                await context.DisposeAsync();
            }

            return pages;
        }

        public async Task<IEnumerable<Content>> UpdateWebPageContentsAsync(WebPage webPage, IEnumerable<Content> contents)
        {
            if (webPage == null) throw new ArgumentNullException(nameof(webPage), "WebPage must not be null.");
            if (contents == null) throw new ArgumentNullException(nameof(contents), "Contents must not be null.");

            var context = GetContext();
            var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                context.Set<Content>().UpdateRange(contents);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                contents = await context.Contents
                    .Where(content => content.WebPageId == webPage.WebPageId)
                    .AsNoTracking()
                    .ToListAsync();
            }
            finally
            {
                await context.DisposeAsync();
            }

            return contents;
        }

        // Used for testing purposes only
        public async Task<WebPage?> GetWebPageAsync(int webPageId)
        {
            var context = GetContext();
            WebPage? page = null;

            try
            {
                page = await context.Pages
                    .Include(page => page.Contents)
                    .Include(page => page.Site)
                    .Include(page => page.Author)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(page => page.WebPageId == webPageId);
            }
            finally
            {
                await context.DisposeAsync();
            }

            return page;
        }

        public async Task DeleteWebPageAsync(WebPage page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page), "WebPage must not be null.");

            var context = GetContext();

            try
            {
                context.Pages.Remove(page);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a web page.");
                throw new RepositoryException("Error when deleting a web page.", ex);
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<IEnumerable<Content>> CreateWebPageContentsAsync(WebPage webPage, Content content)
        {
            if (webPage == null) throw new ArgumentNullException(nameof(webPage), "WebPage must not be null.");
            if (content == null) throw new ArgumentNullException(nameof(content), "Content must not be null.");

            var context = GetContext();
            IEnumerable<Content>? contents = null;

            try
            {
                var existingUser = await GetUserAsync(context, content.AuthorId ?? content.Author?.Id);

                if (existingUser == null)
                {
                    throw new InvalidOperationException("User not found");
                }

                var newContent = new Content
                {
                    Title = content.Title,
                    Body = content.Body,
                    ContentType = content.ContentType,
                    OrdinalNumber = content.OrdinalNumber,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Color = content.Color,
                    AuthorId = existingUser.Id,
                    WebPageId = webPage.WebPageId
                };

                await context.Contents.AddAsync(newContent);
                await context.SaveChangesAsync();

                contents = await context.Pages
                    .Where(page => page.WebPageId == webPage.WebPageId)
                    .Include(page => page.Contents)
                    .AsNoTracking()
                    .SelectMany(page => page.Contents)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new content.");
                throw new RepositoryException("Error when creating a content.", ex);
            }
            finally
            {
                await context.DisposeAsync();
            }

            return contents;
        }
        public async Task<IEnumerable<Content>> CreateWebPageContentsAsync2(WebPage webPage, Content content)
        {
            if (webPage == null) throw new ArgumentNullException(nameof(webPage), "WebPage must not be null.");
            if (content == null) throw new ArgumentNullException(nameof(content), "Content must not be null.");

            var context = GetContext();
            IEnumerable<Content>? contents = null;

            try
            {
                Guid authorId = content.AuthorId ?? content.Author?.Id ?? Guid.Empty;
                var newContent = new Content
                {
                    Title = content.Title,
                    Body = content.Body,
                    ContentType = content.ContentType,
                    OrdinalNumber = content.OrdinalNumber,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Color = content.Color,
                    AuthorId = authorId,
                    WebPageId = webPage.WebPageId
                };

                await context.Contents.AddAsync(newContent);
                await context.SaveChangesAsync();

                contents = await context.Pages
                    .Where(page => page.WebPageId == webPage.WebPageId)
                    .Include(page => page.Contents)
                    .AsNoTracking()
                    .SelectMany(page => page.Contents)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new content.");
                throw new RepositoryException("Error when creating a content.", ex);
            }
            finally
            {
                await context.DisposeAsync();
            }

            return contents;
        }
    }
}