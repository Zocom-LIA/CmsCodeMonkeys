using System.ComponentModel.DataAnnotations;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class MenuItem : IEntity
    {
        [Key]
        public int MenuItemId { get; set; }
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public int WebPageId { get; set; }
        public WebPage WebPage { get; set; }
        public int Order { get; set; } // Optional: For ordering menu items
        public object GetIdentifier() => MenuItemId;
    }
}