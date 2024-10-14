using CodeMonkeys.CMS.Public.Components.Shared;
using CodeMonkeys.CMS.Public.Services;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Services;

using Microsoft.AspNetCore.Components;

namespace CodeMonkeys.CMS.Public.Components.Pages
{
    public partial class SiteContentViewer : ContentBaseComponent<SiteContentViewer>
    {
        [Parameter] public int WebPageId { get; set; }
        [Parameter] public bool TakeResponsibilityForNavBar { get; set; } = true;
        public WebPage? WebPage { get; set; }

        [Inject] required public ISiteService SiteService { get; set; }
        [Inject] required public ISectionService SectionService { get; set; }
        [Inject] required public MenuConfigurationService MenuConfigurationService { get; set; }

        private Dictionary<int, Section> Sections { get; set; } = new Dictionary<int, Section>();

        protected override async Task OnInitializedAsync()
        {
            if (TakeResponsibilityForNavBar) MenuConfigurationService.IsEnabled = false;
            await ExecuteWithLoadingAsync(async () =>
            {
                await base.OnInitializedAsync();

                WebPage ??= await WebPageService.GetVisitorWebPageAsync(WebPageId, includeSections: true, includeContents: true);

                if (WebPage == null)
                {
                    Navigation.NavigateTo("/error");
                }
            });
        }

        private WebPage NoLandingPageConfiguredPage()
        {
            // Generate an error message that explains that the site is not yet published. 
            return new WebPage() { Title = "The site is not yet published", Contents = [new Content() { Body = "The site is not yet published. Please try again later." }] };
        }

        private WebPage NoSuchPagePage(string PageName)
        {
            return new WebPage() { Title = "No such page", Contents = [new Content() { Body = $"No such page: {PageName}" }] };
        }

        private RenderFragment RenderSection(Section section) => builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", $"section-{section.SectionId}"); // Add a class to the div element with the section ID
            builder.AddAttribute(2, "style", $"color: {section.Color}");

            int seq = 3;
            foreach (var content in section.ContentItems)
            {
                builder.AddContent(seq++, content.RenderContent());
            }
            builder.CloseElement();
        };
    }
}