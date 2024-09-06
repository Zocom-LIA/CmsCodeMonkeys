namespace CodeMonkeys.CMS.Public.Components.Shared
{
    using CodeMonkeys.CMS.Public.Components.Pages;
    using CodeMonkeys.CMS.Public.Shared;
    using CodeMonkeys.CMS.Public.Shared.Entities;
    using CodeMonkeys.CMS.Public.Shared.Services;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Identity;

    public abstract partial class ContentBaseComponent<T> : BaseComponent<T> where T : class
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Inject] protected IPageStatsService PageStatsService { get; set; }
        [Inject] protected IWebPageService WebPageService { get; set; }
        [Inject] protected IContentService ContentService { get; set; }

        private bool _firstVisit = true;
        private int _pageVisits;
        protected int PageVisits => _pageVisits;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected override async Task OnInitializedAsync()
        {
            if (_firstVisit)
            {
                _firstVisit = false;
                await base.OnInitializedAsync();

                HttpContext httpContext = HttpContextAccessor?.HttpContext!;
                if (httpContext != null)
                {
                    await ExecuteWithLoadingAsync(async () =>
                    {
                        _pageVisits = await PageStatsService.GetPageVisitsAsync(httpContext.Request.Path.Value!) + 1;
                    });
                }
            }
        }
    }
}