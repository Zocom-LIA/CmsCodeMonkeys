
namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface IPageStatsService
    {
        Task<int> GetPageVisitsAsync(string pageUrl);
    }
}