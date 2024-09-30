using CodeMonkeys.CMS.Public.Components.Shared;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Services;

using Microsoft.AspNetCore.Components;

namespace CodeMonkeys.CMS.Public.Components.Pages.ContentItems
{
    public partial class ContentItems : AuthenticationBaseComponent<ContentItems>
    {
        [Parameter] public int WebPageId { get; set; }

        [Inject] IContentItemService ContentItemService { get; set; }
        [Inject] ISectionService SectionService { get; set; }

        private Dictionary<int, Section> _sections = new();

        private Section _section1;
        private Section _section2;
        private Section _section3;
        private Section _section4;
        private int _sectionId1;
        private int _sectionId2;
        private int _sectionId3;
        private int _sectionId4;


        protected override async Task OnInitializedAsync()
        {
            try
            {
                var page = await WebPageService.GetVisitorPageAsync(WebPageId);

                if (page == null)
                {
                    Logger.LogWarning("No web page found with ID {0}", WebPageId);
                    ErrorMessage = "No web page found with that ID. Please, try again!";
                    return;
                }

                _sections = await ContentItemService.GetSectionContentItemsAsync(WebPageId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading content items for web page {0}", WebPageId);
                _sections = new();
            }

            if (_sections.Count() == 0)
            {
                await ResetSectionsAsync();
            }

            await GetSectionsAsync();
        }

        private async Task ResetSectionsAsync()
        {
            await SectionService.DropWebPageSections(WebPageId);
            await GetSectionsAsync();
        }

        private async Task GetSectionsAsync(CancellationToken cancellation = default)
        {
            try
            {
                _sections = await ContentItemService.GetSectionContentItemsAsync(WebPageId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading content items for web page {0}", WebPageId);
                _sections = new();
            }
            finally
            {
                _section1 = GetNamedSection("Header", _sections)!;
                _section2 = GetNamedSection("Body", _sections)!;
                _section3 = new Section { SectionId = 0, Name = "Toolbar", Color = "#fefefe", WebPageId = WebPageId };
                _section4 = GetNamedSection("Footer", _sections)!;
                _sectionId1 = _section1.SectionId;
                _sectionId2 = _section2.SectionId;
                _sectionId3 = _section3.SectionId;
                _sectionId4 = _section4.SectionId;
                selectedList = _section1.SectionId;
                showColorPicker = false;
                currentBox = selectedList;
                selectedBox = _section1.Name;
            }
        }

        // private ContentItemStorage ContentItemStorage { get; set; }
        private string newContentItemText = string.Empty;
        private int selectedList = 1;

        private bool showColorPicker = false;
        private string selectedColor = "White";
        private int currentBox = 1;
        private string selectedBox = "Box 1";

        private Section? GetNamedSection(string name, Dictionary<int, Section> sections)
        {
            return sections.Values.FirstOrDefault(s => s.Name == name);
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
                if (_sections.TryGetValue(boxNumber, out var section))
                {
                    selectedBox = section.Name;
                    selectedColor = section.Color;
                }
                else
                {
                    selectedBox = "Box 1";
                    selectedColor = "#fefefe";
                }

                showColorPicker = true;
            }
        }

        private async Task SaveColorAsync()
        {
            await SectionService.SaveSectionColorAsync(currentBox, selectedColor);
            showColorPicker = false;
            StateHasChanged();
        }

        private async Task AddContentItem()
        {
            if (!string.IsNullOrWhiteSpace(newContentItemText))
            {
                await ContentItemService.AddContentItemAsync(selectedList, newContentItemText);
                newContentItemText = string.Empty;

                // Kontrollera och ställ in ShowEditButton endast om listan inte är tom
                ResetShowEditButton(_section1);
                ResetShowEditButton(_section2);
                ResetShowEditButton(_section3);
                ResetShowEditButton(_section4);
                StateHasChanged();
            }
        }

        private void ResetShowEditButton(Section section1)
        {
            if (section1.ContentItems.Any())
            {
                section1.ContentItems.Last().ShowEditButton = false;
            }
        }

        private void OnDragStart(ContentItem contentItem)
        {
            ContentItemService.StartDrag(contentItem);
        }

        private void OnDrop(int targetListNumber)
        {
            ContentItemService.DropContentItemAsync(targetListNumber).Wait();
            GetSectionsAsync().Wait();
            StateHasChanged();
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

        private void SaveEdit(ContentItem contentItem)
        {
            contentItem.IsEditing = false; // Save the edit and close the input field
            StateHasChanged();
        }

        private async Task RemoveContentItemAsync(ContentItem contentItem)
        {
            await ContentItemService.RemoveContentItemAsync(contentItem);
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

        private void ClearAll()
        {
            _sections = new();

            StateHasChanged();
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
}