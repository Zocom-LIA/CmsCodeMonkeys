namespace CodeMonkeys.CMS.Public.Shared.DTOs
{
    public class WebPageDto
    {
        public string Title { get; set; } = string.Empty;

        public IEnumerable<ContentDto> Contents { get; set; } = new List<ContentDto>();
        public AuthorDto? Author { get; set; }
    }
}