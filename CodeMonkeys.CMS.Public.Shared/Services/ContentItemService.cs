using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Repository;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public class ContentItemService(ContentItemRepository repository) : IContentItemService
    {
        // ContentItem lists
        public ContentItem? DraggedContentItem { get; private set; }
        private ContentItemRepository _repository = repository;

        // Add a new ContentItem item to a specific list
        public async Task AddContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(contentItem, nameof(contentItem));
            await _repository.AddContentItemAsync(contentItem, cancellation);
        }
        public async Task DeleteContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(contentItem, nameof(contentItem));
            await _repository.DeleteContentItemAsync(contentItem, cancellation);
        }
        public async Task UpdateContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(contentItem, nameof(contentItem));
            await _repository.UpdateContentItemAsync(contentItem, cancellation);
        }
        public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<ContentItem> contentItems, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(contentItems, nameof(contentItems));
            return await _repository.GetContentItemsAsync(contentItems, cancellation);
        }
        public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<int> contentIds, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(contentIds, nameof(contentIds));
            return await _repository.GetContentItemsAsync(contentIds, cancellation);
        }
        public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<Section> sections, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(sections, nameof(sections));
            return await _repository.GetContentItemsAsync(sections, cancellation);
        }
        public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(int sectionId, CancellationToken cancellation = default) => await _repository.GetContentItemsAsync(sectionId);

        // Get sections and their ContentItem items if they exist; otherwise, create a new section
        public async Task<Dictionary<int, Section>> GetSectionContentItemsAsync(int webPageId, CancellationToken cancellation = default)
        {
            return await _repository.GetSectionContentItemsAsync(webPageId, cancellation);
        }

        // Start dragging a ContentItem item
        public void StartDrag(ContentItem contentItem)
        {
            DraggedContentItem = contentItem;
        }

        // Drop the dragged ContentItem item into the target list
        public async Task MoveContentItemAsync(int newSectionId, CancellationToken cancellation = default)
        {
            if (DraggedContentItem != null)
            {
                DraggedContentItem.SectionId = newSectionId;

                await _repository.UpdateContentItemAsync(DraggedContentItem, cancellation);
                DraggedContentItem = null;
            }
        }

        public async Task RemoveContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(contentItem, nameof(contentItem));
            await _repository.DeleteContentItemAsync(contentItem, cancellation);
        }

        public async Task UpdateSectionIdAsync(int ContentId, int newSectionId, CancellationToken cancellation = default)
        {
            var contentItem = await _repository.GetContentItemByIdAsync(ContentId, cancellation);
            if (contentItem != null)
            {
                contentItem.SectionId = newSectionId;
                await _repository.UpdateContentItemAsync(contentItem, cancellation);
            }
        }

        public async Task UpdateSortOrderAsync(int contentId, int sortOrder, CancellationToken cancellation = default)
        {
            await _repository.UpdateSortOrderAsync(contentId, sortOrder, cancellation);
        }

        public async Task UpdateSectionContentItemsAsync(ICollection<ContentItem> contentItems, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(contentItems, nameof(contentItems));
            await _repository.UpdateSectionContentItemsAsync(contentItems, cancellation);
        }

        public async Task UpdateContentItemsAsync(IEnumerable<ContentItem> contentItems, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(contentItems, nameof(contentItems));
            await _repository.UpdateContentItemsAsync(contentItems, cancellation).ConfigureAwait(false);
        }

        public async Task SaveChangesAsync(CancellationToken cancellation = default)
        {
            await _repository.SaveChangeAsync(cancellation);
        }
    }
}