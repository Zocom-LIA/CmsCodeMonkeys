using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class RepositoryBase
    {
        public IDbContextFactory<ApplicationDbContext> ContextFactory { get; }
        protected readonly ILogger _logger;

        public RepositoryBase(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            ILogger logger)
        {
            ContextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected async Task<TResult?> ExecuteAsync<TResult>(Func<ApplicationDbContext, CancellationToken, Task<TResult>> operation, CancellationToken cancellation = default)
        {
            var context = ContextFactory.CreateDbContext();

            TResult? result;
            try
            {
                result = await operation(context, cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the operation.");
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }

            return result;
        }

        protected async Task<User?> GetUserAsync(ApplicationDbContext context, Guid? id)
        {
            return await context.Users.FindAsync(id);
        }

        protected ApplicationDbContext GetContext()
        {
            return ContextFactory.CreateDbContext();
        }
    }
}