using AutoMapper;

using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public class SectionRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IMapper mapper, ILogger<SectionRepository> logger)
        : RepositoryBase(contextFactory, mapper, logger), ISectionRepository
    {
        public Task AddAsync(IEnumerable<Section> sections, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(sections, nameof(sections));

            var context = GetContext();

            try
            {
                context.Sections.AddRange(sections);
                return context.SaveChangesAsync(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding sections");
                throw;
            }
            finally
            {
                context.DisposeAsync();
            }
        }

        public async Task<Section?> CreateSectionAsync(int webPageId, string name, CancellationToken cancellation = default)
        {
            var context = GetContext();

            try
            {
                var section = new Section
                {
                    Name = name,
                    WebPageId = webPageId,
                    Color = "#fefefe"
                };

                return await CreateSectionAsync(section, cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding section");
                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<Section?> CreateSectionAsync(Section section, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(section, nameof(section));

            var context = GetContext();

            try
            {
                await context.Sections.AddAsync(section, cancellation);
                await context.SaveChangesAsync(cancellation);

                return section;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create section");

                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task DeleteSectionAsync(int id, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            var context = GetContext();

            try
            {
                var section = await context.Sections.FindAsync(id, cancellation);

                if (section is not null)
                {
                    context.Sections.Remove(section);
                    await context.SaveChangesAsync(cancellation);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete section");

                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public Task DeleteSectionsAsync(IEnumerable<Section> sections, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(sections, nameof(sections));

            var context = GetContext();

            try
            {
                context.Sections.RemoveRange(sections);
                return context.SaveChangesAsync(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting sections");

                throw;
            }
            finally
            {
                context.DisposeAsync();
            }
        }

        public async Task<Section?> GetSectionAsync(int sectionId, CancellationToken cancellation = default)
        {
            var context = GetContext();

            try
            {
                return await context.Sections.FindAsync(sectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get section with ID {0}", sectionId);

                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<Section?> GetSectionByNameAsync(int webPageId, string name, CancellationToken cancellation)
        {
            var context = GetContext();

            try
            {
                cancellation.ThrowIfCancellationRequested();

                // Check if webPageId is valid
                var pageExists = await context.Pages.FindAsync(new object[] { webPageId }, cancellation) != null;
                if (!pageExists)
                {
                    _logger.LogWarning("Web page ID {0} does not exist.", webPageId);
                    return null;
                }

                var section = await context.Sections
                    .Where(s => s.WebPageId == webPageId && s.Name == name)
                    .Include(s => s.ContentItems)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellation);

                if (section == null)
                {
                    _logger.LogInformation("No section found with name {0} for web page ID {1}.", name, webPageId);
                }

                return section;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Operation was canceled while fetching section with name {0}.", name);
                throw; // Rethrow if necessary
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Exception ({ex.GetType().Name}): {ex.Message}");
                _logger.LogError(ex, "Failed to get section with name {0} for web page ID {1}.", name, webPageId);
                throw; // Rethrow to let the caller handle it
            }
            finally
            {
                await context.DisposeAsync(); // Ensure proper disposal of the context
            }
        }


        public async Task<Dictionary<int, Section>> GetSectionsAsync(int webPageId, CancellationToken cancellation = default)
        {
            var context = GetContext();

            try
            {
                cancellation.ThrowIfCancellationRequested();
                return await context.Sections
                    .Where(s => s.WebPageId == webPageId)
                    .Include(s => s.ContentItems)
                    .AsNoTracking()
                .ToDictionaryAsync(source => source.SectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get sections for web page with ID {0}", webPageId);
                return new();
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task SaveSectionColorAsync(int sectionId, string color, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(color, nameof(color));

            var context = GetContext();

            try
            {
                var section = await context.Sections.FirstOrDefaultAsync(s => s.SectionId == sectionId, cancellation);

                if (section is not null)
                {
                    section.Color = color;
                    context.Sections.Update(section);
                    await context.SaveChangesAsync(cancellation);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving box color");

                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        public async Task<Section?> UpdateSectionAsync(Section section, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(section, nameof(section));

            var context = GetContext();

            try
            {
                cancellation.ThrowIfCancellationRequested();
                context.Sections.Update(section);
                await context.SaveChangesAsync(cancellation);

                return section;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update section");

                throw;
            }
            finally
            {
                await context.DisposeAsync();
            }
        }
    }
}