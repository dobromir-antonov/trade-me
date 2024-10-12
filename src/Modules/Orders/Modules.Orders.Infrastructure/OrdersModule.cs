using Asp.Versioning;
using Asp.Versioning.Builder;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Orders.Application.Tickers.Events;
using Modules.Orders.Domain;
using Modules.Orders.Infrastructure.Idempotence;
using Modules.Orders.Infrastructure.Persistance;
using Modules.Orders.Infrastructure.Persistance.Repositories;
using Modules.Price.IntegrationEvents;
using SharedKernel.Application.Abstraction.Data;
using SharedKernel.Infrastructure;
using SharedKernel.Messaging;
using System.Reflection;

namespace Modules.Orders.Infrastructure;


public class OrdersModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(options => options.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly));

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

        services
            .AddDbContext<OrdersDbContext>();

        services
            .AddScoped<IUnitOfWork, OrdersDbContext>();

        services
            .AddScoped<IOrderRepository, OrderRepository>();

        services
            .AddScoped<IIntegrationEventHandler<TickerPricesChangedIntegrationEvent>, TickerPricesChangedIntegrationEventHandler>();
    }

    public void UseModule(WebApplication app)
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

    public void ConfigureMassTransit(IBusRegistrationConfigurator bus)
    {
        bus.AddConsumer<IdempotentIntegrationEventConsumer<TickerPricesChangedIntegrationEvent>>();
    }

    public Assembly GetApplicationAssembly() => Application.AssemblyReference.Assembly;

    public Assembly GetDomainAssembly() => Domain.AssemblyReference.Assembly;

    public Assembly GetInfrasturctureAssembly() => Infrastructure.AssemblyReference.Assembly;
}
