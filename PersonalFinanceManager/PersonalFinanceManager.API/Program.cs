using PersonalFinanceManager.API.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.ConfigureCrossOriginRessourceSharing();
builder.ConfigureDatabase();
builder.ConfigureJwtAuthentication();
builder.ConfigureServices()
    .ConfigureSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var scope = app.Services.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
await app.UseMigration(logger);

app.UseAuthorization();

app.MapControllers();

app.Run();
