using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface IContentItemService
    {
        ContentItem? DraggedContentItem { get; }

        Task AddContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default);
        Task DeleteContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<ContentItem> contentItems, CancellationToken cancellation = default);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<int> contentIds, CancellationToken cancellation = default);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<Section> sections, CancellationToken cancellation = default);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(int sectionId, CancellationToken cancellation = default);
        Task<Dictionary<int, Section>> GetSectionContentItemsAsync(int webPageId, CancellationToken cancellation = default);
        Task MoveContentItemAsync(int newSectionId, CancellationToken cancellation = default);
        Task RemoveContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default);
        Task SaveChangesAsync(CancellationToken cancellation = default);
        void StartDrag(ContentItem contentItem);
        Task UpdateContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default);
        Task UpdateContentItemsAsync(IEnumerable<ContentItem> modifiedContentItems, CancellationToken cancellation = default);
        Task UpdateSectionContentItemsAsync(ICollection<ContentItem> contentItems, CancellationToken cancellation = default);
        Task UpdateSectionIdAsync(int ContentId, int newSectionId, CancellationToken cancellation = default);
        Task UpdateSortOrderAsync(int contentId, int sortOrder, CancellationToken cancellation = default);
    }
}