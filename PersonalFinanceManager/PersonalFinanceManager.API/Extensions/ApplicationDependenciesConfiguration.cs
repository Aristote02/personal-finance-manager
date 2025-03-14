using FluentValidation;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PersonalFinanceManager.Application;
using PersonalFinanceManager.Application.Contracts.Services.Interfaces;
using PersonalFinanceManager.Application.Profiles;
using PersonalFinanceManager.Application.Services.Implementations;
using PersonalFinanceManager.Infrastructure.Extensions;
using PersonalFinanceManager.Shared.Helpers;
using System.Text;

namespace PersonalFinanceManager.API.Extensions;

/// <summary>
/// Configure all of the api's services
/// </summary>
public static partial class ApplicationDependenciesConfiguration
{
    /// <summary>
    /// Add and configure all the service of the api
    /// </summary>
    /// <param name="builder">The web application builder</param>
    /// <returns>A <see cref="IServiceCollection"/></returns>
    public static IServiceCollection ConfigureServices(this WebApplicationBuilder builder)
    {
        builder
            .AddLogger()
            .AddRepositories()
            .AddServices()
            .AddMediatR()
            .AddValidators()
            .AddAutoMapper(typeof(MappingProfile));

        return builder.Services;
    }

    /// <summary>
	/// Configures Cross-Origin Resource Sharing (CORS) for the application
	/// </summary>
	/// <param name="builder">The WebApplicationBuilder used to configure services and middleware</param>
	public static void ConfigureCrossOriginRessourceSharing(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
    }

    /// <summary>
	/// Adds services to the <paramref name="services"/> collection
	/// </summary>
	///<param name="services">The <see cref="IServiceCollection"/> to which services are added</param>
	/// <returns>The service collection</returns>
	public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IIncomeService, IncomeService>()
            .AddScoped<IServiceManager, ServiceManager>();

        return services;
    }

    /// <summary>
	/// Configures MediatR
	/// </summary>
	/// <param name="services"></param>
	/// <returns></returns>
	public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly));

        return services;
    }

    /// <summary>
    /// Configure the services database
    /// </summary>
    /// <param name="builder">The web application builder</param>
    public static IServiceCollection ConfigureDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("PersonalFinanceConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("The connection string is missing or not configured");
        }

        return builder.Services.AddIdentityDatabase(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }

    /// <summary>
	/// Configures fluent validation
	/// </summary>
	/// <param name="services"></param>
	/// <returns>The service collection with added validators</returns>
	public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly);
    }

    /// <summary>
	/// Configure swagger for API documentation
	/// </summary>
	/// <param name="builder"></param>
	public static IServiceCollection ConfigureSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please Enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "Jwt",
                Scheme = "bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }

    /// <summary>
    /// Configure authentication
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder</param>
    /// <exception cref="InvalidOperationException"></exception>
    public static IServiceCollection ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        // Configure JWT
        var jwtSection = builder.Configuration.GetSection("Jwt");
        if (!jwtSection.Exists() || !ValidateJwtSettings(jwtSection))
        {
            throw new InvalidOperationException("JWT configuration values are missing or invalid");
        }

        builder.Services.Configure<JwtSettings>(jwtSection);

        // Configure Google
        var googleSection = builder.Configuration.GetSection("GoogleAuth");
        if (!googleSection.Exists() || !ValidateGoogleSettings(googleSection))
        {
            throw new InvalidOperationException("Google Configuration values are missing or invalid");
        }

        builder.Services.Configure<GoogleSettings>(googleSection);

        // Add authentication schemes
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!))
                };
            })
            .AddGoogle(options =>
            {
                var googleSettings = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<GoogleSettings>>().Value;
                options.ClientId = googleSettings.ClientId;
                options.ClientSecret = googleSettings.ClientSecret;
                options.SaveTokens = true;
            });

        return builder.Services;
    }

    public static IServiceCollection ConfigureAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("JwtOrGoogle", policy =>
            {
                policy.AddAuthenticationSchemes(
                    JwtBearerDefaults.AuthenticationScheme,
                    GoogleDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser();
            });
        });

        return builder.Services;
    }

    /// <summary>
	/// Method to validate jwt
	/// </summary>
	/// <param name="jwtSection"></param>
	/// <returns></returns>
	private static bool ValidateJwtSettings(IConfigurationSection jwtSection)
    {
        return !string.IsNullOrEmpty(jwtSection["Issuer"]) &&
               !string.IsNullOrEmpty(jwtSection["Audience"]) &&
               !string.IsNullOrEmpty(jwtSection["Key"]);
    }

    private static bool ValidateGoogleSettings(IConfigurationSection googleSection)
    {
        return !string.IsNullOrEmpty(googleSection["ClientId"]) &&
               !string.IsNullOrEmpty(googleSection["ClientSecret"]);
    }
}
