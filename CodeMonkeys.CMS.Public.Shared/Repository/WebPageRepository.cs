using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class WebPageRepository : IWebPageRepository
    {
        public WebPageRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public ApplicationDbContext Context { get; }

        public async Task CreateWebPageAsync(WebPage webPage)
        {
            Context.Pages.Add(webPage);

            await Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WebPage>> GetSiteWebPagesAsync(int siteId, int pageIndex = 0, int pageSize = 10)
        {
            if (pageIndex < 0) throw new ArgumentOutOfRangeException("PageIndex must be a positive number.");
            if (pageSize <= 0) throw new ArgumentOutOfRangeException("PageSize must be greater than zero.");

            return await Context.Pages.Where(page => page.SiteId == siteId).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        }
    }
}