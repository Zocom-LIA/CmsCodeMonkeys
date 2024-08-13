﻿using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Repository;
using Microsoft.Extensions.Logging;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public class PageStatsService : IPageStatsService
    {
        private readonly IPageStatsRepository _repository;
        private readonly ILogger _logger;

        public PageStatsService(IPageStatsRepository repository, ILogger<PageStatsService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> GetPageVisitsAsync(string pageUrl)
        {
            return await _repository.GetPageVisitsAsync(pageUrl);
        }
    }
}