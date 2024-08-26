using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class Site
    {
        [Key]
        public int SiteId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public Guid? CreatorId { get; set; }
        public ICollection<WebPage> Pages { get; set; } = new List<WebPage>();
        [ForeignKey("LandingPageId")] public WebPage? LandingPage { get; set; }
    }
}