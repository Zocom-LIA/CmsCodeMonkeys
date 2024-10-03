using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class Section : IEntity, IRenderable, IConfigurable
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

        public virtual RenderFragment RenderContent()
        {
            return builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", $"section-{Name.ToLower()}");
                builder.AddAttribute(2, "style", $"background-color: {Color.ToLower()}");

                int seq = 3;
                foreach (var content in ContentItems)
                {
                    builder.AddContent(seq++, content.RenderContent());
                }

                builder.CloseElement();
            };
        }
    }
}