using AutoMapper;

using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;
using Microsoft.EntityFrameworkCore.Internal;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class ContentItemRepository : CodeMonkeys.CMS.Public.Shared.Repository.RepositoryBase, CodeMonkeys.CMS.Public.Shared.Repository.IContentItemRepository
    {
        public ContentItemRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IMapper mapper, ILogger<ContentItemRepository> logger)
            : base(contextFactory, mapper, logger)
        {
        }

        public async Task AddContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default)
        {
            var context = GetContext();

            try
            {
                cancellation.ThrowIfCancellationRequested();
                contentItem.CreatedDate = DateTime.UtcNow;
                contentItem.ContentType = "TextContent";
                contentItem.LastModifiedDate = DateTime.UtcNow;
                await context.ContentItems.AddAsync(contentItem, cancellation);
                await context.SaveChangesAsync(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding content item");
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task UpdateContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(contentItem);

            var context = GetContext();

            try
            {
                //context.Entry<ContentItem>(contentItem).State = EntityState.Modified;
                context.ContentItems.Update(contentItem);
                await context.SaveChangesAsync(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating content item");
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<Dictionary<int, Section>> GetSectionContentItemsAsync(int webPageId, CancellationToken cancellation = default)
        {
            var context = GetContext();

            try
            {
                var sections = await context.Sections
                    .Where(s => s.WebPageId == webPageId)
                    .Include(s => s.ContentItems)
                    .ToListAsync(cancellation);

                var sectionContentItems = new Dictionary<int, Section>();

                foreach (var section in sections)
                {
                    var contentItems = await context.ContentItems
                        .Where(ci => ci.SectionId == section.SectionId)
                        .ToListAsync(cancellation);

                    sectionContentItems.Add(section.SectionId, section);
                    section.ContentItems = contentItems;
                }

                return sectionContentItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting section content items");
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task DeleteContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(contentItem);

            var context = GetContext();

            try
            {
                context.ContentItems.Remove(contentItem);
                await context.SaveChangesAsync(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting content item");
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(int sectionId, CancellationToken cancellation = default)
        {
            var context = GetContext();

            try
            {
                return await context.ContentItems
                    .Where(ci => ci.SectionId == sectionId)
                    .ToListAsync(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting content items");
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<ContentItem?> GetContentItemAsync(int contentItemId, CancellationToken cancellation = default)
        {
            var context = GetContext();

            try
            {
                return await context.ContentItems.FindAsync(contentItemId, cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting content item");
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<int> contentItemIds, CancellationToken cancellation = default)
        {
            var context = GetContext();

            try
            {
                return await context.ContentItems
                    .Where(ci => contentItemIds.Contains(ci.ContentItemId))
                    .ToListAsync(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting content items");
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<ContentItem> contentItems, CancellationToken cancellation = default)
        {
            var context = GetContext();

            try
            {
                return await context.ContentItems
                    .Where(ci => contentItems.Contains(ci))
                    .ToListAsync(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting content items");
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<Section> sections, CancellationToken cancellation = default)
        {
            var context = GetContext();

            try
            {
                return await context.ContentItems
                    .Where(ci => sections.Contains(ci.Section))
                    .ToListAsync(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting content items");
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }
    }
}