using AutoMapper;

using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class SiteRepository : RepositoryBase, ISiteRepository
    {
        public SiteRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IMapper mapper, ILogger<SiteRepository> logger) : base(contextFactory, mapper, logger) { }

        public async Task<Site> CreateAsync(Site site, Guid? creatorId = null)
        {
            ArgumentNullException.ThrowIfNull(site, nameof(site));


            if (string.IsNullOrWhiteSpace(site.Name))
            {
                throw new ArgumentException("Site must have a valid name.", nameof(site));
            }

            var context = GetContext();

            try
            {
                var assignedCreatorId = creatorId ?? site.CreatorId ?? site.Creator?.Id;

                site.CreatedDate = DateTime.Now;
                site.LastModifiedDate = DateTime.Now;
                site.CreatorId = assignedCreatorId;

                await context.Sites.AddAsync(site);
                await context.SaveChangesAsync();
                return site;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new site.");
                throw new RepositoryException("Error when creating a site.", ex);
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task UpdateSiteAsync(Site site)
        {
            if (site == null) throw new ArgumentNullException(nameof(site), "Site must not be null.");

            var context = GetContext();

            try
            {
                EntityEntry<Site> entry = context.Sites.Update(site);
                await context.SaveChangesAsync();

                site.LandingPage = entry.Entity.LandingPage;
                site.Creator = entry.Entity.Creator;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the site.");
                throw new RepositoryException("Error when updating the site.", ex);
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task DeleteSiteAsync(Site site)
        {
            if (site == null) throw new ArgumentNullException(nameof(site), "Site must not be null.");

            var context = GetContext();

            try
            {
                context.Sites.Remove(site);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the site.");
                throw new RepositoryException("Error when deleting the site.", ex);
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<Site?> GetSiteWithContentsAsync(int siteId)
        {
            var context = GetContext();
            Site? site = null;

            try
            {
                site = await context.Sites
                    .Include(site => site.Pages)
                    .ThenInclude(page => page.Contents)
                    .Include(site => site.LandingPage)
                    .ThenInclude(page => page!.Contents)
                    .Include(site => site.Creator)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(site => site.SiteId == siteId);
            }
            finally
            {
                await context.DisposeAsync();
            }

            return site;
        }

        public async Task<IEnumerable<Site>> GetUserSitesAsync(Guid userId, int pageIndex = 0, int pageSize = 10)
        {
            if (pageIndex < 0) throw new ArgumentOutOfRangeException("PageIndex must be a positive number.");
            if (pageSize <= 0) throw new ArgumentOutOfRangeException("PageSize must be greater than zero.");

            var context = GetContext();
            IEnumerable<Site>? sites = Enumerable.Empty<Site>();

            try
            {
                sites = await context.Sites
                    .Where(site => site.CreatorId.Equals(userId))
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(site => site.CreatedDate)
                    .Include(site => site.LandingPage)
                    .Include(site => site.Pages)
                    .ThenInclude(page => page.Contents)
                    .Include(site => site.Creator)
                    .AsNoTracking()
                    .ToListAsync();
            }
            finally
            {
                await context.DisposeAsync();
            }

            return sites;
        }

        public async Task<Site?> GetUserSiteAsync(Guid userId, int siteId)
        {
            if (userId == Guid.Empty) throw new ArgumentException("UserId must not be empty.", nameof(userId));

            var context = GetContext();
            Site? site = null;

            try
            {
                site = await context.Sites
                    .Include(site => site.LandingPage)
                    .Include(site => site.Pages)
                    .ThenInclude(page => page.Contents)
                    .Include(site => site.Creator)
                    .FirstOrDefaultAsync(site => site.CreatorId.Equals(userId) && site.SiteId == siteId);
            }
            finally
            {
                await context.DisposeAsync();
            }

            return site;
        }

        public async Task<Site?> GetSiteAsync(int siteId)
        {
            var context = GetContext();
            Site? site = null;

            try
            {
                site = await context.Sites
                    .Include(site => site.LandingPage)
                    .Include(site => site.Pages)
                    .ThenInclude(page => page.Contents)
                    .Include(site => site.Creator)
                    .FirstOrDefaultAsync(site => site.SiteId == siteId);
            }
            finally
            {
                await context.DisposeAsync();
            }

            return site;
        }
    }
}