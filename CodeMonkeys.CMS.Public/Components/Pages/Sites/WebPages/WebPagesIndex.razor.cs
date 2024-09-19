using CodeMonkeys.CMS.Public.Components.Shared;
using CodeMonkeys.CMS.Public.Components.Shared.UI;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMonkeys.CMS.Public.Components.Pages.Sites.WebPages
{
    public partial class WebPagesIndex : AuthenticationBaseComponent<WebPagesIndex>
    {
        [SupplyParameterFromForm]
        private SiteModel Input { get; set; } = new SiteModel();

        [Parameter] public int siteId { get; set; }
        public Site? Site { get; set; }

        private WebPageModel _pageModel = new();

        private ConfirmationDialog? Confirmation { get; set; }
        private string ConfirmationTitle = "Delete Page";
        private string? ConfirmationMessage = "Are you sure that you want to delete this page?";
        private string ConfirmationButtonText = "Yes";
        private string CancelButtonText = "Cancel";
        private EventCallback OnConfirm;
        private EventCallback OnCancel;

        // Toolbar Variables (font size, font family, font color)
        private string SelectedFontSize { get; set; } = "16"; // Default font size
        private string SelectedFontFamily { get; set; } = "Arial"; // Default font family
        private string SelectedFontColor { get; set; } = "#000000"; // Default color in hex

        // Method to dynamically apply styles only to the content block
        private string GetTextStyle()
        {
            return $"font-size: {SelectedFontSize}px; font-family: {SelectedFontFamily}; color: {SelectedFontColor};";
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Site = await SiteService.GetUserSiteAsync(User!.Id, siteId);
            if (Site == null)
            {
                Logger.LogDebug($"Site with ID '{siteId}' for User with ID '{User!.Id}' not found.");
                ErrorMessage = "There is no such site available to edit";
                return;
            }

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

            Site.Name = Input.Name;
            Site.LastModifiedDate = DateTime.Now;

            await SiteService.UpdateSiteAsync(Site);
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
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Author = User
                };

                Site!.Pages.Add(page);
                await WebPageService.CreateWebPageAsync(siteId, page);
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
                page.Author = User;

                await WebPageService.UpdateWebPageAsync(page);
            }

            CloseConfirmation();
        }

        public Task EditContentsAsync(int webPageId)
        {
            Navigation.NavigateTo($"/sites/{siteId}/webpages/{webPageId}/edit");
            return Task.CompletedTask;
        }

        public Task DeletePageAsync(int webPageId)
        {
            ConfirmationTitle = "Delete Page";
            ConfirmationMessage = "Are you sure you want to delete this page?";
            ConfirmationButtonText = "Delete";
            CancelButtonText = "Cancel";
            OnConfirm = EventCallback.Factory.Create(this, async () => await DeletePageConfirmedAsync(webPageId));
            OnCancel = EventCallback.Factory.Create(this, () => { CloseConfirmation(); return Task.CompletedTask; });

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