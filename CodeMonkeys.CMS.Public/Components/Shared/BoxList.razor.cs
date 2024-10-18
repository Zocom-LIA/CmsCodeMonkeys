using CodeMonkeys.CMS.Public.Components.Shared.UI;
using CodeMonkeys.CMS.Public.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace CodeMonkeys.CMS.Public.Components.Shared;

public partial class BoxList : ComponentBase
{
    [Parameter]
    public EventCallback<ContentModel> OnBoxSelected { get; set; }

    private ConfigurationDialog Configuration { get; set; }
    private RenderFragment ConfigurationInput { get; set; }
    private EventCallback OnConfigurationConfirm { get; set; }
    private EventCallback OnConfigurationCancel { get; set; }

    private List<ContentModel> boxes = new List<ContentModel>();
    private List<SectionModel> containers = new List<SectionModel>();
    private int nextId = 1; // Startar från 1 för enklare nummer
    private bool showButtons = true;

    protected override void OnInitialized()
    {
    }

    private void ClearAll()
    {
        boxes.Clear(); // Rensa alla boxar
        containers.Clear(); // Rensa alla containrar
        nextId = 1; // Återställ ID-tellern om du vill börja om från början
    }

    private void ToggleButtonsVisibility()
    {
        showButtons = !showButtons;
    }

    private void AddTextModel()
    {
        boxes.Add(new TextModel { Id = nextId++, Text = $"Text Box {nextId}" });
    }

    private void AddImageModel()
    {
        boxes.Add(new ImageModel { Id = nextId++, ImageUrl = "https://via.placeholder.com/100" });
    }

    private void MoveBox(ContentModel box, int rowChange, int columnChange)
    {
        int oldRow = box.Row;
        int oldColumn = box.Column;

        // Calculate new position
        int newRow = oldRow + rowChange;
        int newColumn = oldColumn + columnChange;

        if (IsValidPosition(newRow, newColumn))
        {
            box.Row = newRow;
            box.Column = newColumn;
            Console.WriteLine($"Moved box {box.Id} to new position: Row {newRow}, Column {newColumn}"); // Debugging
        }
        else
        {
            Console.WriteLine($"Invalid move for box {box.Id}: Row {newRow}, Column {newColumn}"); // Debugging
        }
    }

    // Set your grid boundaries, e.g., maxRows and maxColumns
    const int maxRows = 15; // example value
    const int maxColumns = 5; // example value
    private bool IsValidPosition(int row, int column)
    {
        return row > 0 && column > 0 && row <= maxRows && column <= maxColumns;
    }

    private bool CanMoveBox(ContentModel box, int rowChange, int columnChange) => IsValidPosition(box.Row + rowChange, box.Column + columnChange);
    private void MoveBoxUp(ContentModel box)
    {
        if (CanMoveBox(box, -1, 0))
        {
            MoveBox(box, -1, 0);
        }
    }
    private void MoveBoxDown(ContentModel box)
    {
        if (CanMoveBox(box, 1, 0))
        {
            MoveBox(box, 1, 0);
        }
    }
    private void MoveBoxLeft(ContentModel box)
    {
        if (CanMoveBox(box, 0, -1))
        {
            MoveBox(box, 0, -1);
        }
    }
    private void MoveBoxRight(ContentModel box)
    {
        if (CanMoveBox(box, 0, 1))
        {
            MoveBox(box, 0, 1);
        }
    }

    private void CloneBox(ContentModel box)
    {
        for (int col = 1; col <= maxColumns; col++)
        {
            if (col == box.Column) continue;
            var clone = box.Clone(col, box.Row);
            clone.Id = nextId++;
            boxes.Add(clone);
        }
    }

    private void CloneSection(SectionModel box)
    {
        int init = containers.Count();
        for (int col = 1; col <= maxColumns; col++)
        {
            if (col == box.Column) continue;
            var clone = (SectionModel)box.Clone(col, box.Row);
            clone.Id = nextId++;
            boxes.Add(clone);
        }
        int outro = containers.Count();
    }

    private void RemoveBox(ContentModel box)
    {
        boxes.Remove(box);
    }

    private void AddContainer()
    {
        var newContainer = new SectionModel { Id = nextId++, Title = $"Container {nextId - 1}", InnerBoxes = new List<ContentModel>() };
        boxes.Add(newContainer);
        containers.Add(newContainer);
    }

    private void ChangeTextAlign(ContentModel box, ChangeEventArgs e)
    {
        box.TextAlign = e.Value.ToString(); // Update text alignment based on the selected value
    }


    private void HandleImageClick(ContentModel box)
    {
        // Kontrollera om boxen redan är i redigeringsläge
        if (box.IsEditing)
        {
            return; // Gör ingenting om den redan är i redigeringsläge
        }
        ToggleEdit(box); // Annars aktivera redigering
    }

    private void ToggleEdit(ContentModel box)
    {
        box.IsEditing = !box.IsEditing; // Växlar redigeringstillståndet
        if (box is TextModel || box is ImageModel)
        {
            // Visa "Ändra"-knappen om redigering är aktiverad
            box.ShowEditButton = !box.IsEditing;
        }
    }

    private void Save(ContentModel box)
    {
        // Toggle back to original view
        box.IsEditing = false; // Set editing to false
        box.ShowEditButton = false; // Hide the edit button since we're toggling back
    }

    private void Changes(ContentModel box)
    {
        // Implement your save logic here, e.g., updating the model or saving changes
        box.IsEditing = false; // Set editing to false after saving
        box.ShowEditButton = true; // Show "Ändra" button again after saving
    }

    private void AddInnerTextModel(SectionModel container)
    {
        container.InnerBoxes.Add(new TextModel { Id = nextId++, Text = $"Inner Text Model {nextId}" });
    }

    private void AddInnerImageModel(SectionModel container)
    {
        container.InnerBoxes.Add(new ImageModel { Id = nextId++, ImageUrl = "https://via.placeholder.com/100" });
    }

    private void MoveInnerUp(ContentModel box, SectionModel parentContainer)
    {
        int index = parentContainer.InnerBoxes.IndexOf(box);
        if (index > 0)
        {
            parentContainer.InnerBoxes.RemoveAt(index);
            parentContainer.InnerBoxes.Insert(index - 1, box);
        }
    }

    private void MoveInnerDown(ContentModel box, SectionModel parentContainer)
    {
        int index = parentContainer.InnerBoxes.IndexOf(box);
        if (index >= 0 && index < parentContainer.InnerBoxes.Count - 1)
        {
            parentContainer.InnerBoxes.RemoveAt(index);
            parentContainer.InnerBoxes.Insert(index + 1, box);
        }
    }

    private void RemoveInnerBox(ContentModel box, SectionModel parentContainer)
    {
        parentContainer.InnerBoxes.Remove(box);
    }

    private void RemoveContainer(SectionModel container)
    {
        boxes.Remove(container);
        containers.Remove(container);
    }

    private void ClearContainer(SectionModel container)
    {
        container.InnerBoxes.Clear(); // Rensa alla inre boxar
    }

    private void ChangeContainer(ContentModel box, ChangeEventArgs e)
    {
        var selectedContainerIdStr = e.Value?.ToString();
        if (string.IsNullOrEmpty(selectedContainerIdStr))
        {
            return; // Avbryt om inget värde är valt
        }

        int selectedContainerId = int.Parse(selectedContainerIdStr);
        var targetContainer = containers.FirstOrDefault(c => c.Id == selectedContainerId);

        SectionModel currentContainer = containers.FirstOrDefault(c => c.InnerBoxes.Contains(box));

        if (targetContainer != null && !(box is SectionModel))
        {
            if (currentContainer != null)
            {
                currentContainer.InnerBoxes.Remove(box);
            }

            targetContainer.InnerBoxes.Add(box);
            box.SelectedContainerId = targetContainer.Id; // Uppdatera vald container ID

            StateHasChanged(); // Uppdatera UI
        }
    }
}