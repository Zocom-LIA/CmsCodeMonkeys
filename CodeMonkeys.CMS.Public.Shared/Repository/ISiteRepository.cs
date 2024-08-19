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
        public Task<Site?> GetSiteByNameAsync(string name);
    }
}
