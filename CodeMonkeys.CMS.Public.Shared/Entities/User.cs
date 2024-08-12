using Microsoft.AspNetCore.Identity;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class User : IdentityUser<Guid>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ICollection<Page> Pages { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}