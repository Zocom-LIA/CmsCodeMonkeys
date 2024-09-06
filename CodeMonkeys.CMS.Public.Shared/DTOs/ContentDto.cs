namespace CodeMonkeys.CMS.Public.Shared.DTOs
{
    public class ContentDto
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public int OrdinalNumber { get; set; }
    }
}