using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface ISectionService
    {
        Task AddAsync(IEnumerable<Section> sections, CancellationToken cancellation = default);
        Task<Section?> CreateSectionAsync(Section section, CancellationToken cancellation = default);
        Task DeleteSectionAsync(int id, CancellationToken cancellation = default);
        Task DropWebPageSections(int webPageId, CancellationToken cancellation = default);
        Task<Section?> GetSectionAsync(int sectionId, CancellationToken cancellation = default);
        Task<IEnumerable<Section>> GetSectionsAsync(int webPageId, CancellationToken cancellation = default);
        Task SaveSectionColorAsync(int boxNumber, string color, CancellationToken cancellation = default);
        Task<Section?> UpdateSectionAsync(Section section, CancellationToken cancellation = default);
    }
}