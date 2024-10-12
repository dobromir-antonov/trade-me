using MassTransit;
using Microsoft.AspNetCore.Diagnostics;
using SharedKernel.Infrastructure;

namespace Lumina.Api.DependencyInjection;

public static class MassTransitDependencyInjection
{
    public static IServiceCollection AddMassTransitDependency(this IServiceCollection services, ICollection<IModule> modules)
    {
        services.AddMassTransit(bus =>
        {
            bus.SetKebabCaseEndpointNameFormatter();
            bus.UsingInMemory((context, config) => config.ConfigureEndpoints(context));

            // Configure each module's MassTransit setup
            foreach (var module in modules)
            {
                module.ConfigureMassTransit(bus);
            }
           
        });


        return services;
    }

   
   
}
