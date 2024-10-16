﻿using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modules.Price.Infrastructure.BackgroundJobs;
using Modules.Price.Infrastructure.Persistance;
using Modules.Price.IntegrationEvents;
using RabbitMQ.Client;
using SharedKernel.Infrastructure;
using System.Reflection;

namespace Modules.Price.Infrastructure;

public class PriceModule : IModule
{
    public Assembly GetApplicationAssembly() => throw new NotImplementedException();
    public Assembly GetDomainAssembly() => Domain.AssemblyReference.Assembly;
    public Assembly GetInfrasturctureAssembly() => Infrastructure.AssemblyReference.Assembly;


    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<PriceDbContext>();
        services.AddSingleton<IPriceGenerator, PriceGenerator>();
        services.AddHostedService<StockPricesChangedPublisherJob>();
    }

    public void UseModule(WebApplication app)
    {
    }

    public void AddMessageBrokerConsumers(IServiceCollection services, IBusRegistrationConfigurator bus)
    {
    }

    public void ConfigureRabbitMqEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.Publish<TickerPricesChangedIntegrationEvent>(p =>
        {
            p.ExchangeType = ExchangeType.Fanout;
        });
    }
}
