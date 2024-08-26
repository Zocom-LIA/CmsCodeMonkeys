using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class SiteRepository : ISiteRepository
    {
        private readonly ApplicationDbContext _context;
        public SiteRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException("context");
        }

        public async Task<Site?> GetSiteByNameAsync(string name)
        {
            return await _context.Sites.Include(site => site.LandingPage).ThenInclude(page => page!.Contents).FirstOrDefaultAsync(site => site.Name == name);
        }
    }
}
