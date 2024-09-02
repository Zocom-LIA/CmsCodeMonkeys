using Microsoft.AspNetCore.Components;

namespace CodeMonkeys.CMS.Public.Components.Shared
{
    public partial class VisitorView : BaseComponent<VisitorView>
    {
        [Parameter]
        public RenderFragment PageBody { get; set; }

        [Parameter]
        public RenderFragment PageTitle { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}