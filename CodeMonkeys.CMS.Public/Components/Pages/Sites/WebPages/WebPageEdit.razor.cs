﻿using CodeMonkeys.CMS.Public.Components.Shared;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Extensions;
using CodeMonkeys.CMS.Public.Shared.Repository;
using CodeMonkeys.CMS.Public.Shared.Services;

using Microsoft.AspNetCore.Components;

using System.ComponentModel.DataAnnotations;

namespace CodeMonkeys.CMS.Public.Components.Pages.Sites.WebPages
{
    public partial class WebPageEdit : AuthenticationBaseComponent<WebPageEdit>
    {
        [SupplyParameterFromForm]
        private InputModel Input { get; set; } = new InputModel();

        [Parameter]
        public int siteId { get; set; }
        public Site? Site { get; set; }

        [Parameter]
        public int webPageId { get; set; }
        public WebPage? WebPage { get; set; }


        private HashSet<Content> updatedItems = new();
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
            WebPage.Contents = WebPage.Contents.OrderBy(content => content.OrdinalNumber).ToList();
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
                ContentType = ContentTypes.Paragraph.ToString(),
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

                if (WebPage?.Contents != null)
                {
                    WebPage.Contents = WebPage.Contents
                        .OrderBy(c => c.OrdinalNumber)
                        .Select((c, i) =>
                        {
                            c.OrdinalNumber = i + 1;
                            return c;
                        })
                        .ToList();
                }

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

            if (WebPage?.Contents == null)
            {
                ErrorMessage = "WebPage contents are not available";
                return;
            }

            if (Content.OrdinalNumber <= 0 || Content.OrdinalNumber > WebPage.Contents.Count() + 1)
            {
                ErrorMessage = "Invalid data";
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
                    OrdinalNumber = WebPage.Contents.Count(),
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

                WebPage.Contents.Select(c => c.ContentId == content.ContentId ? content : c);

                await WebPageService.UpdateOrdinalNumbersAsync(WebPage, true);
                //await WebPageService.UpdateWebPageAsync(WebPage);

                Content = null;
            }
        }

        private void NavigateToWebPage(int webPageId, NavigationActions action)
        {
            Navigation.NavigateTo($"/sites/{siteId}");
        }

        private async Task MoveUp(int ordinalNumber)
        {
            WebPage!.Contents = (await WebPageService.MoveContentUpAsync(WebPage!, ordinalNumber)).ToList();
        }

        private async Task MoveDown(int ordinalNumber)
        {
            WebPage!.Contents = (await WebPageService.MoveContentDownAsync(WebPage!, ordinalNumber)).ToList();
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
            public string Title { get; set; } = string.Empty;
            [Required]
            public string ContentType { get; set; } = string.Empty;
            [Required]
            public string Body { get; set; } = string.Empty;
            [Required]
            public int OrdinalNumber { get; set; }
        }
    }
}