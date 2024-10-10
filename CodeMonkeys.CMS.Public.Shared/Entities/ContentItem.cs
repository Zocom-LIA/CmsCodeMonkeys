using Microsoft.AspNetCore.Components;

using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class ContentItem : Content, IEntity
    {
        public int SortOrder { get; set; } // Nytt fält för sortering
        required public string Text { get; set; } = string.Empty;
        [NotMapped]
        public bool IsEditing { get; set; } = false;
        [NotMapped]
        public bool ShowEditButton { get; set; } = true;
        public int FontSize { get; set; } = 16; // Default font size
        public string TextColor { get; set; } = "Black"; // Default text color
        public string TextAlign { get; set; } = "center";
        public bool IsBold { get; set; } // New property for bold
        public bool IsItalic { get; set; } // New property for italic
        public string FontFamily { get; set; } = "Arial"; // Default font family
        [NotMapped]
        public bool IsDragging { get; set; }
        private bool isLinkEnabled; // Backing field for IsLinkEnabled
        public bool IsLinkEnabled
        {
            get => isLinkEnabled;
            set
            {
                isLinkEnabled = value;
                if (!value)
                {
                    // Clear the link information when the link is disabled
                    LinkUrl = string.Empty;
                    LinkDescription = string.Empty;
                }
            }
        }

        public string? LinkUrl { get; set; } // For the link URL
        public string? LinkDescription { get; set; } = string.Empty; // For the link description

        required public int SectionId { get; set; } // Section ID
        public Section Section { get; set; }

        public override RenderFragment RenderContent()
        {
            return builder =>
            {
                if (IsLinkEnabled)
                {
                    // If the link is enabled, render the link
                    builder.OpenElement(0, "a");
                    builder.AddAttribute(1, "class", "content-link");
                    builder.AddAttribute(2, "href", LinkUrl ?? string.Empty);
                    builder.AddAttribute(3, "style", $"color: {Color}; text-align: {TextAlign}px; font-size: {FontSize}px; font-family: {FontFamily}; font-weight: {(IsBold ? "bold" : "normal")}; font-style: {(IsItalic ? "italic" : "normal")}");
                    builder.AddContent(4, LinkDescription ?? string.Empty);
                    builder.CloseElement();
                }
                else
                {
                    builder.OpenElement(0, "span");
                    builder.AddAttribute(1, "class", "content-text");
                    builder.AddAttribute(2, "style", $"color: {Color}; text-align: {TextAlign}px; font-size: {FontSize}px; font-family: {FontFamily}; font-weight: {(IsBold ? "bold" : "normal")}; font-style: {(IsItalic ? "italic" : "normal")}");
                    builder.AddContent(3, Text ?? string.Empty);
                    builder.CloseElement();
                }

            };
        }
    }
}