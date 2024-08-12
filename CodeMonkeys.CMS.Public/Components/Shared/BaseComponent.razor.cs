namespace CodeMonkeys.CMS.Public.Components.Shared
{
    using CodeMonkeys.CMS.Public.Shared;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;

    public abstract partial class BaseComponent : ComponentBase
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Inject]
        protected NavigationManager Navigation { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        protected StatisticsHandler StatisticsHandler { get; set; }

        protected ILogger? Logger { get; set; }
        protected bool isLoading = false;
        protected string errorMessage = string.Empty;
        protected string successMessage = string.Empty;
        protected int PageVisits { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // TODO: Count Stats

            await ExecuteWithLoadingAsync(async () =>
            {
                string pageUrl = Navigation.BaseUri;
                await IncrementPageVisitAsync(pageUrl);
            });
        }

        private async Task IncrementPageVisitAsync(string pageUrl)
        {
            PageVisits = await StatisticsHandler.GetAndUpdatePageVisits(pageUrl);
        }

        protected void SetLoading(bool isLoading)
        {
            this.isLoading = isLoading;
        }

        protected void SetError(string message)
        {
            errorMessage = message;
            successMessage = string.Empty;
        }

        protected void SetSuccess(string message)
        {
            successMessage = message;
            errorMessage = string.Empty;
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

        private void LogError(Exception ex)
        {
            Logger?.LogError(ex.Message);
        }
    }
}