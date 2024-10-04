using AutoMapper;

using Azure;

using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using System.Linq;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class WebPageRepository : RepositoryBase, IWebPageRepository
    {
        public WebPageRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IMapper mapper, ILogger<WebPageRepository> logger)
            : base(contextFactory, mapper, logger)
        { }


        public async Task<WebPage> CreateWebPageAsync(WebPage webPage, CancellationToken cancellationToken = default)
        {
            if (webPage == null) throw new ArgumentNullException(nameof(webPage), "WebPage must not be null.");

            var context = GetContext();

            try
            {
                EntityEntry<WebPage> entry = await context.Pages.AddAsync(webPage);
                await context.SaveChangesAsync();

                return entry.Entity;
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

        public async Task<WebPage?> GetVisitorWebPageAsync(int webPageId, bool includeSite, bool includeAuthor, bool includeSections, bool includeContents, CancellationToken cancellation = default)
        {
            var context = GetContext();

            try
            {
                var query = context.Pages
                    .Where(page => page.WebPageId == webPageId);

                if (includeSite)
                {
                    query = query.Include(page => page.Site);
                }

                if (includeAuthor)
                {
                    query = query.Include(page => page.Author);
                }

                if (includeSections)
                {
                    query = query.Include(page => page.Sections)
                        .ThenInclude(section => section.ContentItems);
                }

                if (includeContents)
                {
                    query = query.Include(page => page.Contents);
                }

                return await query.FirstOrDefaultAsync();
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<WebPageIncludeDto?> GetWebPageAsync(int webPageId, bool includeSite, bool includeAuthor, bool includeSections, bool includeContents, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                var context = GetContext();

                // Step 1: Select necessary data instead of loading entire entities with Include
                var webPages = await context.Pages
                    .AsNoTracking()
                    .Where(wp => wp.WebPageId == webPageId)
                    .Select(wp => new
                    {
                        wp.WebPageId,
                        wp.Title,
                        wp.CreatedDate,
                        wp.LastModifiedDate,
                        wp.SiteId,
                        wp.AuthorId,
                        Contents = includeContents ? wp.Contents.Select(c => new
                        {
                            c.ContentId,
                            c.Body,
                            c.Title,
                            c.Color,
                            c.CreatedDate,
                            c.LastModifiedDate,
                            c.AuthorId
                        }) : null,
                    })
                    .FirstOrDefaultAsync(cancellation);

                if (webPages == null) return null;

                // Step 2: Extract all distinct author IDs (ignoring nulls)
                var authorIds = webPages.Contents?
                    .Select(c => c.AuthorId)
                    .Concat(new[] { webPages.AuthorId })
                    .Where(id => id.HasValue)
                    .Distinct()
                    .Select(id => id!.Value);

                // Step 3: Fetch authors in a single query and use projection to avoid loading unnecessary data
                var authors = includeAuthor ? await context.Users
                    .AsNoTrackingWithIdentityResolution()
                    .Where(u => authorIds!.Contains(u.Id))
                    .Select(u => new { u.Id, User = _mapper.Map<AuthorDto>(u) })  // Project to UserDto for efficiency
                    .ToDictionaryAsync(u => u.Id, u => u.User, cancellation)
                    : new Dictionary<Guid, AuthorDto>();

                // Step 4: Assign the correct author to each web page and its content
                var author = webPages.AuthorId.HasValue ? authors.GetValueOrDefault(webPages.AuthorId.Value) : null;

                return new WebPageIncludeDto
                {
                    WebPageId = webPages.WebPageId,
                    Title = webPages.Title,
                    CreatedDate = webPages.CreatedDate,
                    LastModifiedDate = webPages.LastModifiedDate,
                    Contents = (webPages.Contents?.Select(content =>
                    {
                        var contentAuthor = content.AuthorId.HasValue ? authors.GetValueOrDefault(content.AuthorId.Value) : null;
                        return new ContentDto
                        {
                            ContentId = content.ContentId,
                            Author = contentAuthor
                        };
                    }) ?? []).ToList(),
                    Author = author
                };
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting web page {webPageId}", ex);
            }
        }

        public async Task<WebPage?> GetSiteWebPageAsync(int siteId, int pageId, bool includeContents = false, bool includeAuthor = false, bool includeSite = false, CancellationToken cancellationToken = default)
        {
            if (siteId <= 0) throw new ArgumentOutOfRangeException("SiteId must be greater than zero.");
            if (pageId <= 0) throw new ArgumentOutOfRangeException("PageId must be greater than zero.");

            var context = GetContext();

            try
            {
                bool usedTracking = false;
                var query = context.Pages
                    .Where(page => page.WebPageId == pageId && page.SiteId == siteId);

                if (query.TryGetNonEnumeratedCount(out var nonEnumeratedCount))
                {
                    if (nonEnumeratedCount == 0) return null;
                }

                if (includeAuthor)
                {
                    query = query.Include(page => page.Author);
                    query = query.AsNoTrackingWithIdentityResolution();
                    usedTracking = true;
                }

                if (includeSite)
                {
                    query = query.Include(page => page.Site);
                }

                if (includeContents)
                {
                    query = query.Include(page => page.Contents);

                    if (includeAuthor)
                    {
                        query = query.Include(page => page.Contents).ThenInclude(content => content.Author);
                    }
                    else
                    {
                        query = query.Include(page => page.Contents);
                    }

                    if (!usedTracking)
                    {
                        query = query.AsNoTrackingWithIdentityResolution();
                        usedTracking = true;
                    }
                }

                if (!usedTracking)
                {
                    query = query.AsNoTracking();
                }

                var page = await query.FirstOrDefaultAsync();

                if (page == null) return null;

                page.Contents = page.Contents.OrderBy(c => c.OrdinalNumber).ToList();

                return page;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<IEnumerable<ContentDto>> GetWebPageContentsAsync(int pageId, bool sortContent = false, CancellationToken cancellationToken = default)
        {
            if (pageId <= 0) throw new ArgumentOutOfRangeException("PageId must be greater than zero.");

            var context = GetContext();
            IEnumerable<ContentDto> contents = Enumerable.Empty<ContentDto>();

            try
            {
                // Step 1: Select only necessary data
                var query = context.Contents
                    .Where(c => c.WebPageId == pageId)
                    .Select(c => new ContentDto
                    {
                        Title = c.Title,
                        ContentType = c.ContentType,
                        Body = c.Body,
                        OrdinalNumber = c.OrdinalNumber
                    });

                // Step 2: Apply sorting if needed
                if (sortContent)
                {
                    query = query.OrderBy(content => content.OrdinalNumber);
                }

                // Step 3: Fetch data efficiently
                contents = await query.AsNoTracking().ToListAsync();

                return contents;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10, CancellationToken cancellationToken = default)
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

        public async Task UpdateWebPageAsync(WebPage webPage, CancellationToken cancellationToken = default)
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

        public async Task<IEnumerable<WebPageDto>> GetVisitorWebPageAsync(int? pageId, CancellationToken cancellationToken = default)
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

        public async Task<IEnumerable<Content>> UpdateWebPageContentsAsync(WebPage webPage, IEnumerable<Content> contents, CancellationToken cancellationToken = default)
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

                return webPage.Contents.OrderBy(content => content.OrdinalNumber);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while updating the contents.");
                throw new RepositoryException("Error when updating the contents.", ex);
            }
            finally
            {
                await context.DisposeAsync();
                await transaction.DisposeAsync();
            }
        }

        public async Task DeleteWebPageAsync(WebPage page, CancellationToken cancellationToken = default)
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

        public async Task<IEnumerable<Content>> CreateWebPageContentsAsync(WebPage webPage, Content content, CancellationToken cancellationToken = default)
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

                var existingPage = await context.Pages.FindAsync(webPage.WebPageId);

                if (existingPage == null)
                {
                    throw new InvalidOperationException("Page not found");
                }

                content.AuthorId = existingUser.Id;
                content.CreatedDate = DateTime.UtcNow;
                content.LastModifiedDate = DateTime.UtcNow;
                content.WebPageId = existingPage.WebPageId;

                await context.Contents.AddAsync(content);
                await context.SaveChangesAsync();

                return await context.Pages
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
        }
        public async Task<IEnumerable<Content>> CreateWebPageContentsAsync2(WebPage webPage, Content content, CancellationToken cancellationToken = default)
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