using CodeMonkeys.CMS.Public.Components.Shared;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Services;

using Microsoft.AspNetCore.Components;

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

        private Dictionary<int, Section> _sections = new();
        private Section _section1;
        private Section _section2;
        private Section _section3;
        private Section _section4;
        private int _sectionId1;
        private int _sectionId2;
        private int _sectionId3;
        private int _sectionId4;

        // private ContentItemStorage ContentItemStorage { get; set; }
        private string newContentItemText = string.Empty;
        private int selectedList = 1;

        private bool showColorPicker = false;
        private string selectedColor = "White";
        private int currentBox = 1;
        private string selectedBox = "Box 1";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            try
            {
                _sections = await SectionService.GetSectionsAsync(WebPageId);
                await LoadSectionsAsync();
                selectedList = _section1.SectionId;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading content items for web page {0}", WebPageId);
            }
        }



        private async Task LoadSectionsAsync()
        {
            try
            {
                _section1 = await EnsureSectionAsync(SectionNames.Header.ToString());
                _section2 = await EnsureSectionAsync(SectionNames.Body.ToString());
                _section4 = await EnsureSectionAsync(SectionNames.Footer.ToString());
                _section3 = new Section { SectionId = 0, Name = "Toolbar", Color = "#fefefe", WebPageId = WebPageId };

                _sections = new()
            {
                { _section1.SectionId, _section1 },
                { _section2.SectionId, _section2 },
                { _section3.SectionId, _section3 },
                { _section4.SectionId, _section4 }
            };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading sections for web page {0}", WebPageId);
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
                Logger.LogError(ex, "Error ensuring section {0} for web page {1}", name, WebPageId);
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
            if (_sections.TryGetValue(currentBox, out var section))
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
                if (_sections.TryGetValue(selectedList, out var section))
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
                        SortOrder = contentItems.Count() + 1,
                        AuthorId = User!.Id,
                        WebPageId = WebPageId
                    };

                    section.ContentItems.Add(contentItem);

                    await ContentItemService.AddContentItemAsync(contentItem);
                    newContentItemText = string.Empty;

                    // Kontrollera och ställ in ShowEditButton endast om listan inte är tom
                    ResetShowEditButton(_section1);
                    ResetShowEditButton(_section2);
                    ResetShowEditButton(_section3);
                    ResetShowEditButton(_section4);
                    StateHasChanged();
                }
                else
                {
                    Logger.LogWarning("Section with ID {0} does not exist.", selectedList);
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
        private int _startPosition;

        private void OnDragStart(ContentItem contentItem, int startPosition = -1)
        {
            draggedItem = contentItem;
            _startPosition = startPosition;
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
                Logger.LogWarning("Invalid ContentId: {0}", ContentId);
                return; // Prevent further execution
            }

            // Call your service to update the section ID
            await ContentItemService.UpdateSectionIdAsync(ContentId, newSectionId);
        }

        public async Task OnDrop(ContentItem targetItem)
        {
            // Store the original section
            var itemToBeMoved = draggedItem;
            var originalSection = itemToBeMoved.Section;

            // Remove itemToBeMoved from its original section
            originalSection.ContentItems.Remove(itemToBeMoved);

            // Change the section of the item to the target section
            var targetSection = targetItem.Section;
            itemToBeMoved.SectionId = targetSection.SectionId;
            itemToBeMoved.Section = targetSection;

            // Find the position of the targetItem in the target section's ContentItems
            int targetIndex = targetSection.ContentItems
                                .OrderBy(ci => ci.SortOrder)
                                .ToList()
                                .IndexOf(targetItem);

            // Insert itemToBeMoved right after the targetItem
            var items = targetSection.ContentItems.ToList();
            items.Insert(targetIndex + 1, itemToBeMoved);

            targetSection.ContentItems = items;

            // Adjust SortOrder for the original section (spanning from 1 to ContentItems.Count)
            AdjustSortOrder(originalSection);

            // Adjust SortOrder for the target section (spanning from 1 to ContentItems.Count)
            AdjustSortOrder(targetSection);

            // Collect all modified content items from both sections
            var modifiedContentItems = originalSection.ContentItems
                                         .Concat(targetSection.ContentItems)
                                         .Distinct();

            // Call to the update service for modified content items
            await ContentItemService.SaveChangesAsync();
        }

        private void AdjustSortOrder(Section section)
        {
            // Reorder ContentItems by SortOrder and adjust their values to be sequential
            var orderedItems = section.ContentItems
                                      //.OrderBy(ci => ci.SortOrder)
                                      .ToList();
            for (int i = 0; i < orderedItems.Count; i++)
            {
                orderedItems[i].SortOrder = i + 1; // SortOrder should start from 1
            }
        }


        private async Task OnDrop3(ContentItem targetContentItem)
        {
            if (draggedItem == null)
            {
                Logger.LogWarning("No content item is being dragged.");
                return;
            }

            if (draggedItem.ContentId <= 0)
            {
                Logger.LogWarning("Invalid ContentId: {0}", draggedItem.ContentId);
                return;
            }

            // Remove the dragged item from its current section
            Section sourceSection = draggedItem.Section;
            if (sourceSection == null)
            {
                Logger.LogWarning("Invalid source section.");
                return;
            }

            // Determine the target section and its content items
            Section targetSection = targetContentItem.Section;
            var targetContentItems = targetSection.ContentItems.OrderBy(ci => ci.SortOrder).ToList();

            // Calculate the target position based on the target content item's order
            int targetPosition = targetContentItems.IndexOf(targetContentItem);

            // Set the dragged item's sort order
            draggedItem.SortOrder = targetPosition + 1;
            draggedItem.SectionId = targetSection.SectionId;

            // Adjust SortOrder in the source section if necessary
            if (sourceSection != targetSection)
            {
                sourceSection.ContentItems.Remove(draggedItem);
                var items = sourceSection.ContentItems.ToArray();

                // Update SortOrder for remaining items in the source section
                for (int i = 0; i < sourceSection.ContentItems.Count; i++)
                {
                    items[i].SortOrder = i + 1;
                }
            }

            // Insert the dragged item at the target position and adjust other item orders
            // Check if the dragged item is already in the target section
            if (targetContentItems.Contains(draggedItem))
            {
                // Remove the duplicate before inserting
                targetContentItems.Remove(draggedItem);
            }

            targetContentItems.Insert(targetPosition, draggedItem);

            for (int i = targetPosition + 1; i < targetContentItems.Count; i++)
            {
                targetContentItems[i].SortOrder++;
            }

            // Update the target section's content items
            targetSection.ContentItems = targetContentItems;
            await ContentItemService.UpdateSectionContentItemsAsync(targetSection.ContentItems);

            // Update the source section's content items only if it was modified
            if (sourceSection != targetSection)
            {
                await ContentItemService.UpdateSectionContentItemsAsync(sourceSection.ContentItems);
            }

            StateHasChanged();
        }

        private async Task OnDrop2(ContentItem targetContentItem)
        {
            if (draggedItem == null)
            {
                Logger.LogWarning("No content item is being dragged.");
                return;
            }

            if (draggedItem.ContentId <= 0)
            {
                Logger.LogWarning("Invalid ContentId: {0}", draggedItem.ContentId);
                return;
            }

            // Remove the dragged item from its current section
            Section sourceSection = draggedItem.Section;
            if (sourceSection == null)
            {
                Logger.LogWarning("Invalid source section.");
                return;
            }

            // Determine the target section and its content items
            Section targetSection = targetContentItem.Section;
            var targetContentItems = targetSection.ContentItems.OrderBy(ci => ci.SortOrder).ToList();

            // Calculate the target position based on the target content item's order
            int targetPosition = targetContentItems.IndexOf(targetContentItem);

            // Set the dragged item's sort order
            draggedItem.SortOrder = targetPosition + 1;
            draggedItem.SectionId = targetSection.SectionId;

            // Adjust SortOrder in the source section
            if (sourceSection != targetSection)
            {
                sourceSection.ContentItems.Remove(draggedItem);
                ContentItem[] items = sourceSection.ContentItems.OrderBy(ci => ci.SortOrder).ToArray();
                for (int i = 0; i < sourceSection.ContentItems.Count; i++)
                {
                    items[i].SortOrder = i + 1;
                }

                sourceSection.ContentItems = items;

                // Update the source section's content items
                await ContentItemService.UpdateSectionContentItemsAsync(targetSection.ContentItems);
            }

            // Insert the dragged item at the target position and adjust other item orders
            if (targetPosition == targetContentItems.Count)
            {
                if (sourceSection == targetSection)
                {
                    // Remove from the target section
                    targetContentItems.Remove(draggedItem);
                }

                // Insert at the end
                targetContentItems.Add(draggedItem);
            }
            else
            {
                if (targetPosition == 0)
                {
                    if (sourceSection == targetSection)
                    {
                        // Remove from the target section
                        targetContentItems.Remove(draggedItem);
                    }

                    // Insert at the beginning
                    targetContentItems.Insert(0, draggedItem);
                }
                else
                {
                    if (sourceSection == targetSection)
                    {
                        var draggedPosition = targetContentItems.IndexOf(draggedItem);
                        targetContentItems.Remove(draggedItem);

                        //if (targetPosition > draggedPosition)
                        //{
                        //    targetPosition--;
                        //}
                    }

                    // Insert in the middle and adjust orders
                    targetContentItems.Insert(targetPosition, draggedItem);
                }

                for (int i = targetPosition + 1; i < targetContentItems.Count; i++)
                {
                    targetContentItems[i].SortOrder++;
                }
            }

            // 
            targetSection.ContentItems = targetContentItems;

            // Update the target section's content items
            await ContentItemService.UpdateSectionContentItemsAsync(sourceSection.ContentItems);

            StateHasChanged();
        }

        private async Task OnDrop(int targetSectionId, int targetOrder = -1)
        {
            if (draggedItem == null)
            {
                Logger.LogWarning("No content item is being dragged.");
                return;
            }

            if (draggedItem.ContentId <= 0)
            {
                Logger.LogWarning("Invalid ContentId: {0}", draggedItem.ContentId);
                return;
            }

            var sourceSectionId = draggedItem.SectionId;

            if (_sections.TryGetValue(sourceSectionId, out var sourceSection) &&
                _sections.TryGetValue(targetSectionId, out var targetSection))
            {
                if (sourceSection.ContentItems.Contains(draggedItem))
                {
                    sourceSection.ContentItems.Remove(draggedItem);
                    draggedItem.SectionId = targetSectionId;
                    targetSection.ContentItems.Add(draggedItem);

                    // Sort and update order
                    var contentItemsList = targetSection.ContentItems.ToList();
                    for (int i = 0; i < contentItemsList.Count; i++)
                    {
                        contentItemsList[i].SortOrder = i + 1;
                        await UpdateContentItemSortOrderAsync(contentItemsList[i].ContentId, contentItemsList[i].SortOrder);
                    }

                    await UpdateContentItemSectionAsync(draggedItem.ContentId, targetSectionId);
                    StateHasChanged();
                }
                else
                {
                    Logger.LogWarning("Dragged item not found in source section.");
                }
            }
            else
            {
                Logger.LogWarning("Source or target section not found.");
            }
        }



        private async Task UpdateContentItemSortOrderAsync(int contentId, int sortOrder)
        {
            // Uppdatera sorteringsordningen i databasen via ditt ContentItemService
            await ContentItemService.UpdateSortOrderAsync(contentId, sortOrder);
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
            foreach (var section in _sections.Values)
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

        private async Task SaveTextAlignAsync(ContentItem contentItem, string newTextAlign)
        {
            contentItem.TextAlign = newTextAlign;
            await ContentItemService.UpdateContentItemAsync(contentItem);
            StateHasChanged(); // Uppdatera sidan så att den reflekterar ändringen
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

        private ContentItem[] GetSection(Section section) => section.ContentItems?.ToArray() ?? Array.Empty<ContentItem>();

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
        Footer,
        Toolbar
    }
}