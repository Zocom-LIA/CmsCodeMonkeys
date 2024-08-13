
namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IPageStatsRepository
    {
        Task<int> GetPageVisitsAsync(string pageUrl);
        Task UpdatePageCountAsync(string pageUrl);
    }
}