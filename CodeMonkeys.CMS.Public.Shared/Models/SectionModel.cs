using Microsoft.AspNetCore.Components;

using System.Collections.Generic;

namespace CodeMonkeys.CMS.Public.Shared.Models;

public class SectionModel : ContentModel
{
    public string? Title { get; set; }
    public List<ContentModel> InnerBoxes { get; set; } = new List<ContentModel>(); // Lista över boxar i containern
    private int? _selectedInnerBoxId;
    public int Height { get; set; } = 100;

    public int? SelectedInnerBoxId
    {
        get => _selectedInnerBoxId;
        set
        {
            _selectedInnerBoxId = value;
            // Invoke a method or event to handle the box movement when selection changes
            if (_selectedInnerBoxId.HasValue)
            {
                // This is where you might call a method to move the box
                // MoveBox(selectedInnerBoxId.Value); // Uncomment this if you implement a MoveBox method
            }
        }
    }

    public override MarkupString GetStyles()
    {
        var sb = GetStyleBuilder();
        sb.Append($"height:{Height}px;");
        return (MarkupString)sb.ToString();
    }

    public override MarkupString GetContent()
    {
        return (MarkupString)Title; // Returnera titeln för containern som MarkupString
    }


}
