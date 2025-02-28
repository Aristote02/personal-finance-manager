using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PersonalFinanceManager.Application.Helpers;
using PersonalFinanceManager.Domain.Entities;
using PersonalFinanceManager.Shared.Constants;

namespace PersonalFinanceManager.Infrastructure.SeedData;

public class SeedManagerUser
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ILogger<SeedManagerUser> _logger;

    public SeedManagerUser(UserManager<AppUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        ILogger<SeedManagerUser> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedManagerUserAsync()
    {
        string managerRole = Roles.Manager;
        if (!await _roleManager.RoleExistsAsync(managerRole))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>(managerRole));
        }

        List<(string, string, string)> managers =
        [
            ("Patrick", "patrick@gmail.com", "Trickpa123@"),
            ("Jamal", "jamaljin@gmail.com", "Jinmal123@")
        ];

        await CreateUserAsync(managers, Roles.Manager);
    }

    private async Task CreateUserAsync(IEnumerable<(string UserName, string Email, string Password)> users, string role)
    {
        foreach (var user in users)
        {
            if (await _userManager.FindByNameAsync(user.UserName) is not null)
            {
                _logger.LogInformation("{Role} user {UserName} already exists", role, user.UserName);
                continue;
            }

            var identityUser = new AppUser
            {
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);
            result.ThrowExceptionIfResultDoNotSucceed(_logger);

            await _userManager.AddRoleToUserAsync(role, identityUser, _logger);
            _logger.LogInformation("Seeded {Role} user {UserName}", role, user.UserName);
        }
    }
}
