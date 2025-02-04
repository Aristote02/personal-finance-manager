using Microsoft.AspNetCore.Identity;
using PersonalFinanceManager.Shared.Constants;

namespace PersonalFinanceManager.Infrastructure.SeedData;

/// <summary>
/// SeedRole represents a class that provides functionality for seeding role in the application
/// </summary>
public class SeedRoles
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    /// <summary>
    /// Initializes a new instance of <see cref="SeedRoles"/> class
    /// </summary>
    /// <param name="roleManager"></param>
    public SeedRoles(RoleManager<IdentityRole<Guid>> roleManager)
    {
        _roleManager = roleManager;
    }

    /// <summary>
    /// Initializes the predefined roles in the application if they do not already exist
    /// </summary>
    /// <returns></returns>
    public async Task InitializeRolesAsync()
    {
        string[] roleNames = [Roles.Admin, Roles.Manager, Roles.User];

        foreach (var roleName in roleNames)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (roleExist)
            {
                continue;
            }

            var role = new IdentityRole<Guid>(roleName);
            await _roleManager.CreateAsync(role);
        }
    }
}