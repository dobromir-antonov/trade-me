using Asp.Versioning;
using Asp.Versioning.Builder;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modules.Orders.Application;
using Modules.Orders.Domain.Orders.Abstraction;
using Modules.Orders.Domain.Tickers.Abstraction;
using Modules.Orders.Infrastructure.EventualConsistency;
using Modules.Orders.Infrastructure.Outbox;
using Modules.Orders.Infrastructure.OutboxWriter;
using Modules.Orders.Infrastructure.Persistance;
using Modules.Orders.Infrastructure.Persistance.Repositories;
using Modules.Orders.Infrastructure.Validation;
using SharedKernel.Infrastructure;

namespace Modules.Orders.Infrastructure;

internal static class DependencyInjection
{

    public static IServiceCollection RegisterOrdersModule(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .RegisterEndpointVersioning()
            .RegisterValidation()
            .RegisterPersistance(configuration)
            .RegisterOutbox()
            .RegisterMediator();
    }

    public static WebApplication UseOrdersModule(this WebApplication app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        app.MapEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.ApplyMigrations();
        }

        return app;
    }


    private static IServiceCollection RegisterOutbox(this IServiceCollection services)
    {
        services.AddHostedService<OutboxIntegrationEventsPublisherJob>();
        services.AddScoped<OutboxIntegrationEventsWriter>();
        return services;
    }

    private static IServiceCollection RegisterMediator(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                Application.AssemblyReference.Assembly,
                AssemblyReference.Assembly);

            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }

    private static IServiceCollection RegisterValidation(this IServiceCollection services)
    {
        return services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly);
    }

    private static IServiceCollection RegisterEndpointVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }

    private static IServiceCollection RegisterPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrdersDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Orders"),
                    cfg => cfg.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema.DefaultSchema)));

        services
            .AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<OrdersDbContext>())
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<ITickerRepository, TickerRepository>();

        return services;
    }


    private static WebApplication MapEndpoints(this WebApplication app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder routeGroup = app
            .MapGroup("api/v{version:apiVersion}/orders")
            .WithApiVersionSet(apiVersionSet);

        app.MapEndpoints(Application.AssemblyReference.Assembly, routeGroup);
        return app;
    }

    private static WebApplication ApplyMigrations(this WebApplication app)
    {
        IServiceScopeFactory factory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = factory.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
        dbContext.Database.Migrate();
        return app;
    }
}
