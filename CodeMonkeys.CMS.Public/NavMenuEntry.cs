namespace CodeMonkeys.CMS.Public
{
    public class NavMenuEntry
    {
        public bool ShowWhenAuthorized { get; set; } = false;
        public bool ShowWhenUnauthorized { get; set; }= false;
        public string Href { get; set; }
        public string LinkText { get; set; }
        public string? EmptySpanClass { get; set; }
    }
}
