namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class Page
    {
        public int Id { get; set; }
        required public string Title { get; set; }
        required public DateTime CreatedDate { get; set; }
        required public DateTime LastModifiedDate { get; set; }

        public int SiteId { get; set; }
        required public Guid UserId { get; set; }

        public ICollection<Content> Contents { get; set; } = new List<Content>();
    }
}