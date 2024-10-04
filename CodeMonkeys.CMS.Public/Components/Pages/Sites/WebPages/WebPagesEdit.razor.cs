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
    public partial class WebPagesEdit : AuthenticationBaseComponent<WebPagesEdit>
    {
        [SupplyParameterFromForm]
        private WebPageModel Input { get; set; } = new WebPageModel();

        [Parameter] public int siteId { get; set; }
        public Site? Site { get; set; }

        [Parameter] public int webPageId { get; set; }
        public WebPage? WebPage { get; set; }

        private ContentModel? Content { get; set; }
        private ConfirmationDialog? Confirmation { get; set; }
        private string ConfirmationTitle = "Delete Content";
        private string? ConfirmationMessage = "Are you sure that you want to delete this content?";
        private string ConfirmationButtonText = "Yes";
        private string CancelButtonText = "Cancel";
        private EventCallback OnConfirm;
        private EventCallback OnCancel;

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

            WebPage = await WebPageService.GetSiteWebPageAsync(siteId, webPageId, true, true, true);

            if (WebPage == null)
            {
                Logger.LogDebug($"WebPage with ID '{webPageId}' for site with ID '{siteId}' not found.");
                ErrorMessage = "There is no such webpage available to edit";
                return;
            }

            Input.Title = WebPage.Title;
            //WebPage.Contents = WebPage.Contents.OrderBy(content => content.OrdinalNumber).ToList();
        }

        private async Task HandleValidSubmit()
        {
            if (string.IsNullOrEmpty(Input.Title))
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

            WebPage!.Title = Input.Title;
            WebPage.LastModifiedDate = DateTime.Now;

            await WebPageService.UpdateWebPageAsync(WebPage);

            Navigation.NavigateTo($"sites/{siteId}/webpages");
        }

        public Task AddOrUpdateContentAsync(int? contentId = null)
        {
            ConfirmationMessage = null;

            if (contentId == null)
            {
                ConfirmationTitle = "Add Content";
                Content = new ContentModel
                {
                    Title = "A Content Title",
                    ContentType = "Text",
                    Body = "A Content Body",
                    OrdinalNumber = WebPage!.Contents.Count
                };
                ConfirmationButtonText = "Add Content";
            }
            else
            {
                ConfirmationTitle = "Edit Content";
                var content = WebPage!.Contents.FirstOrDefault(content => content.ContentId == contentId);
                if (content == null)
                {
                    ErrorMessage = "Content not found";
                    return Task.CompletedTask;
                }

                Content = new ContentModel
                {
                    ContentId = content.ContentId,
                    Title = content.Title,
                    ContentType = content.ContentType,
                    Body = content.Body,
                    OrdinalNumber = content.OrdinalNumber
                };
                ConfirmationButtonText = "Update Content";
            }
            CancelButtonText = "Cancel";
            OnConfirm = EventCallback.Factory.Create(this, async () => await AddOrUpdateContentConfirmedAsync(contentId));
            OnCancel = EventCallback.Factory.Create(this, () => CloseConfirmation());

            ShowConfirmation();
            return Task.CompletedTask;
        }

        private async Task AddOrUpdateContentConfirmedAsync(int? contentId = null)
        {
            if (Content == null)
            {
                ErrorMessage = "Content is required";
                return;
            }

            if (string.IsNullOrEmpty(Content.Title))
            {
                ErrorMessage = "Content Title is required";
                return;
            }

            if (string.IsNullOrEmpty(Content.ContentType))
            {
                ErrorMessage = "Content Type is required";
                return;
            }

            if (string.IsNullOrEmpty(Content.Body))
            {
                ErrorMessage = "Content Body is required";
                return;
            }

            if (contentId == null)
            {
                var content = new Content
                {
                    Title = Content.Title,
                    ContentType = Content.ContentType,
                    Body = Content.Body,
                    OrdinalNumber = Content.OrdinalNumber,
                    AuthorId = User!.Id
                };

                WebPage!.Contents = (await WebPageService.CreateWebPageContentAsync(WebPage!, content)).ToList();
            }
            else
            {
                var content = WebPage!.Contents.FirstOrDefault(content => content.ContentId == contentId);
                if (content == null)
                {
                    ErrorMessage = "Content not found";
                    return;
                }

                content.ContentId = Content.ContentId ?? WebPage!.Contents.Aggregate((cur, max) => cur.ContentId > max.ContentId ? cur : max).ContentId + 1;
                content.Title = Content.Title;
                content.ContentType = Content.ContentType;
                content.Body = Content.Body;
                content.LastModifiedDate = DateTime.Now;
                content.AuthorId = User!.Id;
                content.OrdinalNumber = Content.OrdinalNumber;
            }

            await WebPageService.UpdateOrdinalNumbersAsync(WebPage!);
            CloseConfirmation();
        }

        public Task DeleteContentAsync(int contentId)
        {
            ConfirmationTitle = "Delete Content";
            ConfirmationMessage = "Are you sure you want to delete this content?";
            ConfirmationButtonText = "Delete";
            CancelButtonText = "Cancel";
            OnConfirm = EventCallback.Factory.Create(this, async () => await DeleteContentConfirmedAsync(contentId));
            OnCancel = EventCallback.Factory.Create(this, async () => { CloseConfirmation(); await Task.CompletedTask; });

            ShowConfirmation();

            return Task.CompletedTask;
        }

        private async Task DeleteContentConfirmedAsync(int contentId)
        {
            await ContentService.DeleteContentAsync(contentId);
            WebPage!.Contents = WebPage!.Contents.Where(content => content.ContentId != contentId).ToList();

            await WebPageService.UpdateOrdinalNumbersAsync(WebPage!);
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

        private async Task MoveUpAsync(int ordinalNumber)
        {
            WebPage!.Contents = (await WebPageService.MoveContentUpAsync(WebPage!, ordinalNumber)).ToList();
        }

        private async Task MoveDownAsync(int ordinalNumber)
        {
            WebPage!.Contents = (await WebPageService.MoveContentDownAsync(WebPage!, ordinalNumber)).ToList();
        }

        public sealed class WebPageModel
        {
            [Required]
            public string Title { get; set; } = string.Empty;
        }

        public sealed class ContentModel
        {
            public int? ContentId { get; set; }
            [Required]
            [MinLength(1, ErrorMessage = "You must not leave the Title empty.")]
            public string Title { get; set; } = string.Empty;
            [Required]
            [MinLength(1, ErrorMessage = "You must not leave the ContentType empty.")]
            public string ContentType { get; set; } = string.Empty;
            [Required]
            [MinLength(1, ErrorMessage = "You must not leave the Body empty.")]
            public string Body { get; set; } = string.Empty;
            public int OrdinalNumber { get; set; }
        }
    }
}