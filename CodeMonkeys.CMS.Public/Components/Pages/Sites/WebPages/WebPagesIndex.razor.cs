using CodeMonkeys.CMS.Public.Components.Shared;
using CodeMonkeys.CMS.Public.Components.Shared.UI;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

using System.ComponentModel.DataAnnotations;

namespace CodeMonkeys.CMS.Public.Components.Pages.Sites.WebPages
{
    public partial class WebPagesIndex : AuthenticationBaseComponent<WebPagesIndex>
    {
        [SupplyParameterFromForm]
        private SiteModel Input { get; set; } = new SiteModel();

        [Parameter] public int siteId { get; set; }
        [Inject] public IPageStatsService PageStatsService { get; set; }
        public Site? Site { get; set; }

        private WebPageModel _pageModel = new();

        private ConfirmationDialog? Confirmation { get; set; }
        private string ConfirmationTitle = "Delete Page";
        private string? ConfirmationMessage = "Are you sure that you want to delete this page?";
        private string ConfirmationButtonText = "Yes";
        private string CancelButtonText = "Cancel";
        private EventCallback OnConfirm;
        private EventCallback OnCancel;
        private IEnumerable<PageStats> PageStats;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Site = await SiteService.GetUserSiteAsync(User!.Id, siteId);
            if (Site == null)
            {
                throw new InvalidOperationException($"Site with ID '{siteId}' for User with ID '{User!.Id}' not found.");
                Logger.LogDebug($"Site with ID '{siteId}' for User with ID '{User!.Id}' not found.");
                ErrorMessage = "There is no such site available to edit";
                return;
            }
            PageStats = await PageStatsService.GetPageStatsAsync(siteId);
            Input.Name = Site.Name;
        }

        private async Task HandleValidSubmit()
        {
            if (string.IsNullOrEmpty(Input.Name))
            {
                ErrorMessage = "Name is required";
                return;
            }

            User? user = await GetCurrentUserAsync();
            if (user == null)
            {
                Logger.LogDebug("Authenticated User is not authenticated");
                return;
            }

            Site ??= await SiteService.GetUserSiteAsync(user.Id, siteId);

            if (Site == null)
            {
                ErrorMessage = "Site not found";
                return;
            }

            Site!.Name = Input.Name;
            Site!.LastModifiedDate = DateTime.Now;

            await SiteService.UpdateSiteAsync(Site);

            //Navigation.NavigateTo($"sites/{siteId}/webpages");
        }

        public Task AddOrUpdatePageAsync(int? webPageId = null)
        {
            ConfirmationMessage = null;

            if (webPageId == null)
            {
                ConfirmationTitle = "Add Page";
                _pageModel = new WebPageModel
                {
                    Title = "A New Page"
                };
                ConfirmationButtonText = "Add Page";
            }
            else
            {
                ConfirmationTitle = "Edit Page";
                var page = Site!.Pages.FirstOrDefault(page => page.WebPageId == webPageId);
                if (page == null)
                {
                    ErrorMessage = "Content not found";
                    return Task.CompletedTask;
                }

                _pageModel.Title = page.Title;
                ConfirmationButtonText = "Update Content";
            }

            CancelButtonText = "Cancel";
            OnConfirm = EventCallback.Factory.Create(this, async () => await AddOrUpdatePageConfirmedAsync(webPageId));
            OnCancel = EventCallback.Factory.Create(this, () => CloseConfirmation());

            ShowConfirmation();
            return Task.CompletedTask;
        }

        private async Task AddOrUpdatePageConfirmedAsync(int? webPageId = null)
        {
            if (webPageId == null)
            {
                var page = new WebPage
                {
                    Title = _pageModel.Title,
                    AuthorId = User?.Id
                };

                page = await WebPageService.CreateWebPageAsync(siteId, page);
                Site!.Pages.Add(page);
            }
            else
            {
                var page = Site!.Pages.FirstOrDefault(page => page.WebPageId == webPageId);

                if (page == null)
                {
                    ErrorMessage = "Page not found";
                    return;
                }

                page.Title = _pageModel.Title;
                page.LastModifiedDate = DateTime.Now;

                await WebPageService.UpdateWebPageAsync(page);
            }

            CloseConfirmation();
        }

        public Task EditContentsAsync(int webPageId)
        {
            Navigation.NavigateTo($"/sites/{siteId}/webpages/{webPageId}");
            //Navigation.NavigateTo($"/sites/{siteId}/webpages/{webPageId}/edit");
            return Task.CompletedTask;
        }

        public Task DeletePageAsync(int webPageId)
        {
            ConfirmationTitle = "Delete Page";
            ConfirmationMessage = "Are you sure you want to delete this page?";
            ConfirmationButtonText = "Delete";
            CancelButtonText = "Cancel";
            OnConfirm = EventCallback.Factory.Create(this, async () => await DeletePageConfirmedAsync(webPageId));
            OnCancel = EventCallback.Factory.Create(this, async () => { CloseConfirmation(); await Task.CompletedTask; });

            ShowConfirmation();

            return Task.CompletedTask;
        }

        private async Task DeletePageConfirmedAsync(int webPageId)
        {
            var page = Site!.Pages.FirstOrDefault(page => page.WebPageId == webPageId);
            if (page == null)
            {
                ErrorMessage = "Page not found";
                return;
            }

            Site!.Pages.Remove(page);
            await WebPageService.DeleteWebPageAsync(page);
            CloseConfirmation();
        }

        private void CloseConfirmation()
        {
            Confirmation?.Hide();
        }

        private void ShowConfirmation()
        {
            Confirmation?.Show();
        }

        private void NavigateToWebPage(int webPageId, NavigationActions action)
        {
            Navigation.NavigateTo($"/sites/{siteId}");
        }

        private int PageVisitCount(int pageId) => PageStats.Where(s => s.PageId == pageId).Select(s => s.PageVisits).Sum();

        public sealed class SiteModel
        {
            [Required]
            public string Name { get; set; } = string.Empty;
        }

        public sealed class WebPageModel
        {
            public int? WebPageId { get; set; }
            [Required]
            [MinLength(1, ErrorMessage = "You must not leave the Title empty.")]
            public string Title { get; set; } = string.Empty;
        }
    }
}