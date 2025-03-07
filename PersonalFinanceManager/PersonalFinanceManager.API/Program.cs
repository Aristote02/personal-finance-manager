using PersonalFinanceManager.API.Extensions;
using PersonalFinanceManager.API.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.ConfigureCrossOriginRessourceSharing();
builder.ConfigureDatabase();
builder.ConfigureAuthentication();
builder.ConfigureAuthorization();
builder.ConfigureServices()
    .ConfigureSwaggerGen();

var app = builder.Build();

var scope = app.Services.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
await app.UseMigration(logger);

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
app.UseCors();

app.UseExceptionMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
