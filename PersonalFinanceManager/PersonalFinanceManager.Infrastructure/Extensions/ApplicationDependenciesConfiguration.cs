using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceManager.Domain.Entities;
using PersonalFinanceManager.Infrastructure.Contracts.Repositories.Interfaces;
using PersonalFinanceManager.Infrastructure.Data;
using PersonalFinanceManager.Infrastructure.Repositories.Implementations;
using PersonalFinanceManager.Infrastructure.SeedData;
using Serilog;

namespace PersonalFinanceManager.Infrastructure.Extensions;

public static class ApplicationDependenciesConfiguration
{
    public static IServiceCollection AddIdentityDatabase(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
    {
        services
            .AddDbContext<PersonalFinanceDbContext>(options)
            .AddIdentity<AppUser, IdentityRole<Guid>>(optionsIdentity =>
            {
                optionsIdentity.User.RequireUniqueEmail = true;
                optionsIdentity.Password.RequireNonAlphanumeric = true;
                optionsIdentity.Password.RequireLowercase = false;
                optionsIdentity.Password.RequireUppercase = true;
                optionsIdentity.Password.RequireDigit = true;
                optionsIdentity.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                optionsIdentity.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddEntityFrameworkStores<PersonalFinanceDbContext>();

        return services;
    }

    /// <summary>
    /// Extension method to configure serilog
    /// </summary>
    /// <param name="builder">The web application builder</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddLogger(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName();
        });

        return builder.Services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<SeedRoles>()
            .AddScoped<SeedAdmin>()
            .AddScoped<SeedManagerUser>()
            .AddScoped<IIncomeRepository, IncomeRepository>()
            .AddScoped<IRepositoryManager, RepositoryManager>();

        return services;
    }
}