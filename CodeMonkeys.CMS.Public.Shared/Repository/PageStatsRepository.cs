using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class PageStatsRepository : IPageStatsRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly ILogger _logger;

        public IDbContextFactory<ApplicationDbContext> ContextFactory => _contextFactory;

        public PageStatsRepository(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<PageStatsRepository> logger)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory), "The context factory must not return a null context.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task UpdatePageCountAsync(int siteId, int pageId, string pageUrl)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var pageStats = context.PageStats;
                        var visits = await pageStats.AsNoTracking()
                                                    .Where(s => s.PageUrl.Equals(pageUrl))
                                                    // A URL that has been reassigned from one page to another should get a new counter.
                                                    .Where(s => s.PageId.Equals(pageId))
                                                    .FirstOrDefaultAsync();

                        if (visits == null)
                        {
                            visits = new PageStats
                            {
                                PageUrl = pageUrl,
                                SiteId = siteId,
                                PageId = pageId,
                                PageVisits = 1
                            };
                            await pageStats.AddAsync(visits);
                        }
                        else
                        {
                            visits.PageVisits++;
                            pageStats.Update(visits);
                        }

                        _logger.LogInformation($"StatisticsHandler: Updated visits: {visits.PageVisits}");

                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while updating the page count.");
                        await transaction.RollbackAsync();
                        throw new RepositoryException("Error when updating the page count.", ex);
                    }
                }
            }
        }
        public async Task<int> GetPageVisitsAsync(string pageUrl)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var visits = await context.PageStats.AsNoTracking().Where(s => s.PageUrl.Equals(pageUrl)).FirstOrDefaultAsync();
                return visits?.PageVisits ?? 0;
            }
        }

        public async Task<IEnumerable<PageStats>> GetPageStatsAsync(int siteId)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.PageStats.AsNoTracking().Where(s => s.SiteId == siteId).ToListAsync();
            }
        }
    }
}