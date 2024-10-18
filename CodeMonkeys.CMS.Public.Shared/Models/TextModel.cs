using Microsoft.AspNetCore.Components;

namespace CodeMonkeys.CMS.Public.Shared.Models;
public class TextModel : ContentModel
{
    public string? Text { get; set; }
    public bool IsBold { get; set; }
    public bool IsItalic { get; set; }

    public string Color { get; set; } = "#000000";
    public int FontSize { get; set; } = 12;

    public override MarkupString GetContent()
    {
        return (MarkupString)$"<span style='{GetStyles()}'>{Text}</span>";
    }

    public override MarkupString GetStyles()
    {
        var sb = GetStyleBuilder();

        if (Color != "#000000")
        {
            sb.Append($"color:{Color};");
        }

        if (FontSize != 12)
        {
            sb.Append($"font-size:{FontSize}px;");
        }

        if (IsBold)
        {
            sb.Append("font-weight:bold;");
        }

        if (IsItalic)
        {
            sb.Append("font-style:italic;");
        }

        return (MarkupString)sb.ToString();
    }

    public override ContentModel Clone(int col, int row)
    {
        return base.Clone(col, row);
    }
}
