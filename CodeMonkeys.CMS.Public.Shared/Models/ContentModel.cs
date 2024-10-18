using Microsoft.AspNetCore.Components;

using System.Text;

using static System.Net.Mime.MediaTypeNames;

namespace CodeMonkeys.CMS.Public.Shared.Models;
public abstract class ContentModel
{
    public int Id { get; set; }
    public int BorderRadius { get; set; } = 0; // Default border radius
    public string BackgroundColor { get; set; } = "#FFFFFF";
    public string BackgroundBorder { get; set; } = "#FFFFFF";
    public int BorderPix { get; set; } = 0;
    public int Padding { get; set; } = 0;
    public int MarginTop { get; set; } = 0;
    public int MarginBottom { get; set; } = 0;
    public int MarginLeft { get; set; } = 0;
    public int MarginRight { get; set; } = 0;
    public string TextAlign { get; set; } = "left"; // Default alignment

    public int? SelectedContainerId { get; set; }
    public bool IsEditing { get; set; } = false;
    public bool ShowEditButton { get; set; }

    public int Row { get; set; } = 1; // Default row
    public int Column { get; set; } = 1; // Default column

    public abstract MarkupString GetContent();

    public virtual ContentModel Clone(int col, int row)
    {
        var clone = (ContentModel)MemberwiseClone();
        clone.Column = col;
        clone.Row = row;
        return clone;
    }

    public virtual TModel Clone<TModel>(int col, int row) where TModel : ContentModel
    {
        return Clone(col, row) as TModel ?? throw new InvalidCastException("Could not cast clone to specified type");
    }

    protected StringBuilder GetStyleBuilder()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append($"border-radius:{BorderRadius}px;");

        if (BackgroundColor != "#FFFFFF")
        {
            sb.Append($"background-color:{BackgroundColor};");
        }

        if (BackgroundBorder != "#FFFFFF" && BorderPix > 0)
        {
            sb.Append($"border:{BorderPix}px solid {BackgroundBorder};");
        }

        if (Padding > 0)
        {
            sb.Append($"padding:{Padding}px;");
        }

        if (MarginTop != 0)
        {
            sb.Append($"margin-top:{MarginTop}px;");
        }

        if (MarginBottom != 0)
        {
            sb.Append($"margin-bottom:{MarginBottom}px;");
        }

        if (MarginLeft != 0)
        {
            sb.Append($"margin-left:{MarginLeft}px;");
        }

        if (MarginRight != 0)
        {
            sb.Append($"margin-right:{MarginRight}px;");
        }

        return sb;
    }

    public virtual MarkupString GetStyles()
    {
        return (MarkupString)GetStyleBuilder().ToString();
    }
}