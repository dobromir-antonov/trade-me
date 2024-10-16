using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modules.Orders.Application.Tickers.TickerPricesChanged;
using Modules.Orders.IntegrationEvents;
using Modules.Portfolio.Application.Abstraction;
using Modules.Portfolio.Application.Orders.OrderPlaced;
using Modules.Portfolio.Domain;
using Modules.Portfolio.Domain.Orders.Abstraction;
using Modules.Portfolio.Domain.Tickers.Abstraction;
using Modules.Portfolio.Infrastructure.Persistance;
using Modules.Portfolio.Infrastructure.Persistance.Repositories;
using Modules.Price.IntegrationEvents;
using SharedKernel.Infrastructure;
using SharedKernel.Messaging;

namespace Modules.Portfolio.Infrastructure;

internal static class DependencyInjection
{

    public static IServiceCollection RegisterPortfolioModule(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .RegisterEndpointVersioning()
            .RegisterMediator()
            .RegisterPersistance(configuration)
            .RegisterIntegrationEventHandlers();
    }

    public static WebApplication UsePortfolioModule(this WebApplication app)
    {
        // app.UseMiddleware<EventualConsistencyMiddleware>();

        app.MapEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.ApplyMigrations();
        }

        return app;
    }


    private static IServiceCollection RegisterMediator(this IServiceCollection services)
    {
        return services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                Application.AssemblyReference.Assembly,
                AssemblyReference.Assembly);
        });
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
        services.AddDbContext<PortfolioDbContext>(o =>
            o.UseNpgsql(
                configuration.GetConnectionString("Portfolio"),
                cfg => cfg.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema.DefaultSchema)));

        services.AddDbContext<PortfolioReadOnlyDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("Portfolio")));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<PortfolioDbContext>());
        services.AddScoped<IPortfolioReadOnlyDbContext>(sp => sp.GetRequiredService<PortfolioReadOnlyDbContext>());

        services.AddScoped<ITickerRepository, TickerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }

    private static IServiceCollection RegisterIntegrationEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<IIntegrationEventHandler<TickerPricesChangedIntegrationEvent>, TickerPricesChangedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<OrderPlacedIntegrationEvent>, OrderPlacedIntegrationEventHandler>();

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
            .MapGroup("api/v{version:apiVersion}/portfolio")
            .WithApiVersionSet(apiVersionSet);

        app.MapEndpoints(Application.AssemblyReference.Assembly, routeGroup);
        return app;
    }

    private static WebApplication ApplyMigrations(this WebApplication app)
    {
        IServiceScopeFactory factory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = factory.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
        dbContext.Database.Migrate();
        return app;
    }
}
