using Asp.Versioning;
using Asp.Versioning.Builder;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modules.Orders.Application.Tickers.Events;
using Modules.Orders.Domain;
using Modules.Orders.Infrastructure.Events;
using Modules.Orders.Infrastructure.Idempotence;
using Modules.Orders.Infrastructure.Outbox;
using Modules.Orders.Infrastructure.OutboxWriter;
using Modules.Orders.Infrastructure.Persistance;
using Modules.Orders.Infrastructure.Persistance.Repositories;
using Modules.Orders.Infrastructure.Validation;
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
        RegisterValidation(services);
        RegisterPersistance(services, configuration);
        RegisterOutbox(services);
        RegisterMediator(services);
        RegisterEndpointVersioning(services);
    }

    public void UseModule(WebApplication app)
    {
        // app.UseMiddleware<EventualConsistencyMiddleware>();

        MapEndpoints(app);

        if (app.Environment.IsDevelopment())
        {
            ApplyMigrations(app);
        }
    }

    public void ConfigureMassTransit(IServiceCollection services, IBusRegistrationConfigurator bus)
    {
        services.AddScoped<IIntegrationEventHandler<TickerPricesChangedIntegrationEvent>, TickerPricesChangedIntegrationEventHandler>();
        bus.AddConsumer<IdempotentIntegrationEventConsumer<TickerPricesChangedIntegrationEvent>>();
    }

    private static void RegisterOutbox(IServiceCollection services)
    {
        services.AddHostedService<OutboxIntegrationEventsPublisherJob>();
        services.AddScoped<OutboxIntegrationEventsWriter>();
    }

    private static void RegisterMediator(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                Application.AssemblyReference.Assembly, 
                AssemblyReference.Assembly);

            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
    }

    private static void RegisterValidation(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly);
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
        services.AddSingleton<PublishDomainEventsInterceptor>();

        services.AddDbContext<OrdersDbContext>((sp, options) => 
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Orders"),
                    cfg => cfg.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema.DefaultSchema))
                .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>()));

        services
            .AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<OrdersDbContext>())
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<ITickerRepository, TickerRepository>();
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
