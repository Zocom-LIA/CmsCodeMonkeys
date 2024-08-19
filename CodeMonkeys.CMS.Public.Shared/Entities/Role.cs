using Microsoft.AspNetCore.Identity;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class Role : IdentityRole<Guid>
    {
        // IdentityRole<Guid> already includes Id (of type Guid) and Name properties
        // You don't need to redefine RoleID or RoleName

        // Enum to define different roles
        public UserRole UserRoles { get; set; }
    }


    public enum UserRole
    {
        Admin,
        User,
        Unicorn
    }
}