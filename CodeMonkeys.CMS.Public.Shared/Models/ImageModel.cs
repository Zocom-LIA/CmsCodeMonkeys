using Microsoft.AspNetCore.Components;
namespace CodeMonkeys.CMS.Public.Shared.Models;

public class ImageModel : ContentModel
{
    public string? ImageUrl { get; set; }
    public int Height { get; set; } = 100; // Default height
    public int Width { get; set; } = 100; // Default width

    public override MarkupString GetStyles()
    {
        var sb = GetStyleBuilder();

        sb.Append($"height:{Height}px;");
        sb.Append($"width:{Width}px;");

        return (MarkupString)sb.ToString();
    }

    public override MarkupString GetContent()
    {
        // Return as MarkupString
        return (MarkupString)$"<img src='{ImageUrl}' height='{Height}px' width='{Width}px' style='{GetStyles()}' />";
    }
}