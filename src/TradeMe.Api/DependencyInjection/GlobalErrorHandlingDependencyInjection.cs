using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TradeMe.Api.DependencyInjection;

public static class GlobalErrorHandlingDependencyInjection
{
    public static IServiceCollection AddGlobalErrorHandling(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
            };
        });


        return services;
    }

    public static WebApplication UseGlobalErrorHandling(this WebApplication app)
    {
        app.UseExceptionHandler("/error");

        app.Map("/error", (HttpContext httpContext) => 
        {
            Exception? exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (exception is null)
            {
                return Results.Problem();
            }

            if (exception is FluentValidation.ValidationException fluentException)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = "one or more validation errors occurred.",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Extensions =
                    {
                        ["errors"] = fluentException.Errors.Select(e => e.ErrorMessage)
                    }
                };

                return Results.Problem(problemDetails);
            }

            return Results.Problem(detail: exception.Message);
        });

        return app;
    }
   
}
