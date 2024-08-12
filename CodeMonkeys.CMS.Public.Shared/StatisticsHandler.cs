using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;

namespace CodeMonkeys.CMS.Public.Shared
{
    public class StatisticsHandler(ApplicationDbContext context)
    {

        private DbContext _context = context;

        public async Task<int> GetAndUpdatePageVisits(string pageUrl)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync().ConfigureAwait(false))
            {
                try
                {
                    var visits = await _context.Set<Statistics>().FirstOrDefaultAsync(page => page.PageUrl == pageUrl);

                    if (visits == null) return 1;

                    visits.PageVisits++;

                    var result = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return visits.PageVisits + 1;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new DbUpdateException("Error updating page visits", ex);
                }
            }
        }
    }
}