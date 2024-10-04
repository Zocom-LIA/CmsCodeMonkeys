using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.DTOs
{
    public class WebPageIncludeDto
    {
        public int WebPageId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public IEnumerable<ContentDto> Contents { get; set; }
        public AuthorDto? Author { get; set; }
    }
}