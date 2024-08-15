using Microsoft.AspNetCore.Identity;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<Page> Pages { get; set; } = new List<Page>();
    }
}