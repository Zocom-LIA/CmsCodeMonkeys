using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface IContentItemService
    {
        Task AddContentItemAsync(ContentItem contentItem);
        Task DeleteContentItemAsync(ContentItem contentItem);
        Task DropContentItemAsync(int webPageId, int newSectionId);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<ContentItem> contentItems);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<int> contentItemIds);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(IEnumerable<Section> sections);
        Task<IEnumerable<ContentItem>> GetContentItemsAsync(int sectionId);
        Task<Dictionary<int, Section>> GetSectionContentItemsAsync(int webPageId);
        Task RemoveContentItemAsync(ContentItem contentItem);
        void StartDrag(ContentItem contentItem);
        Task UpdateContentItemAsync(ContentItem contentItem);
    }
}