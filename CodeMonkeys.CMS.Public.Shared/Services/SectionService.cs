using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Repository;

using Microsoft.Identity.Client.Extensions.Msal;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public class SectionService(ISectionRepository repository) : ISectionService
    {
        private readonly ISectionRepository _repository = repository;

        public async Task<Dictionary<int, Section>> GetSectionsAsync(int webPageId, CancellationToken cancellation = default)
        {
            return await _repository.GetSectionsAsync(webPageId, cancellation);
        }

        public async Task<Section?> GetSectionAsync(int sectionId, CancellationToken cancellation = default)
        {
            return await _repository.GetSectionAsync(sectionId, cancellation);
        }

        public async Task<Section?> CreateSectionAsync(Section section, CancellationToken cancellation = default)
        {
            return await _repository.CreateSectionAsync(section, cancellation);
        }

        public async Task<Section?> UpdateSectionAsync(Section section, CancellationToken cancellation = default)
        {
            return await _repository.UpdateSectionAsync(section, cancellation);
        }

        public async Task DeleteSectionAsync(int id, CancellationToken cancellation = default)
        {
            await _repository.DeleteSectionAsync(id, cancellation);
        }

        public Task AddAsync(IEnumerable<Section> sections, CancellationToken cancellation = default)
        {
            return _repository.AddAsync(sections, cancellation);
        }

        public async Task SaveSectionColorAsync(int boxNumber, string color, CancellationToken cancellation = default)
        {
            await _repository.SaveSectionColorAsync(boxNumber, color, cancellation);
        }

        public async Task DropWebPageSectionsAsync(int webPageId, CancellationToken cancellation = default)
        {
            var sections = (await GetSectionsAsync(webPageId, cancellation)).Select(kvp => kvp.Value);

            await _repository.DeleteSectionsAsync(sections, cancellation);
        }

        public async Task<Section?> GetSectionByNameAsync(int webPageId, string name, CancellationToken cancellation = default)
        {
            return await _repository.GetSectionByNameAsync(webPageId, name, cancellation);
        }
    }
}