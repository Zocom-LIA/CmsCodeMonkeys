using CodeMonkeys.CMS.Public.Components.Shared.UI;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

using System.ComponentModel.DataAnnotations;

namespace CodeMonkeys.CMS.Public.Components.Shared
{
    public partial class SitesEditor : AuthenticationBaseComponent<SitesEditor>
    {
        [Inject] protected UserManager<User> UserManager { get; set; }
        [Inject] protected ISiteService SiteService { get; set; }
        [Inject] protected IWebPageService WebPageService { get; set; }
        [Inject] protected IContentService ContentService { get; set; }

        [SupplyParameterFromForm]
        private InputModel Input { get; set; } = new InputModel();

        protected User? User { get; set; }
        protected Site? Site { get; set; }
        protected WebPage? WebPage { get; set; }

        protected int siteId = 1;
        protected int webPageId = 1;

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

            WebPage = await WebPageService.GetSiteWebPageAsync(siteId, webPageId);

            if (WebPage == null)
            {
                Logger.LogDebug($"WebPage with ID '{webPageId}' for site with ID '{siteId}' not found.");
                ErrorMessage = "There is no such webpage available to edit";
                return;
            }

            Input.Title = WebPage.Title;
            WebPage.Contents = WebPage.Contents.OrderBy(content => content.OrdinalNumber).ToList();
        }

        public sealed class InputModel
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