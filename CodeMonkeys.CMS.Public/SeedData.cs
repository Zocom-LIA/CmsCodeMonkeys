
using CodeMonkeys.CMS.Public.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;

public class SeedData
{
    public static async Task InitializeRolesAsync(RoleManager<Role> roleManager, ILoggerFactory loggerFactory)
    {
        ILogger<SeedData> logger = loggerFactory.CreateLogger<SeedData>();
        // Kontrollera om rollen finns och om inte, skapa den
        foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
        {
            var roleName = role.ToString();
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await roleManager.CreateAsync(new Role
                {
                    Name = roleName
                });

                if (!roleResult.Succeeded)
                {
                    logger.LogError("Error creating role {RoleName}", roleName);
                }
            }
        }
    }
}