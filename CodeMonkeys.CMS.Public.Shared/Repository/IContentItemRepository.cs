using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IContentItemRepository
    {
        Task AddContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default);
        Task DeleteContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default);
        Task<ContentItem?> GetContentItemAsync(int contentId, CancellationToken cancellation = default);
        Task<ContentItem?> GetContentItemByIdAsync(int contentId, CancellationToken cancellation = default);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<ContentItem> contentItems, CancellationToken cancellation = default);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<int> contentIds, CancellationToken cancellation = default);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<Section> sections, CancellationToken cancellation = default);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(int sectionId, CancellationToken cancellation = default);
        Task<Dictionary<int, Section>> GetSectionContentItemsAsync(int webPageId, CancellationToken cancellation = default);
        Task UpdateContentItemAsync(ContentItem contentItem, CancellationToken cancellation = default);
        Task UpdateSortOrderAsync(int contentId, int sortOrder, CancellationToken cancellation = default);
    }
}