using CodeMonkeys.CMS.Public.Shared.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface ISiteRepository
    {
        Task<Site> CreateAsync(Site site, Guid? creatorId = null, CancellationToken cancellation = default);
        Task UpdateSiteAsync(Site site, CancellationToken cancellation = default);
        Task<Site?> GetUserSiteAsync(Guid userId, int siteId, CancellationToken cancellation = default);
        Task<Site?> GetSiteWithContentsAsync(int siteId, CancellationToken cancellation = default);
        Task<IEnumerable<Site>> GetUserSitesAsync(Guid userId, int pageIndex = 0, int pageSize = 10, CancellationToken cancellation = default);
        Task DeleteSiteAsync(Site site, CancellationToken cancellation = default);
        Task<Site?> GetSiteAsync(int siteId, bool includeLandingPage = false, bool includePages = false, bool includeSections = false, bool includeContents = false, bool includeAuthor = false, CancellationToken cancellation = default);
    }
}