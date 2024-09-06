using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Repository;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public class ContentService : IContentService
    {
        public ContentService(IContentRepository repository)
        {
            Repository = repository;
        }

        public IContentRepository Repository { get; }

        public async Task<Content> CreateContentAsync(Content content)
        {
            return await Repository.CreateContentAsync(content);
        }

        public Task DeleteContentAsync(int contentId)
        {
            return Repository.DeleteContentAsync(contentId);
        }

        public async Task<IEnumerable<Content>> GetWebPageContentsAsync(int pageId, int pageIndex = 0, int pageSize = 10)
        {
            if (pageIndex < 0) throw new ArgumentOutOfRangeException("PageIndex must be a positive number.");
            if (pageSize <= 0) throw new ArgumentOutOfRangeException("PageSize must be greater than zero.");

            return await Repository.GetWebPageContentsAsync(pageId, pageIndex, pageSize);
        }

        public async Task<IEnumerable<Content>> UpdateOrdinalNumberAsync(ICollection<Content> contents, bool persist = true)
        {
            return await Repository.UpdateOrdinalNumbersAsync(contents, persist);
        }
    }
}