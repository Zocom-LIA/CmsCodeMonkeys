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

        public int? WebPageId { get; set; }
        public WebPage? WebPage { get; set; }

        public string Color { get; set; } = "#1e1e1e";

        public object GetIdentifier() => ContentId;
    }
    public class TextContent : Content
    {
        public string Text { get; set; }
    }

    public class ImageContent : Content
    {
        public string ImageUrl { get; set; }
    }
    public class VideoContent : Content
    {
        public string VideoUrl { get; set; }
    }

    public class LinkContent : Content
    {
        public string LinkUrl { get; set; }
        public string LinkDescription { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class FileContent : Content
    {
        public string FileUrl { get; set; }
    }

    public class QuoteContent : Content
    {
        public string Quote { get; set; }
        public string QuoteAuthor { get; set; }
    }

    public class CodeContent : Content
    {
        public string Code { get; set; }
    }
}