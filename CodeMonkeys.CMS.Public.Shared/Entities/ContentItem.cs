namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class ContentItem : Content, IEntity
    {
        public int ContentItemId { get; set; }
        required public string Text { get; set; } = string.Empty;
        public bool IsEditing { get; set; } = false;
        public bool ShowEditButton { get; set; } = true;
        public int FontSize { get; set; } = 16; // Default font size
        public string TextColor { get; set; } = "Black"; // Default text color
        public bool IsBold { get; set; } // New property for bold
        public bool IsItalic { get; set; } // New property for italic
        public string FontFamily { get; set; } = "Arial"; // Default font family
        public int BoxNumber { get; set; } = 1; // Default box size

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
    }
}