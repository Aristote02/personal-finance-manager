using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using PersonalFinanceManager.Domain.ErrorModel;
using PersonalFinanceManager.Domain.Exceptions;
using System.Net.Mime;

namespace PersonalFinanceManager.API.Middlewares;

public static class GlobalExceptionHandlingMiddleware
{
    public static void UseExceptionMiddleware(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                var (statusCode, message) = contextFeature?.Error switch
                {
                    NotFoundException => (StatusCodes.Status404NotFound, contextFeature.Error.Message),
                    AlreadyExistsException => (StatusCodes.Status400BadRequest, contextFeature.Error.Message),
                    InvalidCredentialsException => (StatusCodes.Status401Unauthorized, contextFeature.Error.Message),
                    UnauthorizedException => (StatusCodes.Status401Unauthorized, contextFeature.Error.Message),
                    ForbiddenException => (StatusCodes.Status403Forbidden, contextFeature.Error.Message),
                    ValidationException => (StatusCodes.Status400BadRequest, contextFeature.Error.Message),
                    _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
                };

                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsJsonAsync(new ErrorDetails
                {
                    StatusCode = statusCode,
                    Message = message
                });
            });
        });
    }
}
