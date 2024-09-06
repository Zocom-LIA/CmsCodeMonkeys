using System.ComponentModel.DataAnnotations;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class Menu : IEntity
    {
        [Key]
        public int MenuId { get; set; }
        public string Name { get; set; }
        public int SiteId { get; set; }
        public Site Site { get; set; }
        public ICollection<MenuItem> Items { get; set; } = new List<MenuItem>();
        public object GetIdentifier() => MenuId;
    }
}