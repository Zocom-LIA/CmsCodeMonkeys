using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class WebPage
    {
        [Key]
        public int WebPageId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int? SiteId { get; set; }
        public Guid? AuthorId { get; set; }

        public ICollection<Content> Contents { get; set; } = new List<Content>();
    }
}