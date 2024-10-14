namespace CodeMonkeys.CMS.Public
{
    public class NavMenuEntry
    {
        public bool ShowWhenAuthorized { get; set; } = true;
        public bool ShowWhenUnauthorized { get; set; }= true;
        public string Href { get; set; }
        public string LinkText { get; set; }
        public string? EmptySpanClass { get; set; }
    }
}
