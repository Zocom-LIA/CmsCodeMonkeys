namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class Content
    {
        public int Id { get; set; }
        required public string Title { get; set; }
        required public string ContentType { get; set; }
        required public string Body { get; set; }
        required public DateTime CreatedDate { get; set; }
        required public DateTime LastModifiedDate { get; set; }

        public int PageId { get; set; }
    }
}
