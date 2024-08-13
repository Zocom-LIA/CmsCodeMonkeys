using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CodeMonkeys.CMS.Public.Shared.Middleware
{
    public class VisitCounterMiddleware : IMiddleware
    {
        private readonly IPageStatsRepository _repository;
        private readonly ILogger _logger;

        public VisitCounterMiddleware(IPageStatsRepository repository, ILogger<VisitCounterMiddleware> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                var pageUrl = context.Request.Path.Value;

                if (pageUrl != null)
                {
                    await _repository.UpdatePageCountAsync(pageUrl);
                }

                await next(context);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}