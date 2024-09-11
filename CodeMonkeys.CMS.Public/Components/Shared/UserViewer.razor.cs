using Microsoft.AspNetCore.Components;

namespace CodeMonkeys.CMS.Public.Components.Shared
{
    public partial class UserViewer : BaseComponent<UserViewer>
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment? UserViewHeader { get; set; }

        [Parameter] public bool ShowReturnButton { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}