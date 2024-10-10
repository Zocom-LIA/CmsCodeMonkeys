using Microsoft.AspNetCore.Components;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public interface IRenderable
    {
        public RenderFragment RenderContent();
    }
}