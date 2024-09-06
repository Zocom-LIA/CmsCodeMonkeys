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
        Task CreateAsync(Site site);
        Task UpdateSiteAsync(Site site);
        Task<Site?> GetUserSiteAsync(Guid userId, int siteId);
        Task<Site?> GetSiteByNameAsync(string name);
        Task<IEnumerable<Site>> GetUserSitesAsync(Guid userId, int pageIndex = 0, int pageSize = 10);
        Task<Site?> GetSiteAsync(int siteId);
    }
}