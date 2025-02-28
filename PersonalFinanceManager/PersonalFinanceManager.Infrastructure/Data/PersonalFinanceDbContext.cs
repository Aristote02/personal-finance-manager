using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManager.Domain.Entities;
using PersonalFinanceManager.Infrastructure.Configurations;

namespace PersonalFinanceManager.Infrastructure.Data;

/// <summary>
/// Represents the database context for Identity-related functionality
/// </summary>
public class PersonalFinanceDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    /// <summary>
    /// Initializes a new instance of <see cref="PersonalFinanceDbContext"/>
    /// </summary>
    /// <param name="options">The <see cref="DbContextOptions"/>for configuring the context</param>
    public PersonalFinanceDbContext(DbContextOptions<PersonalFinanceDbContext> options)
        : base(options)
    {

    }

    /// <summary>
    /// Configures the model using entity configurations from the assembly
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> instance</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityUserConfiguration).Assembly);
    }
}