namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class Site
    {
        public int Guid { get; set; }
        required public string Name { get; set; }
        required public DateTime CreatedDate { get; set; }
        required public DateTime LastModifiedDate { get; set; }

        public ICollection<Page> Pages { get; set; } = new List<Page>();
    }
}