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
using RabbitMQ.Client;
using SharedKernel.Infrastructure;
using SharedKernel.Messaging;
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
        bus.AddConsumer<IdempotentIntegrationEventConsumer<TickerPricesChangedIntegrationEvent>>();
        bus.AddConsumer<IdempotentIntegrationEventConsumer<OrderPlacedIntegrationEvent>>();
    }

    public void ConfigureRabbitMqEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.ReceiveEndpoint(
            "portfolio.ticker-prices-changed",
            o => o.ConfigureConsumer<IdempotentIntegrationEventConsumer<TickerPricesChangedIntegrationEvent>>(context)
        );

        cfg.ReceiveEndpoint(
            "portfolio.order-placed",
            o => o.ConfigureConsumer<IdempotentIntegrationEventConsumer<OrderPlacedIntegrationEvent>>(context)
        );
    }

}
