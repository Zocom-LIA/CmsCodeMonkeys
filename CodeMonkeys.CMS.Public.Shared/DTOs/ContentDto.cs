namespace CodeMonkeys.CMS.Public.Shared.DTOs
{
    public class ContentDto
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public int ContentId { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public string Body { get; set; }
        public int OrdinalNumber { get; set; }
        public AuthorDto? Author { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}