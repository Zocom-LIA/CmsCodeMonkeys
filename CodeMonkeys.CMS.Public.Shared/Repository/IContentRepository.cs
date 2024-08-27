﻿
using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IContentRepository
    {
        Task<IEnumerable<Content>> GetWebPageContentsAsync(int pageId, int pageIndex = 0, int pageSize = 10);
    }
}