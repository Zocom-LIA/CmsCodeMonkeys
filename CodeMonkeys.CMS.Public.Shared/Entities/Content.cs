using Microsoft.AspNetCore.Components;

using System.ComponentModel.DataAnnotations;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class Content : IEntity, IRenderable, IConfigurable
    {
        private string _contentType;

        [Key]
        public int ContentId { get; set; }
        public string Title { get; set; }
        public string ContentType
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_contentType))
                {
                    return GetType().Name;
                }
                return _contentType;
            }
            set => _contentType = value;
        }
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
        public virtual RenderFragment RenderContent()
        {
            return builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "class", "content");
                builder.AddAttribute(2, "style", $"color: {Color}");
                builder.AddContent(3, Body);
                builder.CloseElement();
            };
        }
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