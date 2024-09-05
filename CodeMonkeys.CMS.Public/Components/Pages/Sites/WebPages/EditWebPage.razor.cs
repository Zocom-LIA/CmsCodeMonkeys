using CodeMonkeys.CMS.Public.Components.Shared;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Services;

using Microsoft.AspNetCore.Components;

using System.ComponentModel.DataAnnotations;

namespace CodeMonkeys.CMS.Public.Components.Pages.Sites.WebPages
{
    public partial class EditWebPage : BaseComponent<EditWebPage>
    {
        [Inject]
        public ISiteService SiteService { get; set; }
        [Inject]
        public IWebPageService WebPageService { get; set; }
        [Inject]
        public IContentService ContentService { get; set; }

        [SupplyParameterFromForm]
        private InputModel Input { get; set; } = new InputModel();

        [Parameter]
        public int siteId { get; set; }
        public Site? Site { get; set; }

        [Parameter]
        public int webPageId { get; set; }
        public WebPage? WebPage { get; set; }

        private ContentModel? Content { get; set; }
        private ConfirmationDialog? Confirmation { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            User? user = await GetCurrentUserAsync();
            if (user == null)
            {
                Logger.LogDebug("Authenticated User is not authenticated");
                return;
            }

            Site = await SiteService.GetUserSiteAsync(user.Id, siteId);
            if (Site == null)
            {
                Logger.LogDebug($"Site with ID '{siteId}' for User with ID '{user.Id}' not found.");
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

        private Task AddContent()
        {
            Content = new ContentModel()
            {
                ContentType = ContentTypes.Text.ToString(),
                OrdinalNumber = WebPage!.Contents.Count() + 1
            };
            return Task.CompletedTask;
        }

        private Task EditContent(int contentId)
        {
            var content = WebPage!.Contents.FirstOrDefault(c => c.ContentId == contentId);

            if (content == null)
            {
                ErrorMessage = "Content not found";
                return Task.CompletedTask;
            }

            Content = new ContentModel()
            {
                ContentId = content.ContentId,
                Title = content.Title,
                ContentType = content.ContentType,
                Body = content.Body,
                OrdinalNumber = content.OrdinalNumber
            };

            return Task.CompletedTask;
        }

        private Task DeleteContent(int contentId)
        {
            var content = WebPage!.Contents.FirstOrDefault(c => c.ContentId == contentId);

            if (content == null)
            {
                ErrorMessage = "Content not found";
                return Task.CompletedTask;
            }

            ShowConfirmation("Are you sure you want to delete this content?", async () =>
            {
                await ContentService.DeleteContentAsync(contentId);

                WebPage = await WebPageService.GetSiteWebPageAsync(siteId, webPageId);

                // TODO: Remember to update Ordinal numbers correctly.

                Confirmation = null;
            },
            () =>
            {
                Confirmation = null;
                return Task.CompletedTask;
            });

            return Task.CompletedTask;
        }

        private void ShowConfirmation(string message, Func<Task> onConfirm, Func<Task> onCancel)
        {
            Confirmation = new ConfirmationDialog(message, onConfirm, onCancel);
        }


        private async Task CreateOrUpdateContent()
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

            if (Content.OrdinalNumber <= 0 || Content.OrdinalNumber > WebPage.Contents.Count() + 1)
            {
                ErrorMessage = $"Ordinal Number must be between 1 and {WebPage.Contents.Count() + 1}.";
                return;
            }

            if (Content.ContentId == null)
            {
                var content = new Content()
                {
                    Title = Content.Title,
                    ContentType = Content.ContentType,
                    Body = Content.Body,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    OrdinalNumber = Content.OrdinalNumber,
                    AuthorId = WebPage.AuthorId
                };

                WebPage.Contents.Add(content);
                Content = null;
                await WebPageService.UpdateWebPageAsync(WebPage);
            }
            else
            {
                var content = WebPage.Contents.FirstOrDefault(c => c.ContentId == Content.ContentId);
                if (content == null)
                {
                    ErrorMessage = "Content not found";
                    return;
                }

                content.Title = Content.Title;
                content.ContentType = Content.ContentType;
                content.Body = Content.Body;
                content.LastModifiedDate = DateTime.Now;
                content.OrdinalNumber = Content.OrdinalNumber;

                await WebPageService.UpdateWebPageAsync(WebPage);

                Content = null;
            }
        }

        private async Task MoveUp(int ordinalNumber)
        {
            WebPage!.Contents = (await WebPageService.MoveContentUpAsync(WebPage!, ordinalNumber)).ToList();
        }

        private async Task MoveDown(int ordinalNumber)
        {
            WebPage!.Contents = (await WebPageService.MoveContentDownAsync(WebPage!, ordinalNumber)).ToList();
        }

        private sealed class InputModel
        {
            [Required]
            public string Title { get; set; } = string.Empty;
        }

        private sealed class ContentModel
        {
            public int? ContentId { get; set; }
            [Required]
            public string Title { get; set; } = string.Empty;
            [Required]
            public string ContentType { get; set; } = string.Empty;
            [Required]
            public string Body { get; set; } = string.Empty;
            [Required]
            public int OrdinalNumber { get; set; }
        }
    }

    public sealed class ConfirmationDialog
    {
        public ConfirmationDialog(string message, Func<Task> onConfirm, Func<Task> onCancel)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            OnConfirm = onConfirm ?? throw new ArgumentNullException(nameof(onConfirm));
            OnCancel = onCancel ?? throw new ArgumentNullException(nameof(onCancel));
        }

        public string Message { get; set; }

        public Func<Task> OnConfirm { get; set; }

        public Func<Task> OnCancel { get; set; }
    }
}