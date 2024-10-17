using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Orders.IntegrationEvents;
using Modules.Portfolio.Infrastructure.IntegrationEventConsumers;
using Modules.Price.IntegrationEvents;
using SharedKernel.Infrastructure;
using System.Reflection;

namespace Modules.Portfolio.Infrastructure;

public class PortfolioModule : IModule
{
    public Assembly GetApplicationAssembly() => Application.AssemblyReference.Assembly;

    public Assembly GetDomainAssembly() => Domain.AssemblyReference.Assembly;

    public Assembly GetInfrasturctureAssembly() => AssemblyReference.Assembly;


    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterPortfolioModule(configuration);
    }

    public void UseModule(WebApplication app)
    {
        app.UsePortfolioModule();
    }

    public void AddMessageBrokerConsumers(IServiceCollection services, IBusRegistrationConfigurator bus)
    {
        //bus.AddConsumer<IdempotentIntegrationEventConsumer<TickerPricesChangedIntegrationEvent>>();
        //bus.AddConsumer<IdempotentIntegrationEventConsumer<OrderPlacedIntegrationEvent>>();
        bus.AddConsumer<TickerPricesChangedIntegrationEventConsumer>();
        bus.AddConsumer<OrderPlacedIntegrationEventConsumer>();
    }

    public void ConfigureRabbitMqEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.ReceiveEndpoint(
            "portfolio.ticker-prices-changed",
            //o => o.ConfigureConsumer<IdempotentIntegrationEventConsumer<TickerPricesChangedIntegrationEvent>>(context)
            o => o.ConfigureConsumer<TickerPricesChangedIntegrationEventConsumer> (context)
        );

        cfg.ReceiveEndpoint(
            "portfolio.order-placed",
            //o => o.ConfigureConsumer<IdempotentIntegrationEventConsumer<OrderPlacedIntegrationEvent>>(context)
            o => o.ConfigureConsumer<OrderPlacedIntegrationEventConsumer>(context)
        );
    }

}
