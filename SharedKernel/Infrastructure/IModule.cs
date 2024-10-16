using MassTransit;
using MassTransit.Configuration;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SharedKernel.Infrastructure;

public interface IModule
{
    void AddModule(IServiceCollection services, IConfiguration configuration);
    void UseModule(WebApplication app);
    void AddMessageBrokerConsumers(IServiceCollection services, IBusRegistrationConfigurator bus);
    void ConfigureRabbitMqEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg);

    Assembly GetDomainAssembly();
    Assembly GetApplicationAssembly();
    Assembly GetInfrasturctureAssembly();
}
