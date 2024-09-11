using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Repository;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface IContentService
    {
        IContentRepository Repository { get; }

        Task<Content> CreateContentAsync(Content content);
        Task DeleteContentAsync(int contentId);
        Task<IEnumerable<Content>> GetWebPageContentsAsync(int pageId, int pageIndex = 0, int pageSize = 10);
        Task<IEnumerable<Content>> UpdateOrdinalNumberAsync(ICollection<Content> contents, bool persist = false);
    }
}