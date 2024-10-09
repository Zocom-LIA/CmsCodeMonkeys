using AutoMapper;

using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface ISectionRepository
    {
        Task AddAsync(IEnumerable<Section> sections, CancellationToken cancellation);
        Task<Section?> CreateSectionAsync(Section section, CancellationToken cancellation);
        Task<Section?> CreateSectionAsync(int webPageId, string name, CancellationToken cancellation = default);
        Task DeleteSectionAsync(int id, CancellationToken cancellation);
        Task DeleteSectionsAsync(IEnumerable<Section> sections, CancellationToken cancellation);
        Task<Section?> GetSectionAsync(int sectionId, CancellationToken cancellation);
        Task<Section?> GetSectionByNameAsync(int webPageId, string name, CancellationToken cancellation);
        Task<Dictionary<int, Section>> GetSectionsAsync(int webPageId, CancellationToken cancellation);
        Task SaveSectionColorAsync(int boxNumber, string color, CancellationToken cancellation);
        Task<Section?> UpdateSectionAsync(Section section, CancellationToken cancellation);
    }
}