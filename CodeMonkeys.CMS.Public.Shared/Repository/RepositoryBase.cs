using AutoMapper;

using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class RepositoryBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;

        public RepositoryBase(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            IMapper mapper,
            ILogger logger)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected async Task<User?> GetUserAsync(ApplicationDbContext context, Guid? id)
        {
            return await context.Users.FindAsync(id);
        }

        protected ApplicationDbContext GetContext()
        {
            return _contextFactory.CreateDbContext();
        }
    }
}