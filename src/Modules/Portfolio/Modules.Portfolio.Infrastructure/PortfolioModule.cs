using Asp.Versioning;
using Asp.Versioning.Builder;
using MassTransit;
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
using Modules.Portfolio.Infrastructure.Idempotence;
using Modules.Portfolio.Infrastructure.Persistance;
using Modules.Portfolio.Infrastructure.Persistance.Repositories;
using Modules.Price.IntegrationEvents;
using SharedKernel.Infrastructure;
using SharedKernel.Messaging;
using System.Reflection;

namespace Modules.Portfolio.Infrastructure;

public class PortfolioModule : IModule
{
    public Assembly GetApplicationAssembly() => Application.AssemblyReference.Assembly;

    public Assembly GetDomainAssembly() => Domain.AssemblyReference.Assembly;

    public Assembly GetInfrasturctureAssembly() => Infrastructure.AssemblyReference.Assembly;


    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        RegisterMediator(services);
        RegisterEndpointVersioning(services);
        RegisterPersistance(services, configuration);
    }

    public void UseModule(WebApplication app)
    {
        MapEndpoints(app);

        if (app.Environment.IsDevelopment())
        {
            ApplyMigrations(app);
        }
    }

    public void ConfigureMassTransit(IServiceCollection services, IBusRegistrationConfigurator bus)
    {
        services.AddScoped<IIntegrationEventHandler<TickerPricesChangedIntegrationEvent>, TickerPricesChangedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<OrderPlacedIntegrationEvent>, OrderPlacedIntegrationEventHandler>();

        bus.AddConsumer<IdempotentIntegrationEventConsumer<TickerPricesChangedIntegrationEvent>>();
        bus.AddConsumer<IdempotentIntegrationEventConsumer<OrderPlacedIntegrationEvent>>();
    }


    private static void RegisterMediator(IServiceCollection services)
    {
        services.AddMediatR(options => options.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly));
    }

    private static void RegisterEndpointVersioning(IServiceCollection services)
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

    }

    private static void RegisterPersistance(IServiceCollection services, IConfiguration configuration)
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
        //services.AddScoped<IUserPortfolioRepository, UserPortfolioRepository>();
    }

    private static void MapEndpoints(WebApplication app)
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
    }

    private static void ApplyMigrations(WebApplication app)
    {
        IServiceScopeFactory factory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = factory.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
        dbContext.Database.Migrate();
    }
}
