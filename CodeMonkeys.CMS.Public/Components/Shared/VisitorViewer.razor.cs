using Microsoft.AspNetCore.Components;

using System.Security.Claims;

namespace CodeMonkeys.CMS.Public.Components.Shared
{
    public partial class VisitorViewer : BaseComponent<VisitorViewer>
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string? Title { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}