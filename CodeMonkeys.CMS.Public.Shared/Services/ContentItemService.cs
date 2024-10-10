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
        public async Task AddContentItemAsync(ContentItem contentItem) => await _repository.AddContentItemAsync(contentItem);
        public async Task DeleteContentItemAsync(ContentItem contentItem) => await _repository.DeleteContentItemAsync(contentItem);
        public async Task UpdateContentItemAsync(ContentItem contentItem) => await _repository.UpdateContentItemAsync(contentItem);
        public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<ContentItem> contentItems) => await _repository.GetContentItemsAsync(contentItems);
        public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<int> contentIds) => await _repository.GetContentItemsAsync(contentIds);
        public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<Section> sections) => await _repository.GetContentItemsAsync(sections);
        public async Task<IEnumerable<ContentItem>> GetContentItemsAsync(int sectionId) => await _repository.GetContentItemsAsync(sectionId);

        // Get sections and their ContentItem items if they exist; otherwise, create a new section
        public async Task<Dictionary<int, Section>> GetSectionContentItemsAsync(int webPageId, CancellationToken cancellation = default)
        {
            return await _repository.GetSectionContentItemsAsync(webPageId, cancellation);
        }

        // Start dragging a ContentItem item
        public void StartDrag(ContentItem contentItem, CancellationToken cancellation = default)
        {
            DraggedContentItem = contentItem;
        }

        // Drop the dragged ContentItem item into the target list
        public async Task MoveContentItemAsync(int newSectionId, CancellationToken cancellation = default)
        {
            if (DraggedContentItem != null)
            {
                DraggedContentItem.SectionId = newSectionId;

                await _repository.UpdateContentItemAsync(DraggedContentItem);
                DraggedContentItem = null;
            }
        }

        public async Task RemoveContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default)
        {
            await _repository.DeleteContentItemAsync(contentItem, cancellation);
        }

        public async Task UpdateSectionIdAsync(int ContentId, int newSectionId, CancellationToken cancellation = default)
        {
            var contentItem = await _repository.GetContentItemByIdAsync(ContentId, cancellation);
            if (contentItem != null)
            {
                contentItem.SectionId = newSectionId;
                await _repository.UpdateContentItemAsync(contentItem);
            }
        }

        public async Task UpdateSortOrderAsync(int contentId, int sortOrder, CancellationToken cancellation = default)
        {
            await _repository.UpdateSortOrderAsync(contentId, sortOrder, cancellation);
        }
    }
}