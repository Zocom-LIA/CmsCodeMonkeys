
using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IContentRepository
    {
        Task<Content> CreateContentAsync(Content content);
        Task<Content?> DeleteContentAsync(int contentId);
        Task<IEnumerable<Content>> GetWebPageContentsAsync(int pageId, int pageIndex = 0, int pageSize = 10);
        Task<IEnumerable<Content>> UpdateContentsAsync(WebPage webPage, IEnumerable<Content> contents);
        Task<IEnumerable<Content>> UpdateOrdinalNumbersAsync(ICollection<Content> contents, bool persist);
    }
}