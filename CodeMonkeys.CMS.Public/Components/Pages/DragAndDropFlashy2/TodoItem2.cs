using System;
using System.Collections.Generic;

namespace CodeMonkeys.CMS.Public.Components.Pages.DragAndDropFlashy2
{
    public class TodoItem2
    {
        public string Text { get; set; } = string.Empty;
        public bool IsEditing { get; set; } = false;
        public bool ShowEditButton { get; set; } = true;
        public int FontSize { get; set; } = 16; // Default font size
        public string TextColor { get; set; } = "Black"; // Default text color
        public string? Color { get; set; } 
        public bool IsBold { get; set; } // New property for bold
        public bool IsItalic { get; set; } // New property for italic
        public string FontFamily { get; set; } = "Arial"; // Default font family
    }
}
