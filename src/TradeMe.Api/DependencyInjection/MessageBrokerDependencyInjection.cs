using MassTransit;
using SharedKernel.Infrastructure;

namespace TradeMe.Api.DependencyInjection;

public static class MessageBrokerDependencyInjection
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, ICollection<IModule> modules)
    {
        services.AddMassTransit(bus =>
        {
            bus.SetKebabCaseEndpointNameFormatter();

            // Configure each module's MassTransit setup
            foreach (var module in modules)
            {
                module.AddMessageBrokerConsumers(services, bus);
            }

            bus.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                foreach (var module in modules)
                {
                    module.ConfigureRabbitMqEndpoints(context, cfg);
                }
            });
        });


        return services;
    }
}
