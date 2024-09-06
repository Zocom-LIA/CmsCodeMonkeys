using Microsoft.AspNetCore.Components;

namespace CodeMonkeys.CMS.Public.Components.Shared.UI
{
    public partial class ConfirmationDialog : ComponentBase
    {
        private bool IsVisible { get; set; }

        [Parameter] public string Title { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public string? ConfirmationMessage { get; set; }

        [Parameter] public string CancelButtonText { get; set; } = "No";

        [Parameter] public string ConfirmationButtonText { get; set; } = "Yes";

        [Parameter] public EventCallback OnCancel { get; set; }

        [Parameter] public EventCallback OnConfirm { get; set; }

        protected override void OnInitialized()
        {
            IsVisible = false;
        }

        public void Show()
        {
            IsVisible = true;
            StateHasChanged();
        }

        public void Hide()
        {
            IsVisible = false;
            StateHasChanged();
        }

        private void Cancel()
        {
            OnCancel.InvokeAsync(null);
            Hide();
        }

        private void Confirm()
        {
            OnConfirm.InvokeAsync(null);
            Hide();
        }
    }
}