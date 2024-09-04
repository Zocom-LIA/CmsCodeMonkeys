using CodeMonkeys.CMS.Public.Components.Pages;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CodeMonkeys.CMS.Public.Components.Shared
{
    [Authorize(Roles = "User, Admin")]
    public abstract partial class AuthenticationBaseComponent<T> : BaseComponent<T> where T : class
    {
        [Inject] protected UserManager<User> UserManager { get; set; }
        [Inject] protected ISiteService SiteService { get; set; }
        [Inject] protected IWebPageService WebPageService { get; set; }
        [Inject] protected IContentService ContentService { get; set; }

        protected User? User { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            User = await GetCurrentUserAsync();

            if (User == null)
            {
                Logger.LogWarning("Authenticated User is not authenticated");
                Navigation.NavigateTo("account/login");
            }
        }

        protected async Task<User?> GetCurrentUserAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                return await UserManager.GetUserAsync(user);
            }

            return null;
        }
    }
}