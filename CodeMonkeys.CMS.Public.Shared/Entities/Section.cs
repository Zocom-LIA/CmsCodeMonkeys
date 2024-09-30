using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class Section : IEntity
    {
        public int SectionId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Color { get; set; }

        public int WebPageId { get; set; }
        public WebPage WebPage { get; set; }

        public ICollection<ContentItem> ContentItems { get; set; } = new List<ContentItem>();

        public object GetIdentifier() => SectionId;
    }
}
