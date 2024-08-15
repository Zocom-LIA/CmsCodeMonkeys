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
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
            }
            finally
            {
                try
                {
                    var header = context.Response.Headers["Content-Type"].ToString();
                    if (string.IsNullOrEmpty(header) || header.Contains("text/html"))
                    {
                        var pageUrl = context.Request.Path.Value;

                        if (pageUrl != null)
                        {
                            await _repository.UpdatePageCountAsync(pageUrl);
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}