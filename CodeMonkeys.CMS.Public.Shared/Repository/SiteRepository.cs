﻿using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;

using System.Text;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class SiteRepository : ISiteRepository
    {
        public ApplicationDbContext Context { get; }
        public SiteRepository(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException("context");
        }

        public async Task CreateAsync(Site site)
        {
            Context.Sites.Add(site);
            await Context.SaveChangesAsync();
        }

        public async Task<Site?> GetSiteByNameAsync(string name)
        {
            return await Context.Sites.Include(site => site.LandingPage).ThenInclude(page => page!.Contents).FirstOrDefaultAsync(site => site.Name == name);
        }

        public async Task<IEnumerable<Site>> GetUserSitesAsync(Guid userId, int pageIndex = 0, int pageSize = 10)
        {
            if (pageIndex < 0) throw new ArgumentOutOfRangeException("PageIndex must be a positive number.");
            if (pageSize <= 0) throw new ArgumentOutOfRangeException("PageSize must be greater than zero.");

            return await Context.Sites
                .Where(site => site.CreatorId.Equals(userId))
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .OrderBy(site => site.CreatedDate)
                .Include(site => site.LandingPage)
                .Include(site => site.Pages)
                .ThenInclude(page => page.Contents)
                .Include(site => site.Creator)
                .ToListAsync();
        }

        public Task<Site?> GetUserSiteAsync(Guid userId, int siteId) => Context.Sites.Include(site => site.LandingPage).Include(site => site.Pages).FirstOrDefaultAsync(site => site.SiteId == siteId && site.CreatorId.Equals(userId));

        public Task UpdateSiteAsync(Site site)
        {
            Context.Sites.Update(site);
            return Context.SaveChangesAsync();
        }

        // Used for testing purposes only
        public Task<Site?> GetSiteAsync(int siteId)
        {
            return Context.Sites.Include(site => site.LandingPage).Include(site => site.Pages).FirstOrDefaultAsync(site => site.SiteId == siteId);
        }
    }
}