using CodeMonkeys.CMS.Public.Components.Pages;
using CodeMonkeys.CMS.Public.Shared;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CodeMonkeys.CMS.Public.Components.Shared
{
    public abstract partial class BaseComponent<T> : ComponentBase where T : class
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Inject] protected NavigationManager Navigation { get; set; }
        [Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] protected IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] protected ILogger<T> Logger { get; set; }
        [Inject] protected UserManager<User> UserManager { get; set; }

        private bool _isLoading = false;
        private string _errorMessage = string.Empty;
        private string _successMessage = string.Empty;

        protected bool IsLoading => _isLoading;
        protected string ErrorMessage { get; set; }
        protected string SuccessMessage => _successMessage;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected void SetLoading(bool isLoading)
        {
            _isLoading = isLoading;
        }

        protected void SetError(string message)
        {
            _errorMessage = message;
            _successMessage = string.Empty;
        }

        protected void SetSuccess(string message)
        {
            _successMessage = message;
            _errorMessage = string.Empty;
        }

        protected async Task<bool> HasRole(string role)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user.IsInRole(role);
        }

        protected void NavigateTo(string url)
        {
            Navigation.NavigateTo(url);
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

        protected async Task ExecuteWithLoadingAsync(Func<Task> action)
        {
            SetLoading(true);

            try
            {
                await action();
            }
            catch (Exception ex)
            {
                LogError(ex);
                SetError("An error occurred. Please try again later.");
            }
            finally
            {
                SetLoading(false);
            }
        }

        protected void LogError(Exception ex)
        {
            Logger?.LogError(ex.Message);
        }
    }
}