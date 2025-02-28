using Microsoft.EntityFrameworkCore;
using PersonalFinanceManager.Infrastructure.Data;
using PersonalFinanceManager.Infrastructure.SeedData;

namespace PersonalFinanceManager.API.Extensions;

/// <summary>
/// Provides extension methods for configuring and managing application dependencies
/// </summary>
public static partial class ApplicationDependenciesConfiguration
{
    /// <summary>
    /// Applies pending database migrations and seeds initial data
    /// </summary>
    /// <param name="application">The Web application instance</param>
    /// <param name="logger">The logger</param>
    /// <returns></returns>
    public static async Task UseMigration(this WebApplication application, ILogger logger)
    {
        var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PersonalFinanceDbContext>();
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (!pendingMigrations.Any())
        {
            logger.LogInformation("No pending migrations was found");
            await SeedDatabase(scope.ServiceProvider);

            return;
        }

        await dbContext.Database.MigrateAsync();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            await SeedDatabase(scope.ServiceProvider);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            logger.LogInformation("Migration completed successfully");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "An error occured during migration");
            throw;
        }
    }

    private static async Task SeedDatabase(IServiceProvider serviceProvider)
    {
        var seedRole = serviceProvider.GetRequiredService<SeedRoles>();
        await seedRole.InitializeRolesAsync();

        var seedAdmin = serviceProvider.GetRequiredService<SeedAdmin>();
        await seedAdmin.InitializesAdminAsync();

        var seedManager = serviceProvider.GetRequiredService<SeedManagerUser>();
        await seedManager.SeedManagerUserAsync();
    }
}