using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface IContentItemService
    {
        ContentItem? DraggedContentItem { get; }

        Task AddContentItemAsync(ContentItem contentItem);
        Task DeleteContentItemAsync(ContentItem contentItem);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<ContentItem> contentItems);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<int> contentIds);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<Section> sections);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(int sectionId);
        Task<Dictionary<int, Section>> GetSectionContentItemsAsync(int webPageId, CancellationToken cancellation = default);
        Task MoveContentItemAsync(int newSectionId, CancellationToken cancellation = default);
        Task RemoveContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default);
        void StartDrag(ContentItem contentItem, CancellationToken cancellation = default);
        Task UpdateContentItemAsync(ContentItem contentItem);
        Task UpdateSectionIdAsync(int ContentId, int newSectionId, CancellationToken cancellation = default);
        Task UpdateSortOrderAsync(int contentId, int sortOrder, CancellationToken cancellation = default);
    }
}