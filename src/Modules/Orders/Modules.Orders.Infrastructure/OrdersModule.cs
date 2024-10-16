using Asp.Versioning;
using Asp.Versioning.Builder;
using FluentValidation;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modules.Orders.Application;
using Modules.Orders.Application.Tickers.TickerPricesChanged;
using Modules.Orders.Domain.Orders.Abstraction;
using Modules.Orders.Domain.Tickers.Abstraction;
using Modules.Orders.Infrastructure.Events;
using Modules.Orders.Infrastructure.Idempotence;
using Modules.Orders.Infrastructure.Outbox;
using Modules.Orders.Infrastructure.OutboxWriter;
using Modules.Orders.Infrastructure.Persistance;
using Modules.Orders.Infrastructure.Persistance.Repositories;
using Modules.Orders.Infrastructure.Validation;
using Modules.Orders.IntegrationEvents;
using Modules.Price.IntegrationEvents;
using RabbitMQ.Client;
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
        services.RegisterOrdersModule(configuration);
    }

    public void UseModule(WebApplication app)
    {
        app.UseOrdersModule();
    }

    public void AddMessageBrokerConsumers(IServiceCollection services, IBusRegistrationConfigurator bus)
    {
        bus.AddConsumer<IdempotentIntegrationEventConsumer<TickerPricesChangedIntegrationEvent>>();
    }

    public void ConfigureRabbitMqEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.Publish<OrderPlacedIntegrationEvent>(o => o.ExchangeType = ExchangeType.Fanout);

        cfg.ReceiveEndpoint(
            "orders.ticker-prices-changed", 
            o => o.ConfigureConsumer<IdempotentIntegrationEventConsumer<TickerPricesChangedIntegrationEvent>>(context)
        );
    }


}
