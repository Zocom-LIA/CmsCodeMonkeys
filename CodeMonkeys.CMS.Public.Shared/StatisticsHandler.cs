using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;

namespace CodeMonkeys.CMS.Public.Shared
{
    public class StatisticsHandler
    {
        private DbContext _context;
        private readonly DbSet<Statistics> _statistics;
        private readonly ILogger _logger;

        public StatisticsHandler(ApplicationDbContext context, ILogger<StatisticsHandler> logger)
        {
            _context = context;
            _logger = logger;
            _statistics = context.Set<Statistics>();
        }

        public async Task<int> GetAndUpdatePageVisits(string pageUrl)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync().ConfigureAwait(false))
            {
                try
                {
                    // TODO: Handle transactions properly by using a transaction scope and savepoints
                    _logger.LogInformation($"StatisticsHandler: Updating page visits for {pageUrl}");
                    var visits = await _statistics.Where(s => s.PageUrl.Equals(pageUrl)).FirstOrDefaultAsync();
                    if (visits == null)
                    {
                        visits = new Statistics
                        {
                            PageUrl = pageUrl,
                            PageVisits = 1
                        };
                        await _statistics.AddAsync(visits);
                    }
                    else
                    {
                        visits.PageVisits++;
                        _statistics.Update(visits);
                    }

                    _logger.LogInformation($"StatisticsHandler: Updated visits: {visits.PageVisits}");

                    await _context.SaveChangesAsync();

                    return visits.PageVisits;
                }
                catch (Exception ex)
                {
                    // Log the error and rethrow the exception
                    _logger.LogError(ex, "An error occurred while updating page visits.");
                    throw;
                }
            }
        }

        public async Task<int> GetPageVisits(string pageUrl)
        {
            _logger.LogInformation($"StatisticsHandler: Getting page visits for {pageUrl}");
            var visits = await _statistics.Where(s => s.PageUrl.Equals(pageUrl)).FirstOrDefaultAsync();
            _logger.LogInformation($"StatisticsHandler: Page visits: {visits?.PageVisits ?? 0}");
            return visits?.PageVisits ?? 0;
        }
    }
}