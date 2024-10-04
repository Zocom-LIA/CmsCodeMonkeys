using CodeMonkeys.CMS.Public.Components.Shared;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Services;

using Microsoft.AspNetCore.Components;

using System.Runtime.CompilerServices;

namespace CodeMonkeys.CMS.Public.Components.Pages.Sites.WebPages.ContentItems
{
    public partial class ContentItems : AuthenticationBaseComponent<ContentItems>
    {
        [Parameter] public int WebPageId { get; set; }
        public WebPage WebPage { get; set; }

        [Parameter] public int SiteId { get; set; }
        public WebPage Site { get; set; }

        [Inject] IContentItemService ContentItemService { get; set; }
        [Inject] ISectionService SectionService { get; set; }

        private IEnumerable<Section> _sections = [];
        private Section _contentHeader;
        private Section _contentBody;
        private Section _contentFooter;
        private Section _toolbar;

        private string newContentItemText = string.Empty;
        private int selectedList;

        private bool showColorPicker = false;
        private string selectedColor = "White";
        private int currentBox;
        private string selectedBox = "Box 1";

        protected override async Task OnInitializedAsync()
        {
            await ExecuteWithLoadingAsync(async () =>
            {
                await base.OnInitializedAsync();
                try
                {
                    _sections = (await SectionService.GetSectionsAsync(WebPageId)).Select(kvp => kvp.Value);
                    await LoadSectionsAsync();
                    selectedList = _contentHeader.SectionId;
                }
                catch (Exception ex)
                {
                    LogError(new { Method = nameof(this.OnInitializedAsync), SiteId = SiteId, WebPageId = WebPageId },
                        "Error loading content items for web page.", ex);
                }
            });
        }

        private async Task LoadSectionsAsync()
        {
            try
            {
                _contentHeader = await EnsureSectionAsync(SectionNames.Header.ToString());
                _contentBody = await EnsureSectionAsync(SectionNames.Body.ToString());
                _contentFooter = await EnsureSectionAsync(SectionNames.Footer.ToString());
                _toolbar = new Section { SectionId = 0, Name = "Toolbar", Color = "#fefefe", WebPageId = WebPageId };

                _sections = [_contentHeader, _contentBody, _contentFooter];
            }
            catch (Exception ex)
            {
                LogError(new { Method = nameof(this.LoadSectionsAsync), SiteId = SiteId, WebPageId = WebPageId },
                    "Error loading sections for web page.", ex);
            }
        }

        private async Task<Section> EnsureSectionAsync(string name)
        {
            try
            {
                var section = await SectionService.GetSectionByNameAsync(WebPageId, name);
                if (section == null)
                {
                    section = new Section { Name = name, Color = "#fefefe", WebPageId = WebPageId };
                    section = await SectionService.CreateSectionAsync(section)
                        ?? throw new InvalidOperationException($"Error creating section: {name}");
                }
                return section;
            }
            catch (Exception ex)
            {
                LogError(new { Method = nameof(this.EnsureSectionAsync), SiteId = SiteId, WebPageId = WebPageId, SectionName = name },
                    "Error ensuring section for web page.", ex);
                throw;
            }
        }

        private async Task ResetSectionsAsync()
        {
            await SectionService.DropWebPageSectionsAsync(WebPageId);
            await LoadSectionsAsync();
        }

        private void OpenColorPicker(int boxNumber)
        {
            if (currentBox == boxNumber && showColorPicker)
            {
                showColorPicker = false;
            }
            else
            {
                currentBox = boxNumber;
                var section = _sections.FirstOrDefault(section => section.SectionId == boxNumber);
                if (section != null)
                {
                    selectedBox = section.Name;
                    selectedColor = section.Color;
                    showColorPicker = true;
                }
                else
                {
                    LogWarning(new { Method = nameof(this.OpenColorPicker), SiteId = SiteId, WebPageId = WebPageId },
                        "Section with ID {0} does not exist.", boxNumber);
                    ErrorMessage = "Error loading color picker.";
                }
            }
        }

        private async Task SaveColorAsync()
        {
            if (TryGetSection(currentBox, out var section))
            {
                section.Color = selectedColor; // Uppdatera färgen lokalt också
                await SectionService.SaveSectionColorAsync(currentBox, selectedColor); // Spara i backend
                showColorPicker = false;
                await InvokeAsync(StateHasChanged); // Se till att uppdatera komponenten
            }
        }

        private async Task AddContentItem()
        {
            if (!string.IsNullOrWhiteSpace(newContentItemText))
            {
                if (TryGetSection(selectedList, out var section))
                {
                    section.ContentItems ??= new List<ContentItem>();
                    var contentItems = section.ContentItems;
                    var contentItem = new ContentItem
                    {
                        SectionId = selectedList,
                        Text = newContentItemText,
                        Title = newContentItemText,
                        Body = newContentItemText,
                        OrdinalNumber = contentItems.Count(),
                        AuthorId = User!.Id,
                        WebPageId = WebPageId
                    };

                    section.ContentItems.Add(contentItem);

                    await ContentItemService.AddContentItemAsync(contentItem);
                    newContentItemText = string.Empty;

                    // Kontrollera och ställ in ShowEditButton endast om listan inte är tom
                    ResetShowEditButton(_contentHeader);
                    ResetShowEditButton(_contentBody);
                    ResetShowEditButton(_toolbar);
                    ResetShowEditButton(_contentFooter);
                    StateHasChanged();
                }
                else
                {
                    LogWarning(new { Method = nameof(this.AddContentItem), SiteId = SiteId, WebPageId = WebPageId, SectionId = selectedList },
                        "Error adding content item to section.");
                }
            }
        }

        private void ResetShowEditButton(Section section1)
        {
            if (section1.ContentItems.Any())
            {
                section1.ContentItems.Last().ShowEditButton = false;
            }
        }

        private ContentItem draggedItem;

        private void OnDragStart(ContentItem contentItem)
        {
            draggedItem = contentItem;
            // Indicate the drag has started (this is optional, helps to visualize dragging)
            contentItem.IsDragging = true;
        }

        private void OnDragEnd(ContentItem contentItem)
        {
            contentItem.IsDragging = false; // Optionally indicate the drag has ended
            StateHasChanged();
        }

        private async Task UpdateContentItemSectionAsync(int ContentId, int newSectionId)
        {
            if (ContentId <= 0)
            {
                LogWarning(new { Method = nameof(this.UpdateContentItemSectionAsync), SiteId = SiteId, WebPageId = WebPageId },
                    "Invalid ContentItemId: {0}", ContentId);
                return; // Prevent further execution
            }

            // Call your service to update the section ID
            await ContentItemService.UpdateSectionIdAsync(ContentId, newSectionId);
        }

        private async Task OnDrop(int targetSectionId)
        {
            if (draggedItem == null)
            {
                Logger.LogWarning("No content item is being dragged.");
                return;
            }

            if (draggedItem.ContentId <= 0) // Kontrollera ContentId istället
            {
                LogWarning(new { Method = nameof(this.OnDrop), SiteId = SiteId, WebPageId = WebPageId },
                    "Attempting to drop an invalid content {0}.", draggedItem.ContentId);
                return; // Avbryt om ID är ogiltigt
            }

            var sourceSectionId = draggedItem.SectionId;

            if (TryGetSection(sourceSectionId, out var sourceSection) &&
                TryGetSection(targetSectionId, out var targetSection))
            {
                // Ta bort från källsektionen
                sourceSection.ContentItems.Remove(draggedItem);

                // Uppdatera SectionId för det dragna objektet
                draggedItem.SectionId = targetSectionId;

                // Lägg till i målsektionen
                targetSection.ContentItems.Add(draggedItem);

                // Uppdatera databasen via en liknande metod som AddContentItem
                await UpdateContentItemSectionAsync(draggedItem.ContentId, targetSectionId);

                // Uppdatera UI
                StateHasChanged();
            }
        }






        private void ToggleEdit(ContentItem contentItem)
        {
            contentItem.ShowEditButton = !contentItem.ShowEditButton; // Toggle the edit button visibility
            StateHasChanged();
        }

        private void StartEdit(ContentItem contentItem)
        {
            contentItem.IsEditing = true;
            contentItem.ShowEditButton = false; // Hide the "Ändra" button
            StateHasChanged();
        }

        private async Task SaveEdit(ContentItem contentItem)
        {
            contentItem.IsEditing = false; // Save the edit and close the input field
            await ContentItemService.UpdateContentItemAsync(contentItem);
            StateHasChanged();
        }

        private async Task RemoveContentItemAsync(ContentItem contentItem)
        {
            await ContentItemService.RemoveContentItemAsync(contentItem);
            await LoadSectionsAsync();
            StateHasChanged();
        }


        private async Task RemoveAllContentItemsAsync()
        {
            // Gå igenom alla sektioner och ta bort deras innehåll
            foreach (var section in _sections)
            {
                // Ta bort alla content items i sektionen
                var contentItems = section.ContentItems.ToList(); // Hämta en lista med innehållsobjekt
                foreach (var item in contentItems)
                {
                    await ContentItemService.RemoveContentItemAsync(item);
                }

                // Sätt färgen på sektionen till vit
                section.Color = "#FFFFFF"; // Sätt färgen till vit (hex-koden för vit)
                await SectionService.SaveSectionColorAsync(section.SectionId, section.Color);
            }

            await LoadSectionsAsync(); // Uppdatera sektionerna efter borttagningen
            StateHasChanged();
        }


        private void ChangeFontSize(ContentItem contentItem, int change)
        {
            contentItem.FontSize += change * 2; // Increase or decrease size by 10 pixels
            if (contentItem.FontSize < 10) // Minimum size
            {
                contentItem.FontSize = 10;
            }
            StateHasChanged();
        }

        private bool TryGetSection(int currentSection, out Section section)
        {
            var actualSection = _sections.FirstOrDefault(section => section.SectionId == currentSection);
            if (actualSection != null)
            {
                section = actualSection;
                return true;
            }

            LogWarning(new { Method = nameof(this.TryGetSection), SiteId = SiteId, WebPageId = WebPageId },
                "Section with ID {0} does not exist.", currentSection);
            ErrorMessage = "Error loading color picker.";

            section = null!;
            return false;
        }

        private Dictionary<string, string> colorOptions = new Dictionary<string, string>
            {
                { "Black", "#000000" },
                { "White", "#FFFFFF" },
                { "Red", "#FF0000" },
                { "Green", "#008000" },
                { "Blue", "#0000FF" },
                { "Yellow", "#FFFF00" },
                { "Orange", "#FFA500" },
                { "Purple", "#800080" },
                { "Pink", "#FFC0CB" },
                { "Brown", "#A52A2A" },
                { "Gray", "#808080" },
                { "Cyan", "#00FFFF" },
                { "Magenta", "#FF00FF" },
                { "LightBlue", "#ADD8E6" },
                { "DarkBlue", "#00008B" },
                { "DarkGreen", "#006400" },
                { "LightGreen", "#90EE90" },
                { "LightGray", "#D3D3D3" },
                { "Gold", "#FFD700" },
                { "Silver", "#C0C0C0" },
                { "Teal", "#008080" }
            };

        private Dictionary<string, string> fontFamilyOptions = new Dictionary<string, string>
            {
                { "Arial", "Arial, sans-serif" },
                { "Verdana", "Verdana, sans-serif" },
                { "Tahoma", "Tahoma, sans-serif" },
                { "Times New Roman", "'Times New Roman', serif" },
                { "Georgia", "Georgia, serif" },
                { "Courier New", "'Courier New', monospace" },
                { "Comic Sans MS", "'Comic Sans MS', cursive" },
                { "Impact", "Impact, sans-serif" },
                { "Lucida Console", "'Lucida Console', monospace" },
                { "Trebuchet MS", "'Trebuchet MS', sans-serif" },
                { "Palatino Linotype", "'Palatino Linotype', serif" },
                { "Garamond", "Garamond, serif" },
                { "Frank Ruhl Libre", "'Frank Ruhl Libre', serif" },
                { "Arial Black", "'Arial Black', sans-serif" },
                { "Futura", "Futura, sans-serif" },
                { "Droid Sans", "'Droid Sans', sans-serif" },
                { "Segoe UI", "'Segoe UI', sans-serif" },
                { "Roboto", "'Roboto', sans-serif" },
                { "Open Sans", "'Open Sans', sans-serif" },
                { "Montserrat", "'Montserrat', sans-serif" },
                { "Lato", "'Lato', sans-serif" }
            };
    }
    public enum SectionNames
    {
        Header,
        Body,
        Footer
    }
}