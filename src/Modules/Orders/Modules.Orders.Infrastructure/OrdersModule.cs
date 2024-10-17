using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Orders.Infrastructure.IntegrationEventConsumers;
using Modules.Orders.IntegrationEvents;
using RabbitMQ.Client;
using SharedKernel.Infrastructure;
using System.Reflection;

namespace Modules.Orders.Infrastructure;


public class OrdersModule : IModule
{
    public Assembly GetApplicationAssembly() => Application.AssemblyReference.Assembly;
    public Assembly GetDomainAssembly() => Domain.AssemblyReference.Assembly;
    public Assembly GetInfrasturctureAssembly() => AssemblyReference.Assembly;

    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOrdersModule(configuration);
    }

    public void UseModule(WebApplication app)
    {
        app.UseOrdersModule();
    }

    public void AddMessageBrokerConsumers(IServiceCollection services, IBusRegistrationConfigurator bus)
    {
        //bus.AddConsumer<TickerPricesChangedConsumer<TickerPricesChangedIntegrationEvent>>();
        bus.AddConsumer<TickerPricesChangedIntegrationEventConsumer>();
    }

    public void ConfigureRabbitMqEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.Publish<OrderPlacedIntegrationEvent>(o => o.ExchangeType = ExchangeType.Fanout);

        cfg.ReceiveEndpoint(
            "orders.ticker-prices-changed", 
            //o => o.ConfigureConsumer<TickerPricesChangedConsumer<TickerPricesChangedIntegrationEvent>>(context)
            o => o.ConfigureConsumer<TickerPricesChangedIntegrationEventConsumer>(context)
        );
    }


}
