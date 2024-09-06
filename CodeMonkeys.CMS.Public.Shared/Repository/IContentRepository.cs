
using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IContentRepository
    {
        ApplicationDbContext Context { get; }
        Task DeleteContentAsync(int contentId);
        Task<IEnumerable<Content>> GetWebPageContentsAsync(int pageId, int pageIndex = 0, int pageSize = 10);
    }
}