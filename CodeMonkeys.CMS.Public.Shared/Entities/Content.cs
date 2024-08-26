using System.ComponentModel.DataAnnotations;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class Content
    {
        [Key]
        public int ContentId { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public Guid? AuthorId { get; set; }

        public int OrdinalNumber { get; set; }
    }
}
