using System.ComponentModel.DataAnnotations;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class Content : IEntity
    {
        [Key]
        public int ContentId { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int OrdinalNumber { get; set; }

        public Guid? AuthorId { get; set; }
        public User? Author { get; set; }

        public string Color { get; set; } = "#1e1e1e";

        public object GetIdentifier() => ContentId;
    }
}