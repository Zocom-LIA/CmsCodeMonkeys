using Microsoft.AspNetCore.Components;

namespace CodeMonkeys.CMS.Public.Components.Shared.UI;

public partial class ConfigurationDialog : ComponentBase
{
    private bool _isVisible = true;

    [Parameter] public string? Title { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? ConfigurationMessage { get; set; }
    [Parameter] public string CancelButtonText { get; set; } = "Cancel";
    [Parameter] public string ConfigurationButtonText { get; set; } = "Yes";
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnConfirm { get; set; }

    public bool IsVisible => _isVisible;

    protected override void OnInitialized()
    {
        _isVisible = false;
    }

    public void Show()
    {
        _isVisible = true;
        StateHasChanged();
    }

    public void Hide()
    {
        _isVisible = false;
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