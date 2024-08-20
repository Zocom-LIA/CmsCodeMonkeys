using Microsoft.AspNetCore.Identity;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<WebPage> Pages { get; set; } = new List<WebPage>();

        public UserRole UserRoles { get; set; }
    }
}