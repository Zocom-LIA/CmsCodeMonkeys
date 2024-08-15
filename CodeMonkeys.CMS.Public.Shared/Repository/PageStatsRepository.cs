using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class PageStatsRepository : IPageStatsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<PageStats> _pageStats;
        private readonly ILogger _logger;

        public PageStatsRepository(ApplicationDbContext context, ILogger<PageStatsRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pageStats = _context.Set<PageStats>()
                ?? throw new InvalidOperationException("PageStats table is missing in the database. Add a PageStats table in the database, please.");
        }

        public async Task UpdatePageCountAsync(string pageUrl)
        {
            // TODO: Exception handling
            // TODO: Transaction handling
            var visits = await _pageStats.Where(s => s.PageUrl.Equals(pageUrl)).FirstOrDefaultAsync();

            if (visits == null)
            {
                visits = new PageStats
                {
                    PageUrl = pageUrl,
                    PageVisits = 1
                };
                await _pageStats.AddAsync(visits);
            }
            else
            {
                visits.PageVisits++;
                _pageStats.Update(visits);
            }

            _logger.LogInformation($"StatisticsHandler: Updated visits: {visits.PageVisits}");

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetPageVisitsAsync(string pageUrl)
        {
            // TODO: Exception Handling
            return (await _pageStats.Where(page => page.PageUrl.Equals(pageUrl)).FirstOrDefaultAsync())?.PageVisits ?? 0;
        }

        public async Task<IEnumerable<PageStats>> GetPageStatsAsync() => await _pageStats.ToListAsync();
    }
}