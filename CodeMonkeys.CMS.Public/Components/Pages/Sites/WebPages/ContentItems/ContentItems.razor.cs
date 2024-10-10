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
    if (string.IsNullOrWhiteSpace(newContentItemText))
        return;

    if (_sections.TryGetValue(selectedList, out var section))
    {
        section.ContentItems ??= new List<ContentItem>();
        var contentItem = new ContentItem
        {
            SectionId = selectedList,
            Text = newContentItemText,
            Title = newContentItemText,
            Body = newContentItemText,
            OrdinalNumber = section.ContentItems.Count(),
            AuthorId = User!.Id,
            WebPageId = WebPageId
        };

        section.ContentItems.Add(contentItem);
        await ContentItemService.AddContentItemAsync(contentItem);
        newContentItemText = string.Empty;

        // Reset edit button visibility
        ResetEditButtonVisibility();
        StateHasChanged();
    }
    else
    {
        Logger.LogWarning("Section with ID {0} does not exist.", selectedList);
    }
}

private void ResetEditButtonVisibility()
{
    ResetShowEditButton(_section1);
    ResetShowEditButton(_section2);
    ResetShowEditButton(_section3);
    ResetShowEditButton(_section4);
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
                Logger.LogWarning("Invalid ContentId: {0}", ContentId);
                return; // Prevent further execution
            }

            // Call your service to update the section ID
            await ContentItemService.UpdateSectionIdAsync(ContentId, newSectionId);
        }

private bool sortOrderChanged = false;

//NY Ondrop funkar

private async Task OnDrop(int targetSectionId, int? targetIndex)
{
    Console.WriteLine($"OnDrop called with targetSectionId: {targetSectionId}, targetIndex: {targetIndex}");

    if (draggedItem == null || draggedItem.ContentId <= 0)
    {
        HandleInvalidDragItem();
        return;
    }

    var sourceSectionId = draggedItem.SectionId;
    
    Console.WriteLine($"Dragging item from section ID: {sourceSectionId}");

    if (_sections.TryGetValue(sourceSectionId, out var sourceSection) &&
        _sections.TryGetValue(targetSectionId, out var targetSection))
    {
        Console.WriteLine($"Source section found: {sourceSection.Name}, Target section found: {targetSection.Name}");
        var originalIndex = GetOriginalIndex(sourceSection, draggedItem);

        var sourceContentItems = sourceSection.ContentItems.ToList();
        var targetContentItems = targetSection.ContentItems.ToList();

        if (sourceSectionId == targetSectionId)
        {
            await HandleDropWithinSameSection(sourceContentItems, targetContentItems, targetIndex, originalIndex);
            StateHasChanged();
        }
        else
        {
            await HandleDropBetweenSections(sourceContentItems, targetContentItems, targetSectionId, targetIndex);
            StateHasChanged();
        }
        if (sortOrderChanged) // Add a flag to track changes
        {
            await UpdateSortOrder(sourceContentItems);
            StateHasChanged();
        }
        

        // Uppdatera sektionen för det flyttade objektet
        await UpdateContentItemSectionAsync(draggedItem.ContentId, targetSectionId);
       
        Console.WriteLine($"Updated section for dragged item ID: {draggedItem.ContentId}");

        Console.WriteLine("StateHasChanged called after drop operation.");
        StateHasChanged();
    }
    else
    {
        Console.WriteLine("Source or target section not found.");
        Logger.LogWarning("Source or target section not found.");
    }
}


private void HandleInvalidDragItem()
{
    if (draggedItem == null)
    {
        Console.WriteLine("No content item is being dragged.");
        Logger.LogWarning("No content item is being dragged.");
    }
    else
    {
        Console.WriteLine($"Invalid ContentId: {draggedItem.ContentId}");
        Logger.LogWarning("Invalid ContentId: {0}", draggedItem.ContentId);
    }
}

private int GetOriginalIndex(Section sourceSection, ContentItem draggedItem)
{
    return sourceSection.ContentItems
        .Select((item, index) => new { item, index })
        .FirstOrDefault(x => x.item.OrdinalNumber == draggedItem.OrdinalNumber)?.index ?? -1;
}


// NY
private async Task HandleDropWithinSameSection(List<ContentItem> sourceContentItems, List<ContentItem> targetContentItems, int? targetIndex, int originalIndex)
{
    // Kontrollera om det finns ett giltigt targetIndex och om det skiljer sig från originalIndex
    if (!targetIndex.HasValue || targetIndex.Value == originalIndex)
    {
        Console.WriteLine("No position change detected.");
        return;
    }

    sortOrderChanged = true;

    // Se till att originalIndex är giltigt
    if (originalIndex >= 0 && originalIndex < sourceContentItems.Count)
    {
        // Hämta objektet som ska flyttas
        var itemToMove = sourceContentItems[originalIndex];
        sourceContentItems.RemoveAt(originalIndex);
        StateHasChanged();

        // Om targetIndex är giltigt, sätt in objektet på rätt position
        if (targetIndex.Value >= 0 && targetIndex.Value <= sourceContentItems.Count)
        {
            sourceContentItems.Insert(targetIndex.Value, itemToMove);
        }
        else
        {
            // Lägg till objektet sist i listan om indexet är utanför gränserna
            sourceContentItems.Add(itemToMove);
        }

        // Uppdatera sorteringsordningen
        await UpdateSortOrder(sourceContentItems);
        
        // Återge UI:t för att visa den uppdaterade ordningen
        StateHasChanged();
    }
}


private int CompareContentItems(ContentItem x, ContentItem y)
{
    // Anpassa jämförelsen baserat på dina sorteringskriterier
    return x.SortOrder.CompareTo(y.SortOrder);
}

private async Task HandleDropBetweenSections(List<ContentItem> sourceContentItems, List<ContentItem> targetContentItems, int targetSectionId, int? targetIndex)
{
    Console.WriteLine("Handling drop between different sections.");

    if (draggedItem == null)
    {
        Console.WriteLine("No item is being dragged.");
        Logger.LogWarning("No item is being dragged.");
        return;
    }

    // Ta bort objektet från källsektionen
    if (sourceContentItems.Contains(draggedItem))
    {
        sourceContentItems.Remove(draggedItem);
        Console.WriteLine("Removed dragged item from source section.");
    }

    // Uppdatera sektion ID för draget objekt
    draggedItem.SectionId = targetSectionId;
    Console.WriteLine($"Updated dragged item section ID to: {targetSectionId}");

    // Lägg till objektet i den nya sektionen
    if (targetContentItems == null)
    {
        Console.WriteLine("Target content items list is null.");
        Logger.LogWarning("Target content items list is null.");
        return;
    }

    if (targetIndex.HasValue && targetIndex.Value >= 0 && targetIndex.Value <= targetContentItems.Count)
    {
        targetContentItems.Insert(targetIndex.Value, draggedItem);
        Console.WriteLine($"Inserted dragged item at index: {targetIndex.Value} in target section.");
    }
    else
    {
        targetContentItems.Add(draggedItem); // Lägg till sist om inget index ges
        Console.WriteLine("Added dragged item to the end of the target section.");
    }

    // Uppdatera ContentItems i target sektionen
    if (_sections.TryGetValue(targetSectionId, out var targetSection))
    {
        targetSection.ContentItems = targetContentItems;
        Console.WriteLine($"Updated content items in target section: {targetSection.Name}");
    }
    else
    {
        Console.WriteLine("Target section not found while updating content items.");
        Logger.LogWarning("Target section not found while updating content items.");
    }

    // Spara ändringar i databasen för det flyttade objektet (om nödvändigt)
    try
    {
        await UpdateContentItemSectionAsync(draggedItem.ContentId, targetSectionId);
        Console.WriteLine($"Updated section for dragged item ID: {draggedItem.ContentId}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error updating content item section: {ex.Message}");
        Logger.LogError(ex, "Error updating content item section.");
    }

    // Uppdatera UI:n
    StateHasChanged(); // Detta uppdaterar tillståndet på sidan
}




private async Task UpdateSortOrder(List<ContentItem> items)
{
    // Sort and update only if necessary
    var changesMade = false;
    for (int i = 0; i < items.Count; i++)
    {
        if (items[i].SortOrder != i + 1)
        {
            items[i].SortOrder = i + 1; // Sort orders start from 1
            await UpdateContentItemSortOrderAsync(items[i].ContentId, items[i].SortOrder);
            Console.WriteLine($"Updated sort order for item ID: {items[i].ContentId}, new sort order: {items[i].SortOrder}");
            changesMade = true;
        }
    }
    if (changesMade)
    {
        Console.WriteLine("Sort order updated.");
        await InvokeAsync(StateHasChanged);
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