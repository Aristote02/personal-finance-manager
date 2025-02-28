using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PersonalFinanceManager.Domain.Entities;
using PersonalFinanceManager.Shared.Constants;

namespace PersonalFinanceManager.Infrastructure.SeedData;

public class SeedAdmin
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SeedAdmin> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SeedAdmin"/>
    /// </summary>
    /// <param name="userManager">user manager</param>
    /// <param name="configuration">configuration</param>
    /// <param name="logger">the logger</param>
    public SeedAdmin(UserManager<AppUser> userManager, IConfiguration configuration, ILogger<SeedAdmin> logger)
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Seeds an admin user with the specified username and password
    /// </summary>
    /// <returns></returns>
    public async Task InitializesAdminAsync()
    {
        var adminUserName = _configuration["AdminCredentials:UserName"];
        var adminEmail = _configuration["AdminCredentials:Email"];
        var adminPassword = _configuration["AdminCredentials:Password"];

        if (adminUserName is null || adminPassword is null || adminEmail == null)
        {
            _logger.LogError("The admin credentials are not properly configured");
            return;
        }

        var adminUser = await _userManager.FindByNameAsync(adminUserName);

        if (adminUser is not null)
            return;

        adminUser = new AppUser { UserName = adminUserName, Email = adminEmail, EmailConfirmed = true };
        var result = await _userManager.CreateAsync(adminUser, adminPassword);

        if (result.Succeeded)
            await _userManager.AddToRoleAsync(adminUser, Roles.Admin);
    }
}