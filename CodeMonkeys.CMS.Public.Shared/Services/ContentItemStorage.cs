using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public class ContentItemStorage
    {
        public List<ContentItem> ContentItemList1 { get; set; } = new List<ContentItem>();
        public List<ContentItem> ContentItemList2 { get; set; } = new List<ContentItem>();
        public List<ContentItem> ContentItemList3 { get; set; } = new List<ContentItem>();
        public List<ContentItem> ContentItemList4 { get; set; } = new List<ContentItem>();

        // Box background colors
        public string Box1Color { get; set; } = "White";
        public string Box2Color { get; set; } = "White";
        public string Box3Color { get; set; } = "White"; // For Toolbar
        public string Box4Color { get; set; } = "White";
    }
}