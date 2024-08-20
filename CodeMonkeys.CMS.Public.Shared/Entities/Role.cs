using Microsoft.AspNetCore.Identity;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class Role : IdentityRole<Guid>
    {
    }


    public enum UserRole
    {
        Admin,
        User
    }
}