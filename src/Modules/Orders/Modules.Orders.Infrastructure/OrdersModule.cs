using Asp.Versioning;
using Asp.Versioning.Builder;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modules.Orders.Application.Tickers.Events;
using Modules.Orders.Domain;
using Modules.Orders.Infrastructure.Idempotence;
using Modules.Orders.Infrastructure.Persistance;
using Modules.Orders.Infrastructure.Persistance.Repositories;
using Modules.Price.IntegrationEvents;
using SharedKernel.Infrastructure;
using SharedKernel.Messaging;
using System.Reflection;

namespace Modules.Orders.Infrastructure;


public class OrdersModule : IModule
{
    public Assembly GetApplicationAssembly() => Application.AssemblyReference.Assembly;
    public Assembly GetDomainAssembly() => Domain.AssemblyReference.Assembly;
    public Assembly GetInfrasturctureAssembly() => AssemblyReference.Assembly;

    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        RegisterMediator(services);
        RegisterEndpointVersioning(services);
        RegisterPersistance(services, configuration);
        RegisterIntegrationEventHandlers(services);
    }

    public void UseModule(WebApplication app)
    {
        MapEndpoints(app);

        if (app.Environment.IsDevelopment())
        {
            ApplyMigrations(app);
        }
    }

    public void ConfigureMassTransit(IBusRegistrationConfigurator bus)
    {
        bus.AddConsumer<IdempotentIntegrationEventConsumer<TickerPricesChangedIntegrationEvent>>();
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
        services.AddDbContext<OrdersDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Orders")));

        services.AddTransient<IUnitOfWork, UnitOfWork>();

        services
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<ITickerRepository, TickerRepository>();
    }

    private static void RegisterIntegrationEventHandlers(IServiceCollection services)
    {
        services
             .AddScoped<IIntegrationEventHandler<TickerPricesChangedIntegrationEvent>, TickerPricesChangedIntegrationEventHandler>();
    }

    private static void MapEndpoints(WebApplication app)
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
    }

    private static void ApplyMigrations(WebApplication app)
    {
        IServiceScopeFactory factory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = factory.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
        dbContext.Database.Migrate();
    }

}
