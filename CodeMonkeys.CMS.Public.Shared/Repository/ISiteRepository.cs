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
        Task<Site> CreateAsync(Site site, Guid? creatorId = null);
        Task UpdateSiteAsync(Site site);
        Task<Site?> GetUserSiteAsync(Guid userId, int siteId);
        Task<Site?> GetSiteWithContentsAsync(int siteId);
        Task<IEnumerable<Site>> GetUserSitesAsync(Guid userId, int pageIndex = 0, int pageSize = 10);
        Task DeleteSiteAsync(Site site);
        Task<Site?> GetSiteAsync(int siteId);
    }
}